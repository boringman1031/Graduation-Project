/*-------------BY017---------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    [Header("�s��")]
    public VoidEventSO saveDataEvent;//�x�s�[���C���ƥ�
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO openRandomCanvaEvent;//���}�H���D�ԭ��O
    public SceneLoadEventSO unLoadSceneEvent;//���������ƥ�
    public TransitionEventSO transitionEvent;
    public SceneLoadedEventSO sceneLoadedEvent;

    [Header("�ƥ��ť")]
    public SceneLoadEventSO loadEventSO;//�����[���ƥ�
    public VoidEventSO onAllEnemiesDefeated;//��������Ҧ��ĤH�Q���Ѯɪ��ƥ�
    public VoidEventSO loadRandomSceneEvent;//�H�������[���ƥ�
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO gotoBossEvent;//(����demo��)�i�J Boss �ƥ�
    public VoidEventSO BossDeadEvent;//Boss���`�ƥ�
    public VoidEventSO goHomeEvent;//�^�a�ƥ�

    [Header("�����Ѽ�")]
    public GameSceneSO firstLoadScene;//�Ĥ@�ӥ[��������(�C���jť)
    private GameSceneSO sceneToLoad;//�n�s�C���}�l�n�[��������
    private GameSceneSO currentLoadScene;//��e�[��������
    public GameSceneSO MuneScene;//�D����
    public GameSceneSO HomeScene;//���γB����
    public GameSceneSO NecessaryScene; //���n���d 
    public GameSceneSO BossScene;//Boss����
    public GameSceneSO Chap1ENDScene;//�Ĥ@����������

    [Header("�H�������C��")]
    [SerializeField] private List<GameSceneSO> randomScenes; // �i�H����ܪ������C��

    [Header("�վ�Ѽ�")]
    public Transform playerTrans;//���a��m
    public Vector3 firstPosition;//�Ĥ@�ӳ�������m
    public Vector3 menuPosition;//�D����m
    public float fadeTime;//�H�J�H�X�ɶ�

    private Vector3 positionToGo;//�n�ǰe����m
    private int challengeCount = 0;
    private int maxChallenges = 4; // �ݭn�������H���D�Ԧ���
    private bool fadeScreen;//�O�_�H�X�̹�
    private bool isLoading;//�O�_���b�[��

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(MuneScene, menuPosition, false);
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        onAllEnemiesDefeated.OnEventRaised += OnOpenRandomCanvasEvent;//��������Ҧ��ĤH�Q���Ѯɳq��UIManager
        loadRandomSceneEvent.OnEventRaised += OnLoadRandomScene;//�H���D�Գ����[���ƥ�
        gotoBossEvent.OnEventRaised += OnGotoBossScene;//(����demo��)�i�J Boss �ƥ�
        BossDeadEvent.OnEventRaised += OnGotoEndScene;//Boss���`�ƥ�
        goHomeEvent.OnEventRaised += OnHomeEvent;//�^�a�ƥ�
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        onAllEnemiesDefeated.OnEventRaised -= OnOpenRandomCanvasEvent;
        loadRandomSceneEvent.OnEventRaised -= OnLoadRandomScene;
        gotoBossEvent.OnEventRaised -= OnGotoBossScene;//(����demo��)�i�J Boss �ƥ�
        BossDeadEvent.OnEventRaised -= OnGotoEndScene;//Boss���`�ƥ�
        goHomeEvent.OnEventRaised -= OnHomeEvent;//�^�a�ƥ�
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnGotoBossScene()//(����demo��)�i�J Boss �ƥ�
    {
        sceneToLoad = BossScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnGotoEndScene()//Boss���`�ƥ�
    {
        loadEventSO.RaiseLoadRequestEvent(Chap1ENDScene, firstPosition, true);
    }

    private void OnHomeEvent()//�^�a�ƥ�
    {
        sceneToLoad = HomeScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnNewGameStartEvent()//�s�C���ƥ�ɰ���
    {
        sceneToLoad = firstLoadScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
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
        if (challengeCount < maxChallenges)
        {
            GameSceneSO randomScene = GetRandomScene();
            if (randomScene != null)
            {
                sceneToLoad = randomScene;                 
                OnLoadRequestEvent(sceneToLoad, firstPosition, true);                      
                Debug.Log($"�i�J{sceneToLoad.GetType()}�A�ٳ�{challengeCount}���D��");
            }
            else
            {
                Debug.LogError("�S���s����");
            }
        }
        else
        {        
            sceneToLoad = NecessaryScene;// ��D�Ԧ��ƹF�� 3�A�i�J Boss �e���S�w���d
            challengeCount = 0; // ���m�D�Ԧ���
            Debug.Log("�i�J���n ���d");
            OnLoadRequestEvent(sceneToLoad, firstPosition, true);
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

        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadPreviousScene()//������e����
    {
        yield return new WaitForSeconds(fadeTime);
        if (fadeScreen)
        {
            transitionEvent.TransitionIn();//���
        }
        unLoadSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);//�s��:�վ������
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
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadComplete;
    }   
    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> _handle)// �[�����������
    {
        currentLoadScene = sceneToLoad;
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        isLoading = false;
        sceneLoadedEvent.RaiseEvent(currentLoadScene);// Ĳ�o�a�����Ѽƪ��s�ƥ� ��ܨt�Ψϥ�
        afterSceneLoadedEvent.RaiseEvent(); // �s��:�w�[�������ƥ�
        challengeCount++; // �W�[�D�Ԧ���
        FindObjectOfType<UIManager>()?.UpdateChallengeCountUI(challengeCount);//��s�D�Ԧ���UI
        //saveDataEvent.RaiseEvent(); // �s��:�x�s�[���C���ƥ�

    }

    private void OnOpenRandomCanvasEvent()//��������Ҧ��ĤH�Q���Ѯɳq��UIManager
    {
        if (currentLoadScene.sceneType != SceneType.Menu &&
            currentLoadScene.sceneType != SceneType.Boss &&
            currentLoadScene.sceneType != SceneType.Necessary)
        {
            openRandomCanvaEvent.RaiseEvent();
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
