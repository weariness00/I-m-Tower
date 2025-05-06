using System;
using Game;
using PlasticPipe;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Unit.Monster
{
    public class MonsterSpawner : ObjectPoolSpawner<MonsterControl>
    {
        public Action<MonsterControl> onReleaseSuccess;

        private Transform targetTransform;

        public override void Awake()
        {
            base.Awake();

            var scene = SceneManager.GetActiveScene();
            foreach (var rootGameObject in scene.GetRootGameObjects())
            {
                if (rootGameObject.layer == LayerMask.NameToLayer("Tower"))
                {
                    targetTransform = rootGameObject.transform;
                    break;
                }
            }
        }

        public override MonsterControl OnCreateObject()
        {
            var monster = base.OnCreateObject();
            monster.status.onDieEvent.AddListener(() =>
            {
                Release(monster);
                GameManager.Instance.playState.AddKill();
            });

            monster.status.onDamagedEvent += atk => GameManager.Instance.playState.AddDamage(atk);
            return monster;
        }

        public override void OnGetObject(MonsterControl monster)
        {
            monster.InitStatus();
            monster.collider.enabled = true;
            monster.gameObject.SetActive(true);
            monster.aStarAgent.Find(monster.transform.position, targetTransform.position);
        }

        public override void OnReleaseObject(MonsterControl monster)
        {
            monster.collider.enabled = false;
            monster.gameObject.SetActive(false);
            
            onReleaseSuccess?.Invoke(monster);
        }

        public override void OnDestroyObject(MonsterControl monster)
        {
            Destroy(monster.gameObject);
        }
    }
}

