using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Managers.Pool
{
    public class MonoPoolManager : MonoBehaviour
    {
        [SerializeField] 
        private MonoBehaviour[] referencePoolItems;

        [SerializeField] 
        private int initialPoolCount;

        private IList<MonoBehaviour> _poolItems;

        public void Initialize(Action onInitialized = null)
        {
            _poolItems = new List<MonoBehaviour>();
            
            foreach (var referencePoolItem in referencePoolItems)
            {
                for (int i = 0; i < initialPoolCount; i++)
                {
                    var poolItem = Instantiate(referencePoolItem,transform);
                    poolItem.gameObject.SetActive(false);
                    _poolItems.Add(poolItem);
                }
            }
            
            onInitialized?.Invoke();
        }

        public T GetItem<T>() where T : MonoBehaviour
        {
            var inactiveItem = _poolItems.FirstOrDefault(x => !x.gameObject.activeSelf && x is T);
            if (inactiveItem != null)
            {
                inactiveItem.gameObject.SetActive(true);
                return inactiveItem as T;
            }
            else
            {
                var reference = referencePoolItems.FirstOrDefault(x => x is T);
                if (reference == null)
                {
                    throw new Exception("Reference can't be found!");
                }
                
                var item = Instantiate(reference,transform);
                _poolItems.Add(item);
                
                return item as T;
            }
        }

        public void ResetItems()
        {
            foreach (var poolItem in _poolItems)
            {
                poolItem.gameObject.SetActive(false);
            }
        }
    }
}