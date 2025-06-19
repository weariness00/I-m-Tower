using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Util;

namespace Status
{
    public class StatusBase : MonoBehaviour
    {
        public GameObject ownerObject;

        [FormerlySerializedAs("Hp")] public MinMaxValue<int> hp = new();
        public Stat maxHp = new(100);
        public Stat speed = new(1);
        public Stat damage = new(0);
        public Stat criticalDamage = new(1.5f);
        public Stat criticalChance = new(0.1f);
        [InspectorReadOnly] public Stat damaged = new(0); // 받는 대미지
        [Tooltip("방어력")] public Stat defense = new(0);
        
        public Stat attackRange = new(10);
        [Tooltip("Value초당 1번 공격")] public Stat attackSpeed = new(1);
        public MinMaxValue<float> attackTimer = new(0,1);
        
        [HideInInspector] public UnityEvent onDieEvent = new ();
        public Action<int> onDamagedEvent;
        
        public CancellationTokenSource dieCancelToken = new CancellationTokenSource();
        private int stunRefCount = 0; // 현재 스턴중인 횟수
        public bool isStun;

        public int Damage => Mathf.FloorToInt(damage.Value * (criticalChance.Value.IsProbability() ? criticalDamage.Value : 1));

        public virtual void Awake()
        {
            if(ownerObject == null)
            {
                ownerObject = gameObject;
            }
            Init();
            attackTimer.onChangeValueCurrent += UpdateAttackTimer;
        }

        public virtual void Init()
        {
            hp.SetMax(Mathf.CeilToInt(maxHp.Value));
        }

        private void UpdateAttackTimer(MinMaxValue<float> value)
        {
            value.Max = attackSpeed.Value;
        }
        
        public void Damaged(float atk, int defencePenetrationValue = 0) =>Damaged(Mathf.FloorToInt(atk), defencePenetrationValue);
        public virtual void Damaged(int atk, int defencePenetrationValue = 0)
        {
            damaged.Value = atk;
            var realDamage = Mathf.FloorToInt(damaged.Value);// - (Defense - defencePenetrationValue);
            hp.Current -= realDamage;
            onDamagedEvent?.Invoke(realDamage);
            
            DebugManager.Log($"{ownerObject.name}이 {realDamage}만큼 피해를 입었습니다.");

            if (hp.IsMin)
            {
                onDieEvent.Invoke();
                dieCancelToken.Cancel();
                dieCancelToken.Dispose();
                dieCancelToken = new();
            }
        }

        // 스턴은 누적되지 않는다.
        public IEnumerator StunCoroutine(float duration)
        {
            DebugManager.Log($"{ownerObject.name}이 스턴에 걸렸습니다.");
            
            ++stunRefCount;
            isStun = true;
            yield return new WaitForSeconds(duration);
            --stunRefCount;
            if (stunRefCount <= 0)
            {
                isStun = false;
                DebugManager.Log($"{ownerObject.name}이 스턴에서 해제되었습니다.");
            }
        }

        public void GetDebuff(IDebuffController debuff, float tick, float duration) => GetDebuffTask(debuff, tick, duration, dieCancelToken.Token).Forget();
        private async UniTask GetDebuffTask(IDebuffController debuff, float tick, float duration, CancellationToken token)
        {
            float t = 0;
            debuff.Enter(this);
            while (t < duration)
            {
                if (token.IsCancellationRequested) break;
                debuff.Execute(this);
                await UniTask.Delay(TimeSpan.FromSeconds(tick), cancellationToken: token);
                t += tick;
            }
            debuff.End(this);
        }
    }
}

