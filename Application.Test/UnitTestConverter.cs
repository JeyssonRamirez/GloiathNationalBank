using Application.Implementation;
using Core.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace Application.Test
{
    public class UnitTestConverter
    {
        [Fact]

        //Direct Conversion
        public void ConvertCopToEur()
        {
            var converter = new ConverterMoney(new List<Rates> {
                new Rates{ From ="AUD",Rate =decimal.Parse("1.37"),To ="EUR" },
                new Rates{ From ="CAD",Rate =decimal.Parse("0.75"),To ="AUD" },
                new Rates{ From ="COP",Rate =decimal.Parse("0.00028"),To ="EUR" }
            });

          var map =  converter.GetMapToConvertionLineal("COP", "EUR");
            
        }
        [Fact]
        //Complex Convertion 
        public void ConvertCadToEur()
        {
            var converter = new ConverterMoney(new List<Rates> {
                new Rates{ From ="AUD",Rate =decimal.Parse("1.37"),To ="EUR" },
                new Rates{ From ="CAD",Rate =decimal.Parse("0.75"),To ="AUD" },
                new Rates{ From ="COP",Rate =decimal.Parse("0.00028"),To ="EUR" }
            });

          var map =  converter.GetMapToConvertionLineal("CAD", "EUR");

        }
    }
}
