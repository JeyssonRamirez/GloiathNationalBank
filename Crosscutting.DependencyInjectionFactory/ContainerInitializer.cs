using Application.Definition;
using Application.Implementation;
using Core.GlobalRepository;
using DataAccess.ExternalService.ApiRates;
using System;
using Unity;

namespace Crosscutting.DependencyInjectionFactory
{
    public static class ContainerInitializer
    {
        public static void InitializeContainer(this IUnityContainer container)
        {


            //External Repositories 
            container.RegisterType<ICurrencySource, CurrencySourceHerokuApp>();
            //Mongo

            //Other

            //AppServices
            container.RegisterType<ITransactionAppService, TransactionAppService>();
            container.RegisterType<IRateAppService, RateAppService>();


        }
    }
}
