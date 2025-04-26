using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections.Generic;

public class SharedBoneBinder : EditorWindow
{
    private Sprite mainSprite;
    private List<Sprite> targetSprites = new List<Sprite>();
    private Vector2 scrollPos;

    [MenuItem("Tools/2D Animation/套用共用骨架到多張圖")]
    public static void ShowWindow()
    {
        GetWindow<SharedBoneBinder>("共用骨架綁定工具");
    }

    void OnGUI()
    {
        GUILayout.Label("主骨架來源 Sprite（先做好骨架的那張）", EditorStyles.boldLabel);
        mainSprite = (Sprite)EditorGUILayout.ObjectField("主 Sprite", mainSprite, typeof(Sprite), false);

        GUILayout.Space(10);
        GUILayout.Label("要套用骨架的其他 Sprite：", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
        int removeIndex = -1;
        for (int i = 0; i < targetSprites.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            targetSprites[i] = (Sprite)EditorGUILayout.ObjectField(targetSprites[i], typeof(Sprite), false);
            if (GUILayout.Button("移除", GUILayout.Width(50)))
                removeIndex = i;
            EditorGUILayout.EndHorizontal();
        }
        if (removeIndex >= 0)
            targetSprites.RemoveAt(removeIndex);
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("新增 Sprite"))
        {
            targetSprites.Add(null);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("⚡ 開啟 Sprite Editor 進行骨架 Copy / Paste"))
        {
            OpenSpriteEditorAndHint();
        }
    }

    void OpenSpriteEditorAndHint()
    {
        if (mainSprite == null || targetSprites.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "請先指定主 Sprite 與至少一張目標圖。", "OK");
            return;
        }

        EditorUtility.DisplayDialog("📌 操作說明",
@"1️⃣ 開啟主 Sprite 的 Sprite Editor
2️⃣ 進入 Skinning Editor，點 Copy Bones
3️⃣ 關閉後依序開啟以下目標 Sprite：
     - 點 Paste Bones
     - 按 Auto Geometry + Generate Weights
4️⃣ 套用完成後點 Apply 儲存

⬇️ 以下是你要處理的圖：
" + string.Join("\n", targetSprites.ConvertAll(s => s.name)),
"OK，馬上去做");

        // 自動選擇主圖，在 Project 中高亮
        Selection.activeObject = mainSprite;
        EditorGUIUtility.PingObject(mainSprite);
    }
}
