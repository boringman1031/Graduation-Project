/*---------------BY: 017 ---------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomChooseCanvaController : MonoBehaviour
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    private  Vector3 positionToGo;//傳送位置

    private void ChangeButtomImage()
    {
        
    }

    public void TriggerAction()//觸發轉場呼叫
    {
       loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
        Debug.Log($"傳送到{sceneToGo.name}");
    }
}



