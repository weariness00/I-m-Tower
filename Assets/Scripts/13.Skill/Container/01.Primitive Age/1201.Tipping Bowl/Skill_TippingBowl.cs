using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unit.Monster;
using UnityEngine;

namespace Skill
{
    //높은 곳에 놓인 물그릇을 기울여 전방으로 물을 쏟아냅니다.
    public class Skill_TippingBowl : SkillBase
    {
        // 타워에서 물이 웅덩이에서 한번 떨어지는 형식
        // 한번 쏟아지는 시간은 2초
        // tick(0.5초)당 데미지 
        [NonSerialized] public new Skill_TippingBowlStatus status;
        
        private HashSet<MonsterControl> targetMonsterHashSet = new();
        
        public void Update()
        {
            // 스킬 무한 지속
            if (status.attackTimer.Max == 0)
            {
                status.tickTimer.Current += Time.deltaTime;
                if (status.tickTimer.IsMax)
                {
                    status.tickTimer.SetMin();
                    ApplyDamage();
                }
            }
            // 쿨타임 있음
            else
            {
                if (status.isActive)
                {
                    status.tickTimer.Current += Time.deltaTime;
                    if (status.tickTimer.IsMax)
                    {
                        status.tickTimer.SetMin();
                        ApplyDamage();
                    }
                    
                    status.duration.Current += Time.deltaTime;
                    if (status.duration.IsMax)
                    {
                        status.tippingBowlObject.SetActive(false);
                        status.duration.SetMin();
                        status.isActive = false;
                    }
                }
                else
                {
                    status.attackTimer.Current += Time.deltaTime;
                    if (status.attackTimer.IsMax)
                    {
                        status.tippingBowlObject.SetActive(true);
                        status.attackTimer.SetMin();
                        status.isActive = true;
                        status.tickTimer.SetMax(); // 동작하자마자 바로 때리게 하기
                    }
                }
            }
        }

        private void AddInsideMonster(MonsterControl monster)
        {
            targetMonsterHashSet.Add(monster);
            monster.status.speed.AddModifier(status.monsterSlowPercent);
            monster.status.onDieEvent.AddListener(() => monster.status.speed.RemoveModifier(status.monsterSlowPercent));
        }

        private void RemoveInsideMonster(MonsterControl monster)
        {
            targetMonsterHashSet.Remove(monster);
            monster.status.speed.RemoveModifier(status.monsterSlowPercent);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MonsterControl monster))
            {
                AddInsideMonster(monster);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MonsterControl monster))
            {
                RemoveInsideMonster(monster);
            }
        }

        public override void Init()
        {
            base.Init();
            status = base.status as Skill_TippingBowlStatus;
        }

        private void ApplyDamage()
        {
            foreach (var monster in targetMonsterHashSet)
            {
                monster.status.Damaged(status.Damage);
                if (status.isDoubleHit)
                {
                    monster.status.Damaged(status.Damage);
                }
            }
        }
    }
}