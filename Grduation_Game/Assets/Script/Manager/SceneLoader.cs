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
    [Header("廣播")]
    public VoidEventSO saveDataEvent;//儲存加載遊戲事件
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO openRandomCanvaEvent;//打開隨機挑戰面板
    public VoidEventSO openGoHomeEvent;//打開回家面板
    public SceneLoadEventSO unLoadSceneEvent;//卸載場景事件
    public TransitionEventSO transitionEvent;
    public SceneLoadedEventSO sceneLoadedEvent;
    public VoidEventSO unlockSkillEvent;//打開回家面板

    [Header("事件監聽")]
    public SceneLoadEventSO loadEventSO;//場景加載事件
    public VoidEventSO onAllEnemiesDefeated;//當場景中所有敵人被擊敗時的事件
    public VoidEventSO loadRandomSceneEvent;//隨機場景加載事件
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO gotoBossEvent;//(測試demo用)進入 Boss 事件
    public VoidEventSO BossDeadEvent;//Boss死亡事件
    public VoidEventSO goHomeEvent;//回家事件
   


    [Header("場景參數")]
    public GameSceneSO firstLoadScene;//第一個加載的場景(遊戲大聽)
    public GameSceneSO MuneScene;//主場景
    public GameSceneSO HomeScene;//租屋處場景
    public GameSceneSO NecessaryScene; //必要關卡 
    public GameSceneSO BossScene;//Boss場景
    public GameSceneSO Chap1ENDScene;//第一章結束場景

    private GameSceneSO sceneToLoad;//要新遊戲開始要加載的場景
    private GameSceneSO currentLoadScene;//當前加載的場景
    private GameSceneSO lastRandomScene; // 儲存上一次的隨機場景


    [Header("隨機場景列表")]
    [SerializeField] private List<GameSceneSO> randomScenes; // 可隨機選擇的場景列表
    public List<GameSceneSO> selectedSceneChoices = new List<GameSceneSO>();//隨機選擇的場景列表

    [Header("調整參數")]
    public Transform playerTrans;//玩家位置
    public Vector3 firstPosition;//第一個場景的位置
    public Vector3 menuPosition;//主菜單位置
    public float fadeTime;//淡入淡出時間

    private Vector3 positionToGo;//要傳送的位置
    private int challengeCount = 0;
    private int maxChallenges = 4; // 需要完成的隨機挑戰次數
    private bool fadeScreen;//是否淡出屏幕
    private bool isLoading;//是否正在加載

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(MuneScene, menuPosition, false);
        FindObjectOfType<UIManager>().UpdateChallengeCountUI(challengeCount);//更新挑戰次數UI
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        onAllEnemiesDefeated.OnEventRaised += OnOpenRandomCanvasEvent;//當場景中所有敵人被擊敗時通知UIManager
        loadRandomSceneEvent.OnEventRaised += OnLoadRandomScene;//隨機挑戰場景加載事件
        gotoBossEvent.OnEventRaised += OnGotoBossScene;//(測試demo用)進入 Boss 事件
        BossDeadEvent.OnEventRaised += OnGotoEndScene;//Boss死亡事件
        goHomeEvent.OnEventRaised += OnHomeEvent;//回家事件
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
        gotoBossEvent.OnEventRaised -= OnGotoBossScene;//(測試demo用)進入 Boss 事件
        BossDeadEvent.OnEventRaised -= OnGotoEndScene;//Boss死亡事件
        goHomeEvent.OnEventRaised -= OnHomeEvent;//回家事件
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnGotoBossScene()//(測試demo用)進入 Boss 事件
    {
        sceneToLoad = BossScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnGotoEndScene()//Boss死亡事件
    {
        loadEventSO.RaiseLoadRequestEvent(Chap1ENDScene, firstPosition, true);
    }

    private void OnHomeEvent()//回家事件
    {
        sceneToLoad = HomeScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnNewGameStartEvent()//新遊戲事件時執行
    {
        sceneToLoad = firstLoadScene;
        challengeCount=0; // 增加挑戰次數
        FindObjectOfType<UIManager>().UpdateChallengeCountUI(challengeCount);//更新挑戰次數UI
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
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
    private void OnLoadRandomScene()//隨機挑戰場景加載事件
    {
        if (challengeCount < maxChallenges)
        {

            selectedSceneChoices = GetThreeRandomScenes();
            // 呼叫 UIManager 顯示這三個選項（你等等會加這功能）
            FindObjectOfType<UIManager>().ShowRandomChallengeOptions(selectedSceneChoices);                          

        }
        else
        {        
            sceneToLoad = NecessaryScene;// 當挑戰次數達到 3，進入 Boss 前的特定關卡
            challengeCount = 0; // 重置挑戰次數
            Debug.Log("進入必要 關卡");
            OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        }
    }
    private List<GameSceneSO> GetThreeRandomScenes()
    {
        List<GameSceneSO> choices = new List<GameSceneSO>();
        List<GameSceneSO> pool = new List<GameSceneSO>(randomScenes);

        if (pool.Contains(lastRandomScene))
            pool.Remove(lastRandomScene);

        while (choices.Count < 3 && pool.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, pool.Count);
            choices.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return choices;
    }

    public void LoadChosenScene(GameSceneSO chosenScene)
    {
        lastRandomScene = chosenScene;
        challengeCount++;
        FindObjectOfType<UIManager>().UpdateChallengeCountUI(challengeCount);
        sceneToLoad = chosenScene;
        OnLoadRequestEvent(sceneToLoad, firstPosition, true);
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
        

        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadPreviousScene()//卸載當前場景
    {
        yield return new WaitForSeconds(fadeTime);
        if (fadeScreen)
        {
            transitionEvent.TransitionIn();//轉場
        }
        unLoadSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);//廣播:調整血條顯示
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
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadComplete;
    }   
    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> _handle)// 加載完成後執行
    {
        currentLoadScene = sceneToLoad;
        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true); // ✅ 啟用玩家 GameObject（Animator 和 SpriteSkin 會開始執行）   
        isLoading = false;

        // ✅ 對話系統使用的事件，立刻送出
        sceneLoadedEvent.RaiseEvent(currentLoadScene);

        // ✅ 等一幀後再觸發 afterSceneLoaded，確保 SpriteSkin 和 Animator 啟用完成
        StartCoroutine(DelayRaiseAfterSceneLoaded());

    }

    private IEnumerator DelayRaiseAfterSceneLoaded()
    {
        yield return null; // 等待一幀

        afterSceneLoadedEvent.RaiseEvent(); // ✅ 此時觸發動畫初始化，骨架狀態會正確
    }

    private void OnOpenRandomCanvasEvent()//當場景中所有敵人被擊敗時通知UIManager
    {
        if (currentLoadScene.sceneType != SceneType.Menu &&
            currentLoadScene.sceneType != SceneType.Boss &&
            currentLoadScene.sceneType != SceneType.Necessary)
        {
            // 只有第一關顯示 GoHomePanel
            if (currentLoadScene.sceneName == SceneName.Chap1_School)
            {
                unlockSkillEvent.RaiseEvent();
                openGoHomeEvent.RaiseEvent(); // 顯示回家面板
            }
            else
            {
                unlockSkillEvent.RaiseEvent();
                openRandomCanvaEvent.RaiseEvent(); // 顯示原本的隨機挑戰面板
            }
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
