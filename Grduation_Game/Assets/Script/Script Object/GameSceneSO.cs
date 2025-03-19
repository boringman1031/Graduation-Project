/*----------------BY017------------*/
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "GameScene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneReference;
    public string dialogKey; // �s�W�����r�q

    [Header("�оǳ]�w")]
    public TutorialType tutorialType; // �s�W�G�������p���о�����
}