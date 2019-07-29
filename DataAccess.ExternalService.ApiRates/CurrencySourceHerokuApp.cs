using Core.DataTransferObject;
using Core.GlobalRepository;
using Crosscutting.Util;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace DataAccess.ExternalService.ApiRates
{
    public class CurrencySourceHerokuApp : ICurrencySource
    {

        private static string UriApi => ConfigurationManager.AppSettings["UriHerokuAppService"];

        public IEnumerable<RateResult> GetRates()
        {
            var response = new List<RateResult>();
            try
            {
                //log
                var cookie = new CookieCollection();
                var manager = new ServiceManager(UriApi + "/" + ConfigurationManager.AppSettings["UriHerokuAppService-Rates"]);
                var serviceResponse = manager.CallServiceAsync(
                    new List<DSParameter>(),
                    ref cookie,
                    "",
                    "",
                    RequestType.GET,
                    RequestBodyType.None
                    );

                var contentResult = serviceResponse.Content.ReadAsStringAsync().Result;
                //log

                if (serviceResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Ha ocurrido un error al realizar el consumo del servicio..");
                }


                response = JsonConvert.DeserializeObject<List<RateResult>>(contentResult, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception exception)
            {
                var clientLog = new TelemetryClient();
                clientLog.TrackException(new Exception($"Exception :{exception}, Response:{response}"));
            }

            return response;
        }

        public IEnumerable<TransactionResult> GetTransactions()
        {
            var response = new List<TransactionResult>();
            try
            {
                //log
                var cookie = new CookieCollection();
                var manager = new ServiceManager(UriApi + "/" + ConfigurationManager.AppSettings["UriHerokuAppService-Transaction"]);
                var serviceResponse = manager.CallServiceAsync(
                    new List<DSParameter>(),
                    ref cookie,
                    "",
                    "",
                    RequestType.GET,
                    RequestBodyType.None
                    );

                var contentResult = serviceResponse.Content.ReadAsStringAsync().Result;
                //log

                if (serviceResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Ha ocurrido un error al realizar el consumo del servicio...");
                }


                response = JsonConvert.DeserializeObject<List<TransactionResult>>(contentResult, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception exception)
            {
                var clientLog = new TelemetryClient();
                clientLog.TrackException(new Exception($"Exception :{exception}, Response:{response}"));
            }

            return response;
        }
    }
}
