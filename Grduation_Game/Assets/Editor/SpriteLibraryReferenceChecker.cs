using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections.Generic;

public class SpriteLibraryReferenceChecker : EditorWindow
{
    private SpriteLibraryAsset spriteLibrary;

    [MenuItem("Tools/檢查 Sprite Library 是否使用場景圖")]
    public static void ShowWindow()
    {
        GetWindow<SpriteLibraryReferenceChecker>("Sprite Library 檢查工具");
    }

    void OnGUI()
    {
        GUILayout.Label("拖入 Sprite Library Asset", EditorStyles.boldLabel);
        spriteLibrary = EditorGUILayout.ObjectField("Sprite Library", spriteLibrary, typeof(SpriteLibraryAsset), false) as SpriteLibraryAsset;

        if (spriteLibrary != null && GUILayout.Button("開始檢查"))
        {
            CheckSpriteLibrary(spriteLibrary);
        }
    }

    private void CheckSpriteLibrary(SpriteLibraryAsset library)
    {
        var errors = new List<string>();

        var categories = library.GetCategoryNames();
        foreach (string category in categories)
        {
            var labels = library.GetCategoryLabelNames(category);
            foreach (string label in labels)
            {
                var sprite = library.GetSprite(category, label);
                if (sprite != null)
                {
                    string path = AssetDatabase.GetAssetPath(sprite);
                    if (string.IsNullOrEmpty(path))
                    {
                        errors.Add($"❌ [{category}] / [{label}] 使用了場景內 Sprite！");
                    }
                }
            }
        }

        if (errors.Count == 0)
        {
            EditorUtility.DisplayDialog("檢查結果", "✅ 沒有發現場景 Sprite，全部安全！", "OK");
        }
        else
        {
            Debug.LogWarning("⚠️ 檢查結果：");
            foreach (var e in errors)
            {
                Debug.LogError(e);
            }
            EditorUtility.DisplayDialog("檢查結果", $"發現 {errors.Count} 個 Label 使用場景圖！請查看 Console", "OK");
        }
    }
}
