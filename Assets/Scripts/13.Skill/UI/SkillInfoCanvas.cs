using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;

namespace Skill.UI
{
    public class SkillInfoCanvas : MonoBehaviour
    {
        public SkillManager targetSkillManager;

        [Header("스킬 Log 관련")]
        public Canvas addLogCanvas;
        public Transform logParent;
        public SkillAddLogUI skillAddLogUIPrefab;
        private ObjectPool<SkillAddLogUI> logUIPool;
        private Queue<SkillAddLogUI> activeLogUIQueue = new();
        private SkillAddLogUI prevLog;
        public float itemSpacing = 8f;
        public float minScale = 0.7f;

        [Header("모든 스킬 Info 관련")]
        public Canvas listCanvas;

        public void Awake()
        {
            logUIPool = new(
                () => Instantiate(skillAddLogUIPrefab, logParent),
                logUI =>
                {
                    logUI.gameObject.SetActive(true);
                    logUI.rectTransform.anchoredPosition = Vector2.zero;
                    logUI.rectTransform.localScale = Vector3.one * 1.2f;
                    logUI.rectTransform.anchoredPosition = new Vector2(0, -1 * (logUI.rectTransform.rect.height + itemSpacing));
                    logUI.canvasGroup.alpha = 1;
                },
                logUI =>
                {
                    if(activeLogUIQueue.Count != 0) activeLogUIQueue.Dequeue();
                    logUI.gameObject.SetActive(false);
                    if (prevLog == logUI)
                        prevLog = null;
                },
                logUI => Destroy(logUI.gameObject),
                true,
                20,
                20);

            var logUIList = new List<SkillAddLogUI>();
            for (int i = 0; i < 20; i++)
            {
                var logUI = logUIPool.Get();
                logUIList.Add(logUI);
            }
            foreach (var skillAddLogUI in logUIList)
                logUIPool.Release(skillAddLogUI);

            targetSkillManager.onSkillLevelUpEvent += CreateNewSkillAddNewLog;
        }

        // 추가된 스킬의 Log를 생성
        private void CreateNewSkillAddNewLog(SkillBase skill)
        {
            if (logUIPool.CountActive == 20)
                logUIPool.Release(activeLogUIQueue.Peek());
            if (prevLog != null)
            {
                prevLog.StopAllCoroutines();
                prevLog.StartCoroutine(prevLog.AlphaEnumerator(3f));
                prevLog.StartCoroutine(LogReleaseEnumerator(prevLog, 3f));
            }

            var logUI = logUIPool.Get();
            logUI.SetSkill(skill);
            prevLog = logUI;
            
            activeLogUIQueue.Enqueue(logUI);
            StopAllCoroutines();
            StartCoroutine(AnimateLogEntries());
        }
        
        private IEnumerator AnimateLogEntries()
        {
            float duration = 0.2f;
            float t = 0f;

            int count = activeLogUIQueue.Count;
            var logs = activeLogUIQueue.ToArray(); // 스택 → 배열 (최근 생성 순서 반영)
            Array.Reverse(logs);

            Vector2[] startPositions = new Vector2[count];
            Vector2[] endPositions = new Vector2[count];

            Vector3[] startScales = new Vector3[count];
            Vector3[] endScales = new Vector3[count];

            for (int i = 0; i < count; i++)
            {
                var log = logs[i];

                startPositions[i] = log.rectTransform.anchoredPosition;
                endPositions[i] = new Vector2(0, Mathf.Clamp(i-1,0, count) * (log.rectTransform.rect.height * 0.7f + itemSpacing) + (i > 0 ? log.rectTransform.rect.height : 0f));
                
                startScales[i] = log.rectTransform.localScale;
                endScales[i] = (i == 0) ? Vector3.one : Vector3.one * 0.7f;
            }

            while (t < 1f)
            {
                t += Time.deltaTime / duration;

                for (int i = 0; i < count; i++)
                {
                    var log = logs[i];
                    log.rectTransform.anchoredPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
                    log.rectTransform.localScale = Vector3.Lerp(startScales[i], endScales[i], t);
                }

                yield return null;
            }

            // 보정
            for (int i = 0; i < count; i++)
            {
                logs[i].rectTransform.anchoredPosition = endPositions[i];
                logs[i].rectTransform.localScale = endScales[i];
            }
        }

        private IEnumerator LogReleaseEnumerator(SkillAddLogUI log, float duration)
        {
            yield return new WaitForSeconds(duration);
            logUIPool.Release(log);
        }
    }
}