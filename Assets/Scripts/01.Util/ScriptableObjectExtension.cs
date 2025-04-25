using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Util
{
    public class ScriptableObjectExtension
    {
        public static void GenerateSOAssets<T>(int length, string folderPath, out T[] soList) where T : ScriptableObject
        {
            GenerateSOAssets(length, folderPath, out List<T> list);
            soList = list.ToArray();
        }
        public static void GenerateSOAssets<T>(int length, string folderPath, out List<T> soList) where T : ScriptableObject
        {
            soList = new();
            // 폴더가 없으면 생성
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string[] parts = folderPath.Split('/');
                string current = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    string next = current + "/" + parts[i];
                    if (!AssetDatabase.IsValidFolder(next))
                    {
                        AssetDatabase.CreateFolder(current, parts[i]);
                    }
                    current = next;
                }
            }

            string baseName = typeof(T).Name;
            var attr = typeof(T).GetCustomAttribute<CreateAssetMenuAttribute>();
            if (attr != null && !string.IsNullOrEmpty(attr.fileName))
            {
                baseName = attr.fileName;
            }
            
            for (int i = 0; i < length; i++)
            {
                T asset = ScriptableObject.CreateInstance<T>();
                soList.Add(asset);
                string path = $"{folderPath}/{baseName}.asset";
                string uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CreateAsset(asset, uniquePath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"{length}개의 {typeof(T).Name} 에셋 생성 완료!");
        }
        
        public static bool DeleteSOAsset(Object soInstance)
        {
            if (soInstance == null)
            {
                Debug.LogWarning("삭제하려는 SO 인스턴스가 null입니다.");
                return false;
            }

            string path = AssetDatabase.GetAssetPath(soInstance);

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning($"해당 객체는 프로젝트 에셋이 아닙니다: {soInstance.name}");
                return false;
            }

            bool success = AssetDatabase.DeleteAsset(path);

            if (success)
            {
                Debug.Log($"에셋 삭제 성공: {path}");
            }
            else
            {
                Debug.LogWarning($"에셋 삭제 실패: {path}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return success;
        }
        
        public static bool DeleteSOAsset<T>(string folderPath) where T : ScriptableObject
        {
            // 폴더 내의 모든 에셋 경로 가져오기
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });

            bool success = false;   
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                success = AssetDatabase.DeleteAsset(path);

                if (success)
                    Debug.Log($"삭제됨: {path}");
                else
                    Debug.LogWarning($"삭제 실패: {path}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            return success;
        }
        
        public static void DeleteSOAsset<T>(T[] soList) where T : ScriptableObject
        {
            foreach (var so in soList)
            {
                DeleteSOAsset(so);
            }
        }
    }
}
#endif
