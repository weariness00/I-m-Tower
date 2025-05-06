using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Status;
using ProjectTile;
using Unit.Monster;
using UnityEngine;

namespace Skill
{
    public class TelekineticSlamDebris : ProjectileBase
    {
        // 애니메이션이 끝난 후
        // TargetAroundMove를 TargetMove로 변경
        // Target이 없으면 자동으로 -Vector3.up 방향으로 가게끔 하기
        [NonSerialized] public new SkillTelekineticSlam ownerObject;
        [NonSerialized] public new SkillTelekineticSlamStatus ownerStatus;

        private TargetMove targetMove;
        private TargetAroundMove targetAroundMove;

        public override void Awake()
        {
            base.Awake();
            targetMove = new(this);
            targetAroundMove =new TargetAroundMove(this)
            {
                rotateAxis = Vector3.up,
                rotateSpeed = 0,
                radius = 5,
            };
            Move = targetAroundMove;
        }

        public override void Start()
        {
            base.Start();
            ownerObject = base.ownerObject.GetComponent<SkillTelekineticSlam>();
            ownerStatus = base.ownerStatus as SkillTelekineticSlamStatus;
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MonsterStatus otherStatus) &&
                !otherStatus.Hp.IsMin)
            {
                var isUnderHP = otherStatus.Hp.NormalizeToRange() <= ownerStatus.targetHpPercent;
                if (isUnderHP) ownerStatus.value.criticalChange += ownerStatus.slamCriticalChange;
                otherStatus.Damaged(ownerStatus.Damage, Mathf.FloorToInt(otherStatus.Defense * ownerStatus.targetDefensePenetrationMultiple));
                if (isUnderHP) ownerStatus.value.criticalChange -= ownerStatus.slamCriticalChange;
                if(!otherStatus.Hp.IsMin) SetTargetAsync(otherStatus, otherStatus.dieCancelToken.Token).Forget();
                pool.Release(this);
                
                var hitEffect = ownerObject.hitEffectPool.Get();
                var hitEffectMain = hitEffect.main;
                hitEffectMain.startSize = new(other.bounds.size.magnitude);
                hitEffect.transform.position = transform.position;
                hitEffect.Play();

                Move = targetAroundMove;
            }
        }

        public IEnumerator ChangedMoveCoroutine()
        {
            yield return new WaitForSeconds(1f);
            Move = targetMove;
            if(targetStatus.Hp.IsMin) targetMove.SetDirection(-Vector3.up);
        }
        
        private async UniTask SetTargetAsync(StatusBase _targetStatus, CancellationToken token)
        {
            _targetStatus.moreDamageMultiple += ownerStatus.targetMoreDamageMultiple;
            
            await UniTask.Delay(TimeSpan.FromSeconds(ownerStatus.targetMoreDamageMultipleDuration), cancellationToken: token);
            
            _targetStatus.moreDamageMultiple -= ownerStatus.targetMoreDamageMultiple;
        }
    }
}

