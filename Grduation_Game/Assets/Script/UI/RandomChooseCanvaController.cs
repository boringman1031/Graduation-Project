/*---------------BY: 017 ---------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomChooseCanvaController : MonoBehaviour
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    private  Vector3 positionToGo;

    
    private void ChangeButtomImage()
    {
        
    }

    public void TriggerAction()//Ĳ�o����I�s
    {
       loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
        Debug.Log($"�ǰe��{sceneToGo.name}");
    }
}



