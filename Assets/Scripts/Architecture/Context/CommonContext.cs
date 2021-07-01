using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Controllers;
using Core.Models.Enemy;
using Core.Models.Scene;
using Core.Models.User;
using Helpers;
using Helpers.Timing;
using Network.WebNetworkService;
using Network.WebNetworkService.LocalAPI;
using UnityEngine;

namespace Architecture.Context
{
    public class CommonContext : ContextBase
    {
        private static CommonContext _instance;
        
        [SerializeField] private List<EnemyAssetData> enemyDatas;
        [SerializeField] private CoroutineTimingManager coroutineTimingManager;
        [SerializeField] private ResolutionHelper resolutionHelper;

        protected override bool IsCommonContext => true;

        protected override void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                return;
            }
            base.Awake();
        }

        protected override void InjectInstances(IServiceLocator serviceLocator)
        {
            serviceLocator.Add<ILocalRestAPI>(new LocalRestAPI());
            serviceLocator.Add<IWebService>(new WebService(serviceLocator,WebEnvironment.Local));
            serviceLocator.Add(new UserData());
            
            var sceneData = new SceneData();
            sceneData.OnSceneChanged += (sceneType)=> coroutineTimingManager.Clear();
            
            serviceLocator.Add(sceneData);
            serviceLocator.Add(resolutionHelper);
            serviceLocator.Add<ITimingManager>(coroutineTimingManager);
            serviceLocator.Add(new SceneController(serviceLocator));
            serviceLocator.Add<IList<EnemyAssetData>>(enemyDatas);
        }
    }
}