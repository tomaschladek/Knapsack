using System;
using KnapsackProblem.Dtos;

namespace KnapsackProblem.Strategies
{
    public class FtpasStrategy : DecompositionByPriceStrategy
    {
        private double _precision;

        public FtpasStrategy(double precision)
        {
            _precision = precision;
        }

        protected override int GetSearchSpaceSize(DefinitionDto definition)
        {
            return (int) Math.Ceiling((base.GetSearchSpaceSize(definition)-1)/_precision)+1;
        }

        protected override long GetCompositionValue(ItemDto item)
        {
            return (long) Math.Ceiling(base.GetCompositionValue(item)/_precision);
        }

        public override string Id => $"FTPAS R={_precision}";
    }
}