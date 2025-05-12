using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager;
using Unit.Monster;
using UnityEngine;
using Util;

namespace Skill
{
    public partial class DustGaleObject : MonoBehaviour, IPoolOnOff
    {
        private new SphereCollider collider;
        [HideInInspector] public SkillDustGale skill;

        private HashSet<MonsterControl> insideMonster = new();

        public void Awake()
        {
            collider = GetComponent<SphereCollider>();
            InitDustEffect();
            
            DebugManager.ToDo("스턴 넣기");
        }

        public void OnDestroy()
        {
            foreach (var monster in insideMonster)
            {
                monster.status.speed.RemoveModifier(skill.status.targetSpeedModifier);
                monster.status.damaged.RemoveModifier(skill.status.targetDamagedModifier);
            }
            insideMonster.Clear();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MonsterControl monster) && !insideMonster.Contains(monster))
            {
                insideMonster.Add(monster);
                monster.status.speed.AddModifier(skill.status.targetSpeedModifier);
                monster.status.damaged.AddModifier(skill.status.targetDamagedModifier);
                CancelDustGaleEffectTask(monster, monster.status.dieCancelToken.Token).Forget();
                
                // if (skill.status.stunProbability.IsProbability())
                //     monster.status.Stun(skill.status.stunDuration);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MonsterControl monster))
            {
                insideMonster.Remove(monster);
                monster.status.speed.RemoveModifier(skill.status.targetSpeedModifier);
                monster.status.damaged.RemoveModifier(skill.status.targetDamagedModifier);
                // if (skill.status.stunProbability.IsProbability())
                //     monster.status.Stun(skill.status.stunDuration);
            }
        }

        public void PoolOn()
        {
            collider.radius = skill.status.dustRadius;
            dustEffect.Play();
            for (var i = 0; i < dustEffectArray.Length; i++)
            {
                var main = dustEffectArray[i].main;
                var shape = dustEffectArray[i].shape;
                main.startSize = new (dustEffectOriginDataArray[i].StartSize1 * skill.status.dustRadius, dustEffectOriginDataArray[i].StartSize2 * skill.status.dustRadius);
                shape.radius = skill.status.dustRadius * dustEffectOriginDataArray[i].Radius;
                shape.radius = skill.status.dustRadius * dustEffectOriginDataArray[i].RadiusThickness;
            }
        }

        public void PoolOff()
        {
            dustEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // 모래 폭풍의 지속시간이 끝났을 경우 실행
        private async UniTask CancelDustGaleEffectTask(MonsterControl monster, CancellationToken token)
        {
            while (!token.IsCancellationRequested && gameObject.activeSelf)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token); // 다음 프레임까지 대기
            }
            if (insideMonster.Contains(monster))
            {
                insideMonster.Remove(monster);
                monster.status.speed.RemoveModifier(skill.status.targetSpeedModifier);
                monster.status.damaged.RemoveModifier(skill.status.targetDamagedModifier);
            }
        }
    }
    
    // 파티클 관리용
    public partial class DustGaleObject
    {
        public ParticleSystem dustEffect;
        private ParticleSystem[] dustEffectArray; // 하위 파티클들의 수정이 필요해서 사용
        private DustEffectOriginData[] dustEffectOriginDataArray;

        // 이펙트의 Variable 수정을 위한 원본값 저장
        private struct DustEffectOriginData
        {
            public float StartSize1;
            public float StartSize2;
            public float Radius;
            public float RadiusThickness;
        }

        private void InitDustEffect()
        {
            dustEffectArray = dustEffect.GetComponentsInChildren<ParticleSystem>();
            dustEffectOriginDataArray = new DustEffectOriginData[dustEffectArray.Length];
            for (var i = 0; i < dustEffectArray.Length; i++)
            {
                var particle = dustEffectArray[i];
                dustEffectOriginDataArray[i].StartSize1 = particle.main.startSize.constantMin;
                dustEffectOriginDataArray[i].StartSize2 = particle.main.startSize.constantMax;
                dustEffectOriginDataArray[i].Radius = particle.shape.radius;
                dustEffectOriginDataArray[i].RadiusThickness = particle.shape.radiusThickness;
            }
        }
    }
}