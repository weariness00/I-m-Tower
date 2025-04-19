using System.Collections;
using Game;
using Manager;
using UnityEngine;

namespace Looting.Item
{
    // 경험치
    public class ManaCore : MonoBehaviour, ILootingItem, ILootingSuccess
    {
        public uint manaAmount = 1;
        public int ID { get; set; }

        public void OnLootingSuccess()
        {
            StartCoroutine(Earn());
        }

        private IEnumerator Earn()
        {
            yield return new WaitForSeconds(1f);
            GameManager.Instance.tower.status.AddEXP(manaAmount);
            LootingManager.Instance.Release(ID, gameObject);
            DebugManager.Log($"경험치(마나) {manaAmount}를 획득");
        }

    }
}

