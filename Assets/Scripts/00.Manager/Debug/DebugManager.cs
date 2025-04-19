#define ENABLE_BUILD

using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Manager
{
    public static class DebugManager
    {
        #region Log
        [Conditional("ENABLE_BUILD")]
        public static void Log(object massage, Object context = null)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.log) return;
            if(context) Debug.Log(massage, context);
            else Debug.Log(massage);
        }
        
        [Conditional("ENABLE_BUILD")]
        public static void LogWarning(object massage, Object context = null)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.logWaring) return;
            if(context) Debug.LogWarning(massage, context);
            else Debug.LogWarning(massage);
        }

        [Conditional("ENABLE_BUILD")]
        public static void LogError(object massage, Object context = null)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.logError) return;
            if(context) Debug.LogError(massage, context);
            else Debug.LogError(massage);
        }

        #endregion

        #region TODO Log

        [Conditional("ENABLE_BUILD")]
        public static void ToDo(object massage)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.isToDo) return;
            Debug.Log("TO DO List\n" + massage);
        }

        [Conditional("ENABLE_BUILD")]
        public static void ToDoError(object massage)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.isToDoError) return;
            Debug.LogError("TO DO List\n" + massage);
        }
        #endregion

        #region Ray 
        [Conditional("ENABLE_BUILD")]
        public static void DrawRay(Vector3 position, Vector3 direction, Color color, float time)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.drawRay) return;
            Debug.DrawRay(position, direction, color, time);
        }

        [Conditional("ENABLE_BUILD")]
        public static void DrawBoxRay(
            Vector3 center,
            Vector3 halfExtents,
            Vector3 direction,
            Quaternion orientation,
            float distance,
            Color color,
            float duration = 1f)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.drawBoxRay) return;
            Vector3 scaledDirection = direction.normalized * distance;
            Vector3 right = orientation * Vector3.right * halfExtents.x;
            Vector3 up = orientation * Vector3.up * halfExtents.y;
            Vector3 forward = orientation * Vector3.forward * halfExtents.z + scaledDirection;

            Vector3[] corners = new Vector3[8];
            corners[0] = center - right - up - forward;
            corners[1] = center + right - up - forward;
            corners[2] = center - right + up - forward;
            corners[3] = center + right + up - forward;
            corners[4] = center - right - up + forward;
            corners[5] = center + right - up + forward;
            corners[6] = center - right + up + forward;
            corners[7] = center + right + up + forward;

            for (int i = 0; i < 4; i++)
            {
                Debug.DrawLine(corners[i], corners[(i + 1) % 4], color, duration); // Draw base rectangle
                Debug.DrawLine(corners[i + 4], corners[(i + 1) % 4 + 4], color, duration); // Draw top rectangle
                Debug.DrawLine(corners[i], corners[i + 4], color, duration); // Connect bases
            }
        }

        [Conditional("ENABLE_BUILD")]
        public static void DrawSphereRay(Vector3 position, Vector3 direction, float radius, Color color,  float time = 1f)
        {
            if (!DebugScriptableObject.Instance.isDebug || !DebugScriptableObject.Instance.drawRay) return;
            var plusPosition = position * radius;
            var minusPosition = -position * radius;
            Debug.DrawLine(new Vector3(plusPosition.x, position.y, position.z), new Vector3(minusPosition.x, position.y, position.z));
            Debug.DrawLine(new Vector3(position.x, plusPosition.y, position.z), new Vector3(position.x, minusPosition.y, position.z));
            Debug.DrawLine(new Vector3(position.x, position.y, plusPosition.z), new Vector3(position.x, position.y, minusPosition.z));
        }

        #endregion
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class DebugLogExtension
    {
        static DebugLogExtension()
        {
            Application.logMessageReceived += HandleLog;
        }

        private static void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Log)
            {
                if (TryGetInstanceIDFromLog(logString, out int instanceID))
                {
                    EditorApplication.delayCall += () =>
                    {
                        Object contextObject = EditorUtility.InstanceIDToObject(instanceID);
                        if (contextObject != null)
                        {
                            Selection.activeObject = contextObject;
                            EditorGUIUtility.PingObject(contextObject);
                        }
                    };
                }
            }
        }

        /// <summary>
        /// 특정 Object 정보가 있으면 Log를 더블 클릭하면 해당 오브젝트가 선택되도록 설정
        /// </summary>
        /// <param name="logString"></param>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        private static bool TryGetInstanceIDFromLog(string logString, out int instanceID)
        {
            instanceID = -1;
            const string contextPrefix = "[Object : ";
            int contextIndex = logString.LastIndexOf(contextPrefix);
            if (contextIndex != -1)
            {
                int endIndex = logString.IndexOf(']', contextIndex);
                if (endIndex != -1)
                {
                    string idString = logString.Substring(contextIndex + contextPrefix.Length, endIndex - contextIndex - contextPrefix.Length);
                    return int.TryParse(idString, out instanceID);
                }
            }
            return false;
        }
    }
#endif
}
