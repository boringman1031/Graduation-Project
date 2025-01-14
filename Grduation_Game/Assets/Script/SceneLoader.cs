/*-------------BY017---------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("�s��")]
    public VoidEventSO afterSceneLoadedEvent;
    public TransitionEventSO transitionEvent;
    [Header("�ƥ��ť")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;//�Ĥ@�ӥ[��������

    [Header("�վ�Ѽ�")]
    public Transform playerTrans;//���a��m
    public Vector3 firstPosition;//�Ĥ@�ӳ�������m
    public float fadeTime;//�H�J�H�X�ɶ�

    private GameSceneSO currentLoadScene;//��e�[��������
    private GameSceneSO sceneToLoad;//�n�[��������
    private Vector3 positionToGo;//�n�ǰe����m
    private bool fadeScreen;//�O�_�H�X�̹�
    private  bool isLoading;//�O�_���b�[��

    private void Start()
    {
        NewGame();
    } 
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    /// <summary>
    /// �B�z�����[���ШD�ƥ�C
    /// </summary>
    /// <param name="_locationToLaod">�n�[���������C</param>
    /// <param name="_PosToGo">���a�n�ǰe�쪺��m�C</param>
    /// <param name="fadeScreen">�O�_�H�X�̹��C</param>
    private void OnLoadRequestEvent(GameSceneSO _locationToLaod, Vector3 _PosToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = _locationToLaod;
        positionToGo = _PosToGo;
        this.fadeScreen = fadeScreen;
        Debug.Log($"�ǰe��{sceneToLoad.name}");

        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadPreviousScene()
    {      
        if (fadeScreen)
        {
            //���
            transitionEvent.TransitionIn();
        }
        yield return new WaitForSeconds(fadeTime);
        yield return currentLoadScene.sceneReference.UnLoadScene();

        //�������a�H��
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
       var loadingOption= sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
       loadingOption.Completed += OnLoadComplete;
    }
    ///<summary>
    /// �[�����������
    ///</summary>
    ///  <param name="_handle"></param>
    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> _handle)
    {
        currentLoadScene = sceneToLoad;
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //���
            transitionEvent.TransitionOut();
        }
        isLoading = false;
        if(currentLoadScene.sceneType== SceneType.Location)           
            afterSceneLoadedEvent.RaiseEvent();//�s��:�w�[�������ƥ�
    }
}
