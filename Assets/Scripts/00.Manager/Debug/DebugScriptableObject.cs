using UnityEditor;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(fileName = "Debug Manager Data", menuName = "Manager/Debug", order = 0)]
    public class DebugScriptableObject : ScriptableObject
    {
        public static DebugScriptableObject Instance => DebugSettingProviderHelper.setting;
        
        public bool isDebug = true;
        
        public bool log = true;
        public bool logWaring = true;
        public bool logError = true;
        
        public bool isToDo = true;
        public bool isToDoError = true;

        public bool drawRay = true;
        public bool drawBoxRay = true;
    }
    
#if UNITY_EDITOR

    [CustomEditor(typeof(DebugScriptableObject))]
    public class DebugScriptableObjectEditor : UnityEditor.Editor
    {
        #region Property

        private SerializedProperty IsAllDebug;
        private SerializedProperty IsLog;
        private SerializedProperty IsLogWaring;
        private SerializedProperty IsLogError;
        
        private SerializedProperty IsToDo;
        private SerializedProperty IsToDoError;
        
        private SerializedProperty IsDrawRay;
        private SerializedProperty IsDrawBoxRay;
        
        #endregion

        private void OnEnable()
        {
            IsAllDebug = serializedObject.FindProperty("isDebug");
            IsLog = serializedObject.FindProperty("log");
            IsLogWaring = serializedObject.FindProperty("logWaring");
            IsLogError = serializedObject.FindProperty("logError");
            
            IsToDo = serializedObject.FindProperty("isToDo");
            IsToDoError = serializedObject.FindProperty("isToDoError");
            
            IsDrawRay = serializedObject.FindProperty("drawRay");
            IsDrawBoxRay = serializedObject.FindProperty("drawBoxRay");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(IsAllDebug);
            if (IsAllDebug.boolValue)
            {
                EditorGUILayout.PropertyField(IsLog);
                EditorGUILayout.PropertyField(IsLogWaring); 
                EditorGUILayout.PropertyField(IsLogError);
                EditorGUILayout.Space();
                
                EditorGUILayout.PropertyField(IsToDo);
                EditorGUILayout.PropertyField(IsToDoError);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(IsDrawRay);
                EditorGUILayout.PropertyField(IsDrawBoxRay);
            }

            serializedObject.ApplyModifiedProperties(); // 이게 없으면 Property Update가 안됨   
        }
    }
    #endif
}