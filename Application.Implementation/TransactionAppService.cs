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

        public TransactionAppService(ICurrencySource currencySource
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

        public BaseApiResult GetTransactionsBySkuInOtherCurrency(string sku, string currency)
        {
            
            var response = new BaseApiResult();
            try
            {
                TotalTransactionsConverted summary = new TotalTransactionsConverted();
                var transactions = _currencySource.GetTransactions();
                var localrates = new List<Rates>();
                var rates = _currencySource.GetRates();
                if (rates != null)
                {
                    localrates = rates.Select(s => new Rates
                    {
                        RegistrationDate = DateTime.Now,
                        Status = StatusType.Active,
                        From = s.From,
                        Rate = s.Rate,
                        To = s.To
                    }).ToList();
                }

                var converter = new ConverterMoney(localrates);

                var transactionConverted = new List<TransactionConverted>();

                if (transactions != null)
                {
                    var localtransaction = transactions
                        .Where(s => s.Sku == sku)
                        .Select(s => new Transaction
                        {
                            Amount = s.Amount,
                            Currency = s.Currency,
                            Sku = s.Sku,
                            RegistrationDate = DateTime.Now,
                            Status = StatusType.Active,
                        })
                        .ToList();

                    if (localtransaction == null)
                    {

                        response.Message = "no hay transacciones con ese Codigo (sku)";
                        return response;
                    }

                    foreach (var item in localtransaction)
                    {

                        var convertedTransaction = new TransactionConverted
                        {
                            OriginalAmount = item.Amount,
                            Id = item.Id,
                            OriginalCurrency = item.Currency,
                            RegistrationDate = item.RegistrationDate,
                            Sku = item.Sku,
                            Status = item.Status,
                        };


                        if (item.Currency != currency)
                        {
                            //convert
                            var map = converter.GetMapToConvertionLineal(item.Currency, currency);
                            convertedTransaction.ConvertedAmount = converter.ConvertTo(item.Amount, map);
                            convertedTransaction.ConvertedCurrency = currency;
                            convertedTransaction.ConvertionRoute = map;
                        }
                        else
                        {
                            convertedTransaction.ConvertedAmount = item.Amount;
                            convertedTransaction.ConvertedCurrency = item.Currency;

                        }
                        transactionConverted.Add(convertedTransaction);
                    }

                    summary.Transactions = transactionConverted;
                    summary.TotalConverted = transactionConverted.Sum(s => s.ConvertedAmount);
                    summary.CurrencyConverted = currency;

                    response.Data = summary;
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
