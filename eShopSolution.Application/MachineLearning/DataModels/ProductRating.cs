using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.Application.MachineLearning.DataModels
{
    public class ProductRating
    {
        [LoadColumn(0)]
        public string UserId;

        [LoadColumn(1)]
        public int ProductId;

        [LoadColumn(2)]
        public float Label;
    }
}
