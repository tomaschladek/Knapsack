using System.Collections.Generic;
using KnapsackProblem.Experiments;

namespace KnapsackProblem
{
    class Program
    {
        
        static void Main()
        {
            var path = @"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\PAA\Knapsack\";
            var experiments = new List<IExperiment>
            {
                
                new MaxPriceRatioExperiment()

        };
            foreach (var experiment in experiments)
            {
                experiment.Execute(path);
            }
        }
    }
}
