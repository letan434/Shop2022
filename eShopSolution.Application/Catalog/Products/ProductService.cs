using eShopSolution.Application.Common;
using eShopSolution.Application.MachineLearning.Common;
using eShopSolution.Application.MachineLearning.DataModels;
using eShopSolution.Application.MachineLearning.Predictors;
using eShopSolution.Application.MachineLearning.Trainers;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public ProductService(EShopDbContext context, IStorageService storageService, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _storageService = storageService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                Name = request.Name,
                Description = request.Description,
                SeoAlias = request.SeoAlias,
                SeoDescription = request.SeoDescription,
                Details = request.Details,
               
            };
            if(request.ThumbnailImage.Count > 0)
            {
                product.ProductImages = new List<ProductImage>();
                int idx = 0;

                request.ThumbnailImage.ForEach(async (value) =>
                {
                    if (value != null)
                    {
                        product.ProductImages.Add(new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = value.Length,
                            ImagePath = await this.SaveFile(value),
                            IsDefault = idx == 0 ?  true : false ,
                        });
                        idx++;


                    }
                });
            }
            //Save image 
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductVm>> GetAllPaging(GetManageProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
            join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
            from pic in ppic.DefaultIfEmpty()
            join c in _context.Categories on pic.CategoryId equals c.Id into picc
            from c in picc.DefaultIfEmpty()
            join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
            from pi in ppi.DefaultIfEmpty()
            where pi.IsDefault == true
            select new { p, pic, pi };
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.Name.Contains(request.Keyword));

            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = new List<string>()
                    {
                        x.pi.ImagePath
                    }
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ProductVm> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
           
            var categories = await (from c in _context.Categories
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId
                                    select c.Name).ToListAsync();

            var image = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
            var productViewModel = new ProductVm()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories,
                ThumbnailImage = image.Count > 0 ? image.Select(x => x.ImagePath).ToList() : new List<string>() { "no-image.png" }
            };
            return productViewModel;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new EShopException($"Cannot find an image with id {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id {imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productImages =  await _context.ProductImages.Where(x=>x.ProductId == request.Id).ToListAsync();

            if (product == null) throw new EShopException($"Cannot find a product with id: {request.Id}");

            product.Name = request.Name;
            product.SeoAlias = request.SeoAlias;
            product.SeoDescription = request.SeoDescription;
            product.SeoTitle = request.SeoTitle;
            product.Description = request.Description;
            product.Details = request.Details;

            //Save image
            if (request.ThumbnailImage != null && request.ThumbnailImage.Count > 0)
            {
                product.ProductImages = new List<ProductImage>();
                int idx = 0;

                if (productImages.Count > 0)
                {
                    product.ProductImages.AddRange(productImages);
                    idx = 1;
                }

                request.ThumbnailImage.ForEach(async (value) =>
                {
                    if (value != null)
                    {
                        product.ProductImages.Add(new ProductImage()
                        {
                            Caption = "Thumbnail image",
                            DateCreated = DateTime.Now,
                            FileSize = value.Length,
                            ImagePath = await this.SaveFile(value),
                            IsDefault = idx == 0 ? true : false,
                        });
                        idx++;
                    }
                });
            }
            
            _context.Products.Update(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find an image with id {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<PagedResult<ProductVm>> GetAllByCategoryId( GetPublicProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p, pic };
            //2. filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>($"Sản phẩm với id {id} không tồn tại");
            }
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id)
                    && x.ProductId == id);
                if (productInCategory != null && category.Selected == false)
                {
                    _context.ProductInCategories.Remove(productInCategory);
                }
                else if (productInCategory == null && category.Selected)
                {
                    await _context.ProductInCategories.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<ProductVm>> GetFeaturedProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where  (pi == null || pi.IsDefault == true)
                        && p.IsFeatured == true
                        select new { p, pic, pi };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = new List<string>()
                    {
                         x.pi.ImagePath
                    }
                }).ToListAsync();

            return data;
        }

        public async Task<List<ProductVm>> GetLatestProducts( int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where  (pi == null || pi.IsDefault == true)
                        select new { p, pic, pi };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    ThumbnailImage = new List<string>()
                    {
                         x.pi.ImagePath
                    }
                }).ToListAsync();

            return data;
        }

        public async Task<List<ProductOfOrder>> GetProductsOldOrder()
        {
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            var userEntity = await _userManager.FindByNameAsync(user);
            var userId = userEntity.Id;
            var query = from p in _context.Products
                        join od in _context.OrderDetails on p.Id equals od.ProductId into odd
                        from od in odd.DefaultIfEmpty()
                        join o in _context.Orders on od.OrderId equals o.Id into oo
                        from o in oo.DefaultIfEmpty()
                        
                        where ( o.UserId == userId)
                        select new { p, o, od };
            var data = await query.OrderByDescending(x => x.p.DateCreated)
                .Select(x => new ProductOfOrder()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.p.SeoAlias,
                    SeoDescription = x.p.SeoDescription,
                    SeoTitle = x.p.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    StatusOrder = (int)x.o.Status,
                    Quantity = x.od.Quantity,

                }).ToListAsync();

            return data;
        }

        public async Task<ApiResult<bool>> CreateProductStart(ProductStartCreateRequest request)
        {
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (user == null)
            {
                return new ApiErrorResult<bool>("Bạn cần kiểm tra lại thông tin đăng nhập!");
            }
            var userEntity = await _userManager.FindByNameAsync(user);
            
            var userId = userEntity.Id;
            var productStartCur = await _context.ProductStarts.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == request.ProductId);
            if (productStartCur != null)
            {
                return new ApiErrorResult<bool>("Sản phẩm đã được bạn đánh giá");
            }
            var productStart = new ProductStart()
            {
                ProductId = request.ProductId,
                UserId = userId,
                Comment = request.Comment,
                Start = request.Start
            };
            var query1 = await _context.ProductStarts.ToListAsync();
            _context.ProductStarts.Add(productStart);
            string newFileName = "C:\\Users\\Tan\\Documents\\Projects2022\\Shop2022-code-3110\\Shop2022-code-3110\\eShopSolution.Application\\Data\\recommendation-ratings.csv";
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(string));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Label", typeof(float));
            query1.Add(productStart);
            query1.ForEach(
                value =>
                {
                    table.Rows.Add(value.UserId, value.ProductId, (float)value.Start);
                });
            ToCSV(table, newFileName);
            var ressult =  await _context.SaveChangesAsync();
            
            return new ApiSuccessResult<bool>();

            
        }

        public async Task<List<ProductStartVm>> getProductStartByProductId(int productId)
        {

            var query = from ps in _context.ProductStarts
                        join u in _context.Users on ps.UserId equals u.Id into ud
                        from u in ud.DefaultIfEmpty()
                        where (ps.ProductId == productId)
                        select new { ps, u };
            var data = await query.OrderByDescending(x => x.ps.Start)
                .Select(x => new ProductStartVm()
                {
                    Id = x.ps.Id,
                    UserName = x.u.UserName,
                    Comment = x.ps.Comment,
                    Start = x.ps.Start,
                    ProductId = x.ps.ProductId

                }).ToListAsync();

            return data;
        }

        public async Task<List<ProductVm>> GetRecommendationProducts(int take)
        {
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            var data = new List<ProductVm>();

            if (user == null)
            {
                return data;
            }
            var userEntity = await _userManager.FindByNameAsync(user);
            var userId = userEntity.Id;
            //string newFileName = "C:\\Users\\Tan\\Documents\\Projects2022\\Shop2022-code-3110\\Shop2022-code-3110\\eShopSolution.Application\\Data\\recommendation-ratings.csv";
            //DataTable table = new DataTable();
            //table.Columns.Add("UserId", typeof(string));
            //table.Columns.Add("ProductId", typeof(int));
            //table.Columns.Add("Label", typeof(float));
            //var query1 = await _context.ProductStarts.ToListAsync();
            //query1.ForEach(
            //    value =>
            //    {
            //        table.Rows.Add(value.UserId, value.ProductId, (float)value.Start);
            //    });
            //ToCSV(table, newFileName);

            var listRecommid = new List<ProductScore>();
            var query =  from p in _context.Products
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where (pi == null || pi.IsDefault == true)
                        select new { p, pi };

            var  listProduct = await query.Take(100).ToListAsync();
            listProduct.ForEach(value =>
            {
                var newSample = new ProductRating
                {
                    UserId = userEntity.Id.ToString(),
                    ProductId = value.p.Id,
                };
                var result = getFloatMax(newSample);
                listRecommid.Add(new ProductScore()
                {
                    ProductId= value.p.Id,
                    Score = result,
                });

            });
            listRecommid.Sort((x, y) => y.Score.CompareTo(x.Score));
            listRecommid.Take(take);
            if (listRecommid.Count > 0)
            {
                listRecommid.ForEach(
                    value =>
                    {
                        var product = listProduct.Find(x => x.p.Id == value.ProductId);
                        data.Add(
                            new ProductVm()
                            {
                                Id = product.p.Id,
                                Name = product.p.Name,
                                DateCreated = product.p.DateCreated,
                                Description = product.p.Description,
                                Details = product.p.Details,
                                OriginalPrice = product.p.OriginalPrice,
                                Price = product.p.Price,
                                SeoAlias = product.p.SeoAlias,
                                SeoDescription = product.p.SeoDescription,
                                SeoTitle = product.p.SeoTitle,
                                Stock = product.p.Stock,
                                ViewCount = product.p.ViewCount,
                                ThumbnailImage = new List<string>()
                                {
                                         product.pi.ImagePath
                                }
                            });
                    });
            }            
            return data;
        }
        public float getFloatMax(ProductRating prd) {
            var trainers = new List<ITrainerBase>
            {
                new MatrixFactorizationTrainer(10, 50, 0.01)

            };
            var arrFloar = new List<float>();
            trainers.ForEach(t => arrFloar.Add(TrainEvaluatePredict(t, prd)));
            return arrFloar.Max();
        }
        public void ToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public float TrainEvaluatePredict(ITrainerBase trainer, ProductRating newSample)
        {            
            trainer.Fit("C:\\Users\\Tan\\Documents\\Projects2022\\Shop2022-code-3110\\Shop2022-code-3110\\eShopSolution.Application\\Data\\recommendation-ratings.csv");
            //var modelMetrics = trainer.Evaluate();           
            trainer.Save();
            var predictor = new Predictor();
            var prediction = predictor.Predict(newSample);
            return prediction.Score;
        }
    }
}