/*----------------BY017------------*/
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "GameScene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneName sceneName;
    public SceneType sceneType;
    public AssetReference sceneReference;
}
