using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RandomChooseCanvaController : MonoBehaviour
{
    [SerializeField]
    private Button scene1;
    [SerializeField]
    private Button scene2;
    [SerializeField]
    private Button scene3;

    private void Awake()
    {
    }
   
    void Update()
    {
        
    }
    private void ChangeButtomImage()
    {
        
    }
    private void OnDestroy()
    {
        scene1.onClick.RemoveAllListeners();
        scene2.onClick.RemoveAllListeners();
        scene3.onClick.RemoveAllListeners();
    } 
}



