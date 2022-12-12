using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace eShopSolution.Application.MachineLearning.Common
{
    public interface ITrainerBase
    {
        string Name { get; }
        void Fit(string trainingFileName);
        RegressionMetrics Evaluate();
        void Save();
    }
}
