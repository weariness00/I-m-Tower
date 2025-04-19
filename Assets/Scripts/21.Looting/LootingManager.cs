using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Looting
{
    public class LootingManager : Singleton<LootingManager>
    {
        public Dictionary<int, ObjectPool<GameObject>> poolDictionary = new(); 
        
        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }

        public void OnLootingItem(int id, GameObject itemPrefab, Transform lootingTransform)
        {
            if (!poolDictionary.TryGetValue(id, out var pool))
            {
                pool = new ObjectPool<GameObject>(
                    () =>
                    {
                        var item = Instantiate(itemPrefab, lootingTransform.position, lootingTransform.rotation);
                        if(item.TryGetComponent(out ILootingItem lootingItem))
                            lootingItem.ID = itemPrefab.GetInstanceID();

                        return item;
                    },
                    item => item.gameObject.SetActive(true),
                    item => item.gameObject.SetActive(false),
                    item => Destroy(item.gameObject));

                poolDictionary.TryAdd(id, pool);
            }
            
            var item = pool.Get();
            item.transform.position = lootingTransform.position;
            item.transform.rotation = lootingTransform.rotation;
                
            if(item.TryGetComponent(out ILootingSuccess lootingSuccess))
                lootingSuccess.OnLootingSuccess();
        }

        public void Release(int id, GameObject obj)
        {
            if (poolDictionary.TryGetValue(id, out var pool))
            {
                pool.Release(obj);
            }
            else
            {
                Destroy(obj);
                DebugManager.LogWarning($"{id}의 Pool이 존재하지 않아 Destroy합니다.");
            }
        }
    }
}

