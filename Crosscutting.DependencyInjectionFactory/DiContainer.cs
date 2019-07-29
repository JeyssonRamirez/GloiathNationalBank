using Unity;

namespace Crosscutting.DependencyInjectionFactory
{
    public class DiContainer
    {
        public DiContainer()
        {
            Current = new UnityContainer();
            Current.InitializeContainer();
        }

        public IUnityContainer Current { get; set; }
    }
}
