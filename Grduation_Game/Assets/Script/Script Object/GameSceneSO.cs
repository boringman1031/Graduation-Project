/*----------------BY017------------*/
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "GameScene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneName sceneName;
    public SceneType sceneType;
    public AssetReference sceneReference;
    public string dialogKey; // 新增對話鍵字段
    public string skillToUnlock; // 解鎖的技能名稱

    [Header("教學設定")]
    public TutorialType tutorialType; // 新增：場景關聯的教學類型  
}