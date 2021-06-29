using System.Collections.Generic;
using System.Collections.ObjectModel;
using Helpers;
using UnityEngine;

namespace Core.Models.Enemy
{
    public class EnemyFormationData
    {
        private static readonly int[] RowMovement = {-1, 0, 0, 1};
        private static readonly int[] ColumnMovement = { 0, -1, 1, 0 };
        
        private readonly EnemyData[,] _enemyDatas;
        private readonly IList<Vector2Int> _shooterEnemies;
        private readonly ICollection<Vector2Int> _connectedEnemies;
        
        public EnemyFormationData()
        {
            _enemyDatas = new EnemyData[Constants.Game.Grid.x,Constants.Game.Grid.y];
            _shooterEnemies = new List<Vector2Int>();
            _connectedEnemies = new Collection<Vector2Int>();
        }

        public EnemyData[,] Create()
        {
            var yDimensionLength = _enemyDatas.GetLength(1);
            var xDimensionLength = _enemyDatas.GetLength(0);

            for (int y = 0; y < yDimensionLength; y++)
            {
                for (int x = 0; x < xDimensionLength; x++)
                {
                    _enemyDatas[x, y] = new EnemyData
                    {
                        Type = (EnemyType) ((yDimensionLength - y) / 3f).FloorToClosestInt(),
                        Color = Constants.Enemy.Colors.GetRandom()
                    };
                    
                    if (y == 0)
                    {
                        _shooterEnemies.Add(new Vector2Int(x,y));
                    }
                }
            }

            return _enemyDatas;
        }

        public Vector2Int GetRandomShooter()
        {
            return _shooterEnemies.GetRandom();
        }

        public IEnumerable<Vector2Int> GetConnectedEnemies(Vector2Int target)
        {
            _connectedEnemies.Clear();
            GetConnectedEnemiesRecursively(target);
            return _connectedEnemies;
        }

        public void Remove(Vector2Int pos)
        {
            if (_shooterEnemies.Contains(pos))
            {
                var yDimensionLength = _enemyDatas.GetLength(1);
                for (int i = pos.y + 1; i < yDimensionLength; i++)
                {
                    var targetShooter = _enemyDatas[pos.x, i];
                    if (targetShooter != null)
                    {
                        _shooterEnemies.Add(new Vector2Int(pos.x,i));
                        break;
                    }
                }
            }
            _shooterEnemies.Remove(pos);
            _enemyDatas[pos.x, pos.y] = null;
        }

        #region Helpers
        
        private void GetConnectedEnemiesRecursively(Vector2Int pos)
        {
            if (_connectedEnemies.Contains(pos))
            {
                return;
            }
            _connectedEnemies.Add(pos);
            
            var currentEnemy = _enemyDatas[pos.x, pos.y];
            for (int i = 0; i < RowMovement.Length; i++)
            {
                var targetPosition = new Vector2Int(pos.x + RowMovement[i],pos.y + ColumnMovement[i]);
                if (IsAvailable(targetPosition,currentEnemy))
                {
                    GetConnectedEnemiesRecursively(targetPosition);
                }
            }
        }

        private bool IsAvailable(Vector2Int pos, EnemyData enemyData)
        {
            var xDimensionLength = _enemyDatas.GetLength(0);
            var yDimensionLength = _enemyDatas.GetLength(1);

            var isValidPosition = pos.x >= 0 && pos.x < xDimensionLength && pos.y >= 0 && pos.y < yDimensionLength;
            
            EnemyData currentEnemyData = null;
            if (isValidPosition)
            {
                currentEnemyData = _enemyDatas[pos.x, pos.y];
            }
            
            return currentEnemyData != null && currentEnemyData.Color == enemyData.Color && currentEnemyData.Type == enemyData.Type;
        }
        
        
        #endregion
    }
}