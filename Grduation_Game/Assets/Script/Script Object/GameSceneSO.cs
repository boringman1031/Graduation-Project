/*----------------BY017------------*/
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "GameScene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
   
    [Header("場景名稱")]
    public SceneName sceneName;

    [Header("BUTTON顯示名稱")]
    public string displayName;

    [Header("場景類型")]
    public SceneType sceneType;

    [Header("場景Reference")]
    public AssetReference sceneReference;

    [Header("對話鍵字段")]
    public string dialogKey; // 新增對話鍵字段

    [Header("可解鎖的技能")]
    public string skillToUnlock; // 解鎖的技能名稱

    [Header("可解鎖的技能")]
    public string classToUnlock; // 解鎖的職業名稱

    [Header("教學設定")]
    public TutorialType tutorialType; // 新增：場景關聯的教學類型  

   
}