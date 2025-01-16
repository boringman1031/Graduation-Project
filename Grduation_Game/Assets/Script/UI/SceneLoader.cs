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
    [Header("廣播")]
    public VoidEventSO saveDataEvent;//儲存加載遊戲事件
    public VoidEventSO afterSceneLoadedEvent;
    public SceneLoadEventSO unLoadSceneEvent;
    public TransitionEventSO transitionEvent;

    [Header("事件監聽")]
    public SceneLoadEventSO loadEventSO;//場景加載事件
    public VoidEventSO loadRandomSceneEvent;//隨機場景加載事件
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;   

    [Header("場景參數")]
    public GameSceneSO firstLoadScene;//第一個加載的場景(遊戲大聽)
    private GameSceneSO sceneToLoad;//要新遊戲開始要加載的場景
    public GameSceneSO MuneScene;//主場景
    private GameSceneSO currentLoadScene;//當前加載的場景
  
    [Header("隨機場景列表")]
    [SerializeField] private List<GameSceneSO> randomScenes; // 可隨機選擇的場景列表

    [Header("調整參數")]
    public Transform playerTrans;//玩家位置
    public Vector3 firstPosition;//第一個場景的位置
    public float fadeTime;//淡入淡出時間

    private Vector3 positionToGo;//要傳送的位置
    private bool fadeScreen;//是否淡出屏幕
    private  bool isLoading;//是否正在加載

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

    private void OnNewGameStartEvent()//新遊戲事件時執行
    {
        sceneToLoad = firstLoadScene;
        Debug.Log($"新遊戲開始, 加載場景: {sceneToLoad.name}");
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
        Debug.Log($"sceneToLoad{firstLoadScene.name}");
    }
    private void OnBackToMenuEvent()//返回主菜單事件時執行
    {
        sceneToLoad = MuneScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    /// <summary>
    /// 隨機挑戰邏輯(隨機選擇一個場景。)
    /// </summary>
    /// <returns>返回隨機選擇的場景。如果列表為空，返回 null。</returns>
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
    private void OnLoadRandomScene()//隨機挑戰場景加載事件
    {
        GameSceneSO randomScene = GetRandomScene();
        if (randomScene != null)
        {
            loadEventSO.RaiseLoadRequestEvent(randomScene, firstPosition, true);
           // OnLoadRequestEvent(randomScene, positionToGo, true);
        }
        else
        {
            Debug.LogError("沒有新場景");
        }
    }

    /// <summary>
    /// 處理場景加載請求事件。
    /// </summary>
    /// <param name="_locationToLaod">要加載的場景。</param>
    /// <param name="_PosToGo">玩家要傳送到的位置。</param>
    /// <param name="fadeScreen">是否淡出屏幕。</param>
    private void OnLoadRequestEvent(GameSceneSO _locationToLaod, Vector3 _PosToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = _locationToLaod;
        positionToGo = _PosToGo;
        this.fadeScreen = fadeScreen;
        Debug.Log($"當前場景: {currentLoadScene?.name ?? "無"}，即將加載新場景: {sceneToLoad.name}");
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
        Debug.Log($"開始卸載場景: {currentLoadScene.name}");
        if (fadeScreen)
        {
            //轉場
            transitionEvent.TransitionIn();
        }
        Debug.Log($"正在卸載場景: {currentLoadScene.name}");
        yield return new WaitForSeconds(fadeTime);
        unLoadSceneEvent.RaiseLoadRequestEvent(sceneToLoad,positionToGo,true);//廣播:調整血條顯示
        yield return currentLoadScene.sceneReference.UnLoadScene();//卸載當前場景      
        playerTrans.gameObject.SetActive(false); //關閉玩家人物
        LoadNewScene();//加載新場景
    }
  

    /// <summary>
    /// 隨機加載場景的事件處理程序。
    /// </summary>
    /// <param name="_sceneToGo">要加載的場景。</param>
    /// <param name="_positionToGo">玩家要傳送到的位置。</param>
    /// <param name="fadeScreen">是否淡出屏幕。</param>
   
    private void LoadNewScene()//加載新場景
    {
       var loadingOption= sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
       loadingOption.Completed += OnLoadComplete;
    }

    ///<summary>
    /// 加載完成後執行
    ///</summary>
    ///  <param name="_handle"></param>
    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> _handle)
    {
        currentLoadScene = sceneToLoad;
        Debug.Log($"當前加載的場景更新為: {currentLoadScene.name}");
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //轉場
            transitionEvent.TransitionOut();
        }
        isLoading = false;
        if (currentLoadScene.sceneType == SceneType.Location)
        { 
            saveDataEvent.RaiseEvent();//廣播:儲存加載遊戲事件
            afterSceneLoadedEvent.RaiseEvent();//廣播:已加載完成事件
        }
    }

    public DataDefination GetDataID()
    {
       return GetComponent<DataDefination>();
    }

    /// <summary>
    /// 保存當前場景數據。
    /// </summary>
    /// <param name="_data">保存數據的對象。</param>
    public void GetSaveData(Data _data)
    {
        _data.SaveGameScene(currentLoadScene);
    }

    /// <summary>
    /// 加載保存的場景數據。
    /// </summary>
    /// <param name="_data">保存數據的對象。</param>
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
