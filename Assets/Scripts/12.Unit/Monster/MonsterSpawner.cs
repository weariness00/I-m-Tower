using Game;
using UnityEngine;
using Util;

namespace Unit.Monster
{
    public class MonsterSpawner : ObjectPoolSpawner<MonsterControl>
    {
        public override MonsterControl OnCreateObject()
        {
            var monster = base.OnCreateObject();
            monster.status.onDieEvent.AddListener(atk => Release(monster));
            return monster;
        }

        public override void OnGetObject(MonsterControl monster)
        {
            monster.InitStatus();
            monster.collider.enabled = true;
            monster.gameObject.SetActive(true);
            monster.navMeshAgent.SetDestination(GameManager.Instance.tower.transform.position);
        }

        public override void OnReleaseObject(MonsterControl monster)
        {
            monster.collider.enabled = false;
            monster.gameObject.SetActive(false);
        }

        public override void OnDestroyObject(MonsterControl monster)
        {
            Destroy(monster.gameObject);
        }
    }
}

