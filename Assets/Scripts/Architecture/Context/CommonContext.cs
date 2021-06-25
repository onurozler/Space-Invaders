using Architecture.ServiceLocator;
using Core.Controllers;

namespace Architecture.Context
{
    public class CommonContext : ContextBase
    {
        protected override bool IsCommonContext => true;

        protected override void InjectInstances(IServiceLocator serviceLocator)
        {
            serviceLocator.Add<ISceneController>(new SceneController());
        }
    }
}