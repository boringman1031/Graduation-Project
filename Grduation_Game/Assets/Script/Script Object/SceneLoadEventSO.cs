/*--------------by017----------------*/
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{  
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

    /// <summary>
    /// Ĳ�o�����[���ШD�ƥ�
    /// </summary>
    /// <param name="_locationToGo">�n�[��������</param>
    /// <param name="_position">�[����������m</param>
    /// <param name="fadeScreen">�O�_�H�X�̹�</param>
    public void RaiseLoadRequestEvent(GameSceneSO _locationToGo, Vector3 _position, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(_locationToGo, _position, fadeScreen);
    }
}
