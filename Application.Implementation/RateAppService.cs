using Application.Definition;
using Core.DataTransferObject;
using Core.Entities;
using Core.GlobalRepository;
using System;
using System.Linq;

namespace Application.Implementation
{
    public class RateAppService : IRateAppService
    {
        private readonly ICurrencySource _currencySource;

        public RateAppService(
             ICurrencySource currencySource
            )
        {
            _currencySource = currencySource;
        }

        public BaseApiResult GetAll()
        {
            var response = new BaseApiResult();
            try
            {
                var rates = _currencySource.GetRates();
                if (rates != null)
                {
                    var localrates = rates.Select(s => new Rates
                    {
                        RegistrationDate = DateTime.Now,
                        Status = StatusType.Active,
                        From = s.From,
                        Rate = s.Rate,
                        To = s.To
                    });

                    response.Data = localrates;
                    response.Message = "correcto";
                    response.Success = true;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                response.Message = exception.InnerException?.Message ?? exception.Message;
            }

            return response;
        }
    }
}
