using System.Collections.Generic;
using Architecture.ServiceLocator;
using Core.Controllers;
using Core.Controllers.Impl;
using Core.Models.Enemy;
using UnityEngine;

namespace Architecture.Context
{
    public class CommonContext : ContextBase
    {
        [SerializeField] private List<EnemyData> enemyDatas;
        
        protected override bool IsCommonContext => true;

        protected override void InjectInstances(IServiceLocator serviceLocator)
        {
            serviceLocator.Add<ISceneController>(new SceneController());
            serviceLocator.Add<IList<EnemyData>>(enemyDatas);
        }
    }
}