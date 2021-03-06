﻿using System;
using KnapsackSdk.Dtos;

namespace KnapsackSdk.Strategies
{
    public class FtpasStrategy : DecompositionByPriceStrategy
    {
        private double Precision { get; }

        public FtpasStrategy(double precision, string suffix = "")
        {
            Precision = precision;
            Id = $"FTPAS R={Precision}{suffix}";
        }

        protected override int GetSearchSpaceSize(DefinitionDto definition)
        {
            return (int) Math.Ceiling((base.GetSearchSpaceSize(definition)-1)/Precision)+1;
        }

        protected override long GetCompositionValue(ItemDto item)
        {
            return (long) Math.Ceiling(base.GetCompositionValue(item)/Precision);
        }

        public override string Id { get; }
    }
}