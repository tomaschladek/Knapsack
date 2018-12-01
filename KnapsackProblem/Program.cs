using System.Collections.Generic;
using KnapsackProblem.Experiments;
using KnapsackProblem.Experiments.Performance;

namespace KnapsackProblem
{
    class Program
    {
        
        static void Main()
        {
            var path = @"C:\Users\tomas.chladek\Documents\Personal\Uni\Master\3rd\PAA\Knapsack\";
            var experiments = new List<IExperiment>
            {
                //new MaxWeightErrorExperiment(null),
                //new MaxWeightErrorExperiment(17),
                //new MaxWeightErrorExperiment(18),
                //new MaxWeightErrorExperiment(19),
                //new MaxWeightErrorExperiment(20),
                //new MaxWeightErrorExperiment(21),
                //new MaxWeightErrorExperiment(22),
                //new MaxWeightErrorExperiment(23),
                //new MaxWeightErrorExperiment(24),
                //new MaxWeightErrorExperiment(25),
                //new MaxWeightErrorExperiment(26),
                //new MaxWeightErrorExperiment(27),
                //new MaxWeightErrorExperiment(28),
                //new MaxWeightErrorExperiment(29),
                //new MaxWeightErrorExperiment(30),
                //new MaxWeightErrorExperiment(31),
                //new MaxWeightErrorExperiment(32),
                //new MaxWeightRatioErrorExperiment(null),
                //new MaxWeightRatioErrorExperiment(17),
                //new MaxWeightRatioErrorExperiment(18),
                //new MaxWeightRatioErrorExperiment(19),
                //new MaxWeightRatioErrorExperiment(20),
                //new MaxWeightRatioErrorExperiment(21),
                //new MaxWeightRatioErrorExperiment(22),
                //new MaxWeightRatioErrorExperiment(23),
                //new MaxWeightRatioErrorExperiment(24),
                //new MaxWeightRatioErrorExperiment(25),
                //new MaxWeightRatioErrorExperiment(26),
                //new MaxWeightRatioErrorExperiment(27),
                //new MaxWeightRatioErrorExperiment(28),
                //new MaxWeightRatioErrorExperiment(29),
                //new MaxWeightRatioErrorExperiment(30),
                //new MaxWeightRatioErrorExperiment(31),
                //new MaxWeightRatioErrorExperiment(32),
                //new GeneticAlgorithmExperiment(),
                //new MaxPriceErrorExperiment(10),
                //new MaxPriceErrorExperiment(11),
                //new MaxPriceErrorExperiment(12),
                //new MaxPriceErrorExperiment(14),
                //new MaxPriceErrorExperiment(16),
                //new MaxPriceErrorExperiment(20),
                //new MaxPriceErrorExperiment(25),
                //new MaxPriceErrorExperiment(33),
                //new MaxPriceErrorExperiment(50),
                //new MaxPriceErrorExperiment(60),
                //new MaxPriceRatioErrorExperiment(null),
                //new MaxPriceRatioErrorExperiment(10),
                //new MaxPriceRatioErrorExperiment(11),
                //new MaxPriceRatioErrorExperiment(12),
                //new MaxPriceRatioErrorExperiment(14),
                //new MaxPriceRatioErrorExperiment(16),
                //new MaxPriceRatioErrorExperiment(20),
                //new MaxPriceRatioErrorExperiment(25),
                //new MaxPriceRatioErrorExperiment(33),
                //new MaxPriceRatioErrorExperiment(50),
                //new MaxPriceRatioErrorExperiment(60),
                //new PriceWeightRatioErrorExperiment(30,0),
                //new PriceWeightRatioErrorExperiment(30,-5),
                //new PriceWeightRatioErrorExperiment(30,5),
                //new PriceWeightRatioErrorExperiment(30,10),
                //new PriceWeightRatioErrorExperiment(30,-10),
                //new PriceWeightRatioErrorExperiment(35),
                //new PriceWeightRatioErrorExperiment(40),
                //new PriceWeightRatioErrorExperiment(45),
                //new PriceWeightRatioErrorExperiment(50),
                //new PriceWeightRatioErrorExperiment(55),
                //new PriceWeightRatioErrorExperiment(60),
                //new PriceWeightRatioErrorExperiment(65),
                //new PriceWeightRatioErrorExperiment(70)
                //new RandomErrorExperiment()
                //new PriceWeightRatioErrorExperiment(-100),
                //new PriceWeightRatioErrorExperiment(-75),
                //new PriceWeightRatioErrorExperiment(-50),
                //new PriceWeightRatioErrorExperiment(-25),
                //new PriceWeightRatioErrorExperiment(0),
                //new PriceWeightRatioErrorExperiment(25),
                //new PriceWeightRatioErrorExperiment(50),
                //new PriceWeightRatioErrorExperiment(75),
                //new PriceWeightRatioErrorExperiment(100),
                //new InstanceSizeErrorExperiment()
                new GeneticAlgorithmMutationExperiment()
        };
            //for (int weightIndex = 30; weightIndex <= 70; weightIndex += 5)
            //{
            //    for (int offsetIndex = -10; offsetIndex <= 10; offsetIndex += 5)
            //    {
            //        experiments.Add(new PriceWeightRatioErrorExperiment(weightIndex, offsetIndex));
            //    }
            //}
            foreach (var experiment in experiments)
            {
                experiment.Execute(path);
            }
        }
    }
}
