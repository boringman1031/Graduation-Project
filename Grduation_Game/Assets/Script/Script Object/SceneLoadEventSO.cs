/*--------------by017----------------*/
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{  
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

    /// <summary>
    /// 觸發場景加載請求事件
    /// </summary>
    /// <param name="_locationToGo">要加載的場景</param>
    /// <param name="_position">加載場景的位置</param>
    /// <param name="fadeScreen">是否淡出屏幕</param>
    public void RaiseLoadRequestEvent(GameSceneSO _locationToGo, Vector3 _position, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(_locationToGo, _position, fadeScreen);
    }
}
