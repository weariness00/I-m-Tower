using System;
using Game;
using Util;

namespace Unit.Monster
{
    public class MonsterSpawner : ObjectPoolSpawner<MonsterControl>
    {
        public Action<MonsterControl> onReleaseSuccess;
        
        public override MonsterControl OnCreateObject()
        {
            var monster = base.OnCreateObject();
            monster.status.onDieEvent.AddListener(() =>
            {
                Release(monster);
                GamePlayStateManager.Instance.AddKill();
            });

            monster.status.onDamagedEvent += atk => GamePlayStateManager.Instance.AddDamage(atk);
            return monster;
        }

        public override void OnGetObject(MonsterControl monster)
        {
            monster.InitStatus();
            monster.collider.enabled = true;
            monster.gameObject.SetActive(true);
            monster.aStarAgent.Find(monster.transform.position, GameManager.Instance.tower.transform.position);
            // monster.navMeshAgent.SetDestination(GameManager.Instance.tower.transform.position);
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

