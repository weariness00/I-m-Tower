using System;
using Cysharp.Threading.Tasks;
using Unit.Monster;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    [RequireComponent(typeof(SkillDustGaleStatus))]
    public class SkillDustGale : SkillBase
    {
        [NonSerialized] public new SkillDustGaleStatus status;
        public DustGaleObject dustGalePrefab;

        public ObjectPool<DustGaleObject> dustGalePool;

        public override void Awake()
        {
            base.Awake();

            dustGalePool = new(
                () =>
                {
                    var dustGale = Instantiate(dustGalePrefab);
                    dustGale.skill = this;
                    
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
            status.AttackTimer.Current += Time.deltaTime;
            if (status.AttackTimer.IsMax)
            {
                var monster = FindAnyObjectByType<MonsterControl>();
                if (monster != null)
                {
                    status.AttackTimer.SetMin();
                    var dustGale = dustGalePool.Get();
                    dustGale.transform.position = monster.transform.position;
                    
                    WaitTask(dustGale, status.dustDuration).Forget();
                }
            }
        }

        public override void Init()
        {
            base.Init();
            status = base.status as SkillDustGaleStatus;
        }
        
        // 임시
        private async UniTaskVoid WaitTask(DustGaleObject dustGale, float duration)
        {
            await UniTask.WaitForSeconds(duration);
            dustGalePool.Release(dustGale);
        }
    }
}