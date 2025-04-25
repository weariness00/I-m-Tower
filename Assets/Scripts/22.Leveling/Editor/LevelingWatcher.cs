using System.IO;
using Leveling;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class LevelingWatcher
{
    private static FileSystemWatcher watcher;

    static LevelingWatcher()
    {
        if (LevelingDataSO.Instance == null)
        {
            Debug.LogWarning("LevelingDataSO 인스턴스가 null입니다.");
            return;
        }
        
        if (LevelingDataSO.Instance.levelingDataJson == null)
        {
            Debug.LogWarning("levelingDataJson 에셋을 찾을 수 없습니다.");
            return;
        }
        string assetPath = AssetDatabase.GetAssetPath(LevelingDataSO.Instance.levelingDataJson); // 예: "Assets/Data/MyData.asset"
        string fullPath = Path.GetFullPath(assetPath);         // C:/.../Project/Assets/Data/MyData.asset
        string directory = Path.GetDirectoryName(fullPath);    // 디렉토리 경로
        string fileName = Path.GetFileName(fullPath);          // 감시할 파일 이름

        watcher = new FileSystemWatcher(directory, fileName)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
            EnableRaisingEvents = true,
            IncludeSubdirectories = false
        };

        watcher.Changed += OnFileChanged;
        watcher.Renamed += OnFileChanged;
    }

    private static void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        // 유니티 메인 쓰레드에서 실행하려면 다음처럼 사용
        EditorApplication.delayCall += () =>
        {
            Debug.Log($"파일 변경 감지됨: {e.FullPath}");

            if (LevelingDataSO.Instance != null)
            {
                LevelingDataSO.Instance.JsonLoad();
            }
        };
    }
}