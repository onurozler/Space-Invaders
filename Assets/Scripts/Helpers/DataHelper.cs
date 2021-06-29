using System.Collections.Generic;
using System.Linq;
using Core.Models.Enemy;
using UnityEngine;

namespace Helpers
{
    public static class DataHelper
    {
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