using Unity;

namespace Crosscutting.DependencyInjectionFactory
{
    public static class Factory
    {
        private static readonly DiContainer Container = new DiContainer();

        public static TService Resolve<TService>()
        {
            return Container.Current.Resolve<TService>();
        }
    }
}
