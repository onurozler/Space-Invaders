using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Helpers;
using UnityEngine;

namespace Core.Models.Enemy
{
    public class EnemyFormationData
    {
        public event Action OnAllEnemiesDestroyed;
        
        private static readonly int[] RowMovement = {-1, 0, 0, 1};
        private static readonly int[] ColumnMovement = { 0, -1, 1, 0 };
        
        private readonly IList<Vector2Int> _shooterEnemies;
        private readonly ICollection<Vector2Int> _connectedEnemies;
        private EnemyData[,] _enemyDatas;

        public EnemyFormationData()
        {
            _shooterEnemies = new List<Vector2Int>();
            _connectedEnemies = new Collection<Vector2Int>();
        }

        public EnemyData[,] Create(int xDimension, int yDimension)
        {
            _enemyDatas = new EnemyData[xDimension,yDimension];
            
            for (int y = 0; y < yDimension; y++)
            {
                for (int x = 0; x < xDimension; x++)
                {
                    _enemyDatas[x, y] = new EnemyData
                    {
                        Type = (EnemyType) ((yDimension - y) / 3f).FloorToClosestInt(),
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
            if (_shooterEnemies.Count == 0)
            {
                OnAllEnemiesDestroyed?.Invoke();
            }
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