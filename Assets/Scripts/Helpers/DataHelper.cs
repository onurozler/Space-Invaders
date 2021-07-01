using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Models.Enemy;
using Core.Models.Scene;
using UnityEngine;

namespace Helpers
{
    public static class DataHelper
    {
        public static string TypeToName(this SceneType sceneType)
        {
            return sceneType == SceneType.Game ? Constants.Scene.GameName : Constants.Scene.MenuName;
        }
        
        public static int FloorToClosestInt(this float number)
        {
            var decimalPart = number - (int) number;
            return decimalPart > 0 ? (int) number+1 :(int)number;
        }
        
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list.Count <= 0) return default;
            return list[Random.Range(0, list.Count)];
        }

        public static EnemyAssetData GetByType(this IEnumerable<EnemyAssetData> enemyDatas, EnemyType type)
        {
            return enemyDatas?.FirstOrDefault(x => x.Type == type);
        }
    }
}