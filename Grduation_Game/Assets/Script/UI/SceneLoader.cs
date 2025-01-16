/*-------------BY017---------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour,ISaveable
{
    [Header("�s��")]
    public VoidEventSO saveDataEvent;//�x�s�[���C���ƥ�
    public VoidEventSO afterSceneLoadedEvent;
    public SceneLoadEventSO unLoadSceneEvent;
    public TransitionEventSO transitionEvent;

    [Header("�ƥ��ť")]
    public SceneLoadEventSO loadEventSO;//�����[���ƥ�
    public VoidEventSO loadRandomSceneEvent;//�H�������[���ƥ�
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;   

    [Header("�����Ѽ�")]
    public GameSceneSO firstLoadScene;//�Ĥ@�ӥ[��������(�C���jť)
    private GameSceneSO sceneToLoad;//�n�s�C���}�l�n�[��������
    public GameSceneSO MuneScene;//�D����
    private GameSceneSO currentLoadScene;//��e�[��������
  
    [Header("�H�������C��")]
    [SerializeField] private List<GameSceneSO> randomScenes; // �i�H����ܪ������C��

    [Header("�վ�Ѽ�")]
    public Transform playerTrans;//���a��m
    public Vector3 firstPosition;//�Ĥ@�ӳ�������m
    public float fadeTime;//�H�J�H�X�ɶ�

    private Vector3 positionToGo;//�n�ǰe����m
    private bool fadeScreen;//�O�_�H�X�̹�
    private  bool isLoading;//�O�_���b�[��

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(MuneScene, firstPosition, false);      
    } 
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        loadRandomSceneEvent.OnEventRaised += OnLoadRandomScene;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        loadRandomSceneEvent.OnEventRaised -= OnLoadRandomScene;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnNewGameStartEvent()//�s�C���ƥ�ɰ���
    {
        sceneToLoad = firstLoadScene;
        Debug.Log($"�s�C���}�l, �[������: {sceneToLoad.name}");
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
        Debug.Log($"sceneToLoad{firstLoadScene.name}");
    }
    private void OnBackToMenuEvent()//��^�D���ƥ�ɰ���
    {
        sceneToLoad = MuneScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    /// <summary>
    /// �H���D���޿�(�H����ܤ@�ӳ����C)
    /// </summary>
    /// <returns>��^�H����ܪ������C�p�G�C���šA��^ null�C</returns>
    private GameSceneSO GetRandomScene()
    {
        if (randomScenes == null || randomScenes.Count == 0)
        {
            Debug.LogError("Random scenes list is empty. Please add scenes to the list.");
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, randomScenes.Count);
        return randomScenes[randomIndex];
    }
    private void OnLoadRandomScene()//�H���D�Գ����[���ƥ�
    {
        GameSceneSO randomScene = GetRandomScene();
        if (randomScene != null)
        {
            loadEventSO.RaiseLoadRequestEvent(randomScene, firstPosition, true);
           // OnLoadRequestEvent(randomScene, positionToGo, true);
        }
        else
        {
            Debug.LogError("�S���s����");
        }
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
        Debug.Log($"��e����: {currentLoadScene?.name ?? "�L"}�A�Y�N�[���s����: {sceneToLoad.name}");
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
        Debug.Log($"�}�l��������: {currentLoadScene.name}");
        if (fadeScreen)
        {
            //���
            transitionEvent.TransitionIn();
        }
        Debug.Log($"���b��������: {currentLoadScene.name}");
        yield return new WaitForSeconds(fadeTime);
        unLoadSceneEvent.RaiseLoadRequestEvent(sceneToLoad,positionToGo,true);//�s��:�վ������
        yield return currentLoadScene.sceneReference.UnLoadScene();//������e����      
        playerTrans.gameObject.SetActive(false); //�������a�H��
        LoadNewScene();//�[���s����
    }
  

    /// <summary>
    /// �H���[���������ƥ�B�z�{�ǡC
    /// </summary>
    /// <param name="_sceneToGo">�n�[���������C</param>
    /// <param name="_positionToGo">���a�n�ǰe�쪺��m�C</param>
    /// <param name="fadeScreen">�O�_�H�X�̹��C</param>
   
    private void LoadNewScene()//�[���s����
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
        Debug.Log($"��e�[����������s��: {currentLoadScene.name}");
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //���
            transitionEvent.TransitionOut();
        }
        isLoading = false;
        if (currentLoadScene.sceneType == SceneType.Location)
        { 
            saveDataEvent.RaiseEvent();//�s��:�x�s�[���C���ƥ�
            afterSceneLoadedEvent.RaiseEvent();//�s��:�w�[�������ƥ�
        }
    }

    public DataDefination GetDataID()
    {
       return GetComponent<DataDefination>();
    }

    /// <summary>
    /// �O�s��e�����ƾڡC
    /// </summary>
    /// <param name="_data">�O�s�ƾڪ���H�C</param>
    public void GetSaveData(Data _data)
    {
        _data.SaveGameScene(currentLoadScene);
    }

    /// <summary>
    /// �[���O�s�������ƾڡC
    /// </summary>
    /// <param name="_data">�O�s�ƾڪ���H�C</param>
    public void LoadData(Data _data)
    {
        var playerID = playerTrans.GetComponent<DataDefination>().ID;
        if (_data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = _data.characterPosDict[playerID];
            sceneToLoad = _data.GetSaveScene();
            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
