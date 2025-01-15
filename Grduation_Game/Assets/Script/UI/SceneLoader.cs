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
    public VoidEventSO afterSceneLoadedEvent;
    public SceneLoadEventSO unLoadSceneEvent;
    public TransitionEventSO transitionEvent;

    [Header("事件監聽")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;

    [Header("場景參數")]
    public GameSceneSO firstLoadScene;//第一個加載的場景
    public GameSceneSO MuneScene;//主場景
    private GameSceneSO currentLoadScene;//當前加載的場景
    private GameSceneSO sceneToLoad;//要加載的場景

    [Header("調整參數")]
    public Transform playerTrans;//玩家位置
    public Vector3 firstPosition;//第一個場景的位置
    public float fadeTime;//淡入淡出時間

    private Vector3 positionToGo;//要傳送的位置
    private bool fadeScreen;//是否淡出屏幕
    private  bool isLoading;//是否正在加載

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(MuneScene, firstPosition, true);      
    } 
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

   

    private void OnNewGameStartEvent()//新遊戲事件
    {
        sceneToLoad = firstLoadScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }
    private void OnBackToMenuEvent()//返回主菜單事件
    {
        sceneToLoad = MuneScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
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
        Debug.Log($"傳送到{sceneToLoad.name}");

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
            //轉場
            transitionEvent.TransitionIn();
        }

        yield return new WaitForSeconds(fadeTime);
        unLoadSceneEvent.RaiseLoadRequestEvent(sceneToLoad,positionToGo,true);//廣播:調整血條顯示
        yield return currentLoadScene.sceneReference.UnLoadScene();
        
        playerTrans.gameObject.SetActive(false); //關閉玩家人物
        LoadNewScene();//加載新場景
    }

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
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //轉場
            transitionEvent.TransitionOut();
        }
        isLoading = false;
        if(currentLoadScene.sceneType== SceneType.Location)           
            afterSceneLoadedEvent.RaiseEvent();//廣播:已加載完成事件
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
        if (_data.characterPosition.ContainsKey(playerID))
        {
            positionToGo = _data.characterPosition[playerID];
            sceneToLoad = _data.GetSaveScene();
            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
