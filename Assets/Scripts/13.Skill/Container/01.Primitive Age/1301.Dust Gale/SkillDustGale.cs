using System;
using Cysharp.Threading.Tasks;
using Unit.Monster;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    [RequireComponent(typeof(SkillDustGaleStatus))]
    public class SkillDustGale : SkillBase
    {
        [SerializeField] private SkillDustGaleStatus status;
        public DustGaleObject dustGalePrefab;

        public ObjectPool<DustGaleObject> dustGalePool;
        public override SkillStatus Status => status;
        public override void Awake()
        {
            base.Awake();

            dustGalePool = new(
                () =>
                {
                    var dustGale = Instantiate(dustGalePrefab);
                    dustGale.SetSkill(this);
                    
                    return dustGale;
                },
                dustGale =>
                {
                    dustGale.gameObject.SetActive(true);
                    if(dustGale is IPoolOnOff poolOnOff)
                        poolOnOff.PoolOn();
                },
                dustGale =>
                {
                    dustGale.gameObject.SetActive(false);
                    if(dustGale is IPoolOnOff poolOnOff)
                        poolOnOff.PoolOff();
                },
                dustGale => Destroy(dustGale.gameObject));
        }

        public void Update()
        {
            status.attackTimer.Current += Time.deltaTime;
            if (status.attackTimer.IsMax)
            {
                var monster = FindAnyObjectByType<MonsterControl>();
                if (monster != null)
                {
                    status.attackTimer.SetMin();
                    var dustGale = dustGalePool.Get();
                    dustGale.transform.position = monster.transform.position;
                    
                    WaitTask(dustGale, status.dustDuration).Forget();
                }
            }
        }

        // 임시
        private async UniTaskVoid WaitTask(DustGaleObject dustGale, float duration)
        {
            await UniTask.WaitForSeconds(duration);
            dustGalePool.Release(dustGale);
        }
    }
}