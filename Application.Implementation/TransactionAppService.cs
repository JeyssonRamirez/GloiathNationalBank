using Application.Definition;
using Core.DataTransferObject;
using Core.Entities;
using Core.GlobalRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Implementation
{
    public class TransactionAppService : ITransactionAppService
    {

        private readonly ICurrencySource _currencySource;

        public TransactionAppService(
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
                var transactions = _currencySource.GetTransactions();
                if (transactions != null)
                {
                    var localtransaction = transactions.Select(s => new Transaction
                    {
                        Amount = s.Amount,
                        Currency = s.Currency,
                        Sku = s.Sku,
                        RegistrationDate = DateTime.Now,
                        Status = StatusType.Active,
                    });

                    response.Data = localtransaction;
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
