/*----------------BY017------------*/
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "GameScene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneReference;
    public string dialogKey; // 新增對話鍵字段

    [Header("教學設定")]
    public TutorialType tutorialType; // 新增：場景關聯的教學類型
}