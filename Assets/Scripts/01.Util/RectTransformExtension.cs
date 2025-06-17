using UnityEngine;

namespace Util
{
    public static class RectTransformExtension
    {
        
        public static void SetTopHeightPositionToSource(this RectTransform targetRectTransform, GameObject sourceObject)
        {
            if (sourceObject.TryGetComponent<RectTransform>(out RectTransform sourceRectTransform))
            {
                SetTopHeightPositionToSource(targetRectTransform, sourceRectTransform);
            }
            else
            {
                Debug.LogWarning($"[name : {sourceObject.name}] Source object does not have a RectTransform component.");
            }
        }
        /// <summary>
        /// Source RectTransform의 모든 오브젝트의 y좌표 중 가장 높은 갚을 찾는다.
        /// 가장 높은 y좌표 갚으로 target RectTransform의 y위치를 설정한다.
        /// </summary>
        /// <param name="targetRectTransform"></param>
        /// <param name="sourceRectTransform"></param>
        public static void SetTopHeightPositionToSource(this RectTransform targetRectTransform, RectTransform sourceRectTransform)
        {
            float highestY = float.MinValue;

            void FindTopYRecursive(RectTransform rect)
            {
                if (rect.gameObject.activeSelf == false) return;
                if (rect == targetRectTransform) return;
                if (!rect.TryGetComponent(out Canvas canvas))
                {
                    Vector3[] corners = new Vector3[4];
                    rect.GetWorldCorners(corners);

                    // Top 두 점의 Y 중 큰 값 (보통 같은 값이지만 안정성을 위해 Max)
                    float topY = Mathf.Max(corners[1].y, corners[2].y);
                    if (topY > highestY)
                        highestY = topY;
                }

                foreach (RectTransform child in rect)
                {
                    FindTopYRecursive(child);
                }
            }

            FindTopYRecursive(sourceRectTransform);

            // 대상 오브젝트의 현재 월드 중심 위치를 가져옴 (피벗 기준)
            Vector3 currentPos = targetRectTransform.position;

            // y만 수정, 다른 좌표는 그대로 유지
            currentPos.y = highestY;
            targetRectTransform.position = currentPos;
        }
        
        public static void SetBottomHeightPositionToSource(this RectTransform targetRectTransform, GameObject sourceObject)
        {
            if (sourceObject.TryGetComponent<RectTransform>(out RectTransform sourceRectTransform))
            {
                SetBottomHeightPositionToSource(targetRectTransform, sourceRectTransform);
            }
            else
            {
                Debug.LogWarning($"[name : {sourceObject.name}] Source object does not have a RectTransform component.");
            }
        }
        /// <summary>
        /// Source RectTransform의 모든 오브젝트의 y좌표 중 가장 낮은 갚을 찾는다.
        /// 가장 낮은 y좌표 갚으로 target RectTransform의 y위치를 설정한다.
        /// </summary>
        /// <param name="targetRectTransform"></param>
        /// <param name="sourceRectTransform"></param>
        public static void SetBottomHeightPositionToSource(this RectTransform targetRectTransform, RectTransform sourceRectTransform)
        {
            float lowestY = float.MaxValue;

            void FindTopYRecursive(RectTransform rect)
            {
                if (rect.gameObject.activeSelf == false) return;
                if (rect == targetRectTransform) return;
                if (!rect.TryGetComponent(out Canvas canvas))
                {
                    Vector3[] corners = new Vector3[4];
                    rect.GetWorldCorners(corners);

                    // Top 두 점의 Y 중 큰 값 (보통 같은 값이지만 안정성을 위해 Max)
                    float lowY = Mathf.Min(corners[0].y, corners[3].y);
                    if (lowY < lowestY)
                        lowestY = lowY;
                }

                foreach (RectTransform child in rect)
                {
                    FindTopYRecursive(child);
                }
            }

            FindTopYRecursive(sourceRectTransform);

            // 대상 오브젝트의 현재 월드 중심 위치를 가져옴 (피벗 기준)
            Vector3 currentPos = targetRectTransform.position;

            // y만 수정, 다른 좌표는 그대로 유지
            currentPos.y = lowestY;
            targetRectTransform.position = currentPos;
        }
    }
}