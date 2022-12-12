using eShopSolution.Application.MachineLearning.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Trainers.Recommender;

namespace eShopSolution.Application.MachineLearning.Trainers
{
    public sealed class MatrixFactorizationTrainer : TrainerBase
    {
        public MatrixFactorizationTrainer(int numberOfIterations,
                      int approximationRank,
                      double learningRate) : base()
        {
            Name = $"Matrix Factorization {numberOfIterations}-{approximationRank}";

            _model = MlContext.Recommendation().Trainers.MatrixFactorization(
                                                      labelColumnName: "Label",
                                                      matrixColumnIndexColumnName: "UserIdEncoded",
                                                      matrixRowIndexColumnName: "ProductIdEncoded",
                                                      approximationRank: approximationRank,
                                                      learningRate: learningRate,
                                                      numberOfIterations: numberOfIterations);
        }
    }
}
