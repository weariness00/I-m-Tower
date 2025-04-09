using Game;
using UnityEngine;
using Util;

namespace Unit.Monster
{
    public class MonsterSpawner : ObjectPoolSpawner<MonsterControl>
    {
        public override void OnGetObject(MonsterControl monster)
        {
            monster.gameObject.SetActive(true);
            monster.navMeshAgent.SetDestination(GameManager.Instance.tower.transform.position);
        }

        public override void OnReleaseObject(MonsterControl monster)
        {
            monster.gameObject.SetActive(false);
        }

        public override void OnDestroyObject(MonsterControl monster)
        {
            Destroy(monster.gameObject);
        }
    }
}

