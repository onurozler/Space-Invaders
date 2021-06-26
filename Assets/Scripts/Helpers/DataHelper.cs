using System.Collections.Generic;
using System.Linq;
using Core.Models.Enemy;

namespace Helpers
{
    public static class DataHelper
    {
        public static IEnemyData GetByType(this IEnumerable<IEnemyData> enemyDatas, EnemyType type)
        {
            return enemyDatas?.FirstOrDefault(x => x.Type == type);
        }
    }
}