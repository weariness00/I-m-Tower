using System;
using System.Collections;
using Manager;
using Unit.Monster;
using UnityEngine;
using UnityEngine.Pool;

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
                dustGale => dustGale.gameObject.SetActive(true),
                dustGale => dustGale.gameObject.SetActive(false),
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
                    var dustGale = dustGalePool.Get();
                    dustGale.transform.position = monster.transform.position;
                    
                    DebugManager.ToDo("n초뒤 먼지 바람 제거를 임시 코루틴으로 사용중\nR3를 깔면 해당으로 바꾸기");
                    StartCoroutine(Wait(dustGale, status.dustDuration));
                }
            }
        }

        public override void Init()
        {
            base.Init();
            status = base.status as SkillDustGaleStatus;
        }
        
        // 임시
        private IEnumerator Wait(DustGaleObject dustGale, float duration)
        {
            yield return new WaitForSeconds(duration);
            dustGalePool.Release(dustGale);
        }
    }
}