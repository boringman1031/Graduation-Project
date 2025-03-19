using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadedEventSO")]
public class SceneLoadedEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO> OnSceneLoaded;
    public void RaiseEvent(GameSceneSO scene)
    {
        OnSceneLoaded?.Invoke(scene);
    }
}