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

    [Header("事件廣播")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO openRandomCanvaEvent;
    public VoidEventSO openGoHomeEvent;
    public SceneLoadEventSO unLoadSceneEvent;
    public TransitionEventSO transitionEvent;
    public SceneLoadedEventSO sceneLoadedEvent;
    public VoidEventSO unlockSkillEvent;
    public VoidEventSO disablePlayerEvent;

    [Header("事件監聽")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO onAllEnemiesDefeated;
    public VoidEventSO loadRandomSceneEvent;
    public VoidEventSO newGameEvent;
    public VoidEventSO backToMenuEvent;
    public VoidEventSO gotoBossEvent;
    public VoidEventSO gotoBoss2Event;
    public VoidEventSO gotoBoss3Event;
    public VoidEventSO BossDeadEvent;
    public VoidEventSO Boss2DeadEvent;
    public VoidEventSO Boss3DeadEvent;
    public VoidEventSO goHomeEvent;
    public VoidEventSO gotoNesserySceneEvent;

    [Header("主要場景")]
    public GameSceneSO MuneScene;
    public GameSceneSO HomeScene;
    public GameSceneSO firstLoadScene;

    [Header("Boss場景")]
    public GameSceneSO BossScene;
    public GameSceneSO Boss2Scene;
    public GameSceneSO Boss3Scene;

    [Header("必要場景")]
    public GameSceneSO NecessaryScene;
    public GameSceneSO NecessaryScene2;
    public GameSceneSO NecessaryScene3;

    [Header("結束場景")]
    public GameSceneSO Chap1ENDScene;
    public GameSceneSO Chap2ENDScene;
    public GameSceneSO FinalENDScene;

    private GameSceneSO sceneToLoad;
    private GameSceneSO currentLoadScene;
    private GameSceneSO lastRandomScene;

    [Header("玩家位置")]
    public Transform playerTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;

    [Header("挑戰次數")]
    public int challengeCount = 0;
    public int maxChallenges = 4;

    [Header("目前章節")]
    public Chapter currentChapter = Chapter.Chap1;

    [Header("隨機場景列表")]
    public List<GameSceneSO> chap1RandomScenes;
    public List<GameSceneSO> chap2RandomScenes;
    public List<GameSceneSO> chap3RandomScenes;

    [Header("隨機場景選擇列表")]
    public List<GameSceneSO> selectedSceneChoices = new();
    
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeTime;

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(MuneScene, menuPosition, false);
        FindObjectOfType<UIManager>().UpdateChallengeCountUI(challengeCount);
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        onAllEnemiesDefeated.OnEventRaised += OnOpenRandomCanvasEvent;
        loadRandomSceneEvent.OnEventRaised += OnLoadRandomScene;
        gotoBossEvent.OnEventRaised += OnGotoBossScene;
        gotoBoss2Event.OnEventRaised += OnGotoBoss2Scene;
        gotoBoss3Event.OnEventRaised += OnGotoBoss3Scene;
        BossDeadEvent.OnEventRaised += OnGotoChap2;
        Boss2DeadEvent.OnEventRaised += OnGotoChap3;
        Boss3DeadEvent.OnEventRaised += OnGotoEndScene;
        goHomeEvent.OnEventRaised += OnHomeEvent;
        gotoNesserySceneEvent.OnEventRaised += OnLoadNecessaryScene;
        ((ISaveable)this).RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= OnNewGameStartEvent;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        onAllEnemiesDefeated.OnEventRaised -= OnOpenRandomCanvasEvent;
        loadRandomSceneEvent.OnEventRaised -= OnLoadRandomScene;
        gotoBossEvent.OnEventRaised -= OnGotoBossScene;
        gotoBoss2Event.OnEventRaised -= OnGotoBoss2Scene;
        gotoBoss3Event.OnEventRaised -= OnGotoBoss3Scene;
        BossDeadEvent.OnEventRaised -= OnGotoChap2;
        Boss2DeadEvent.OnEventRaised -= OnGotoChap3;
        Boss3DeadEvent.OnEventRaised -= OnGotoEndScene;
        goHomeEvent.OnEventRaised -= OnHomeEvent;
        gotoNesserySceneEvent.OnEventRaised -= OnLoadNecessaryScene;
        ((ISaveable)this).UnRegisterSaveData();
    }

    private void OnNewGameStartEvent()
    {
        sceneToLoad = firstLoadScene;
        challengeCount = 0;
        currentChapter = Chapter.Chap1;
        FindObjectOfType<UIManager>().UpdateChallengeCountUI(challengeCount);
        FindObjectOfType<SkillManager>().resetSkillAndClass();
        FindObjectOfType<SkillUIController>()?.RefreshSkillIcons();
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnBackToMenuEvent() => loadEventSO.RaiseLoadRequestEvent(MuneScene, firstPosition, true);
    private void OnHomeEvent() => loadEventSO.RaiseLoadRequestEvent(HomeScene, firstPosition, true);

    private void OnGotoBossScene() => loadEventSO.RaiseLoadRequestEvent(BossScene, firstPosition, true);
    private void OnGotoBoss2Scene() => loadEventSO.RaiseLoadRequestEvent(Boss2Scene, firstPosition, true);
    private void OnGotoBoss3Scene() => loadEventSO.RaiseLoadRequestEvent(Boss3Scene, firstPosition, true);

    private void OnGotoChap2()
    {
        challengeCount = 0;
        currentChapter = Chapter.Chap2;
        loadEventSO.RaiseLoadRequestEvent(Chap1ENDScene, firstPosition, true);
    }

    private void OnGotoChap3()
    {
        challengeCount = 0;
        currentChapter = Chapter.Chap3;
        loadEventSO.RaiseLoadRequestEvent(Chap2ENDScene, firstPosition, true);
    }

    private void OnGotoEndScene()
    {
        loadEventSO.RaiseLoadRequestEvent(FinalENDScene, firstPosition, true);
    }

    private void OnLoadRandomScene()
    {
        if (challengeCount < maxChallenges)
        {
            selectedSceneChoices = GetThreeRandomScenes();
            FindObjectOfType<UIManager>().ShowRandomChallengeOptions(selectedSceneChoices);
        }
        else
        {
            sceneToLoad = currentChapter switch
            {
                Chapter.Chap1 => NecessaryScene,
                Chapter.Chap2 => NecessaryScene2,
                Chapter.Chap3 => NecessaryScene3,
                _ => null
            };
            OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        }
    }

    private List<GameSceneSO> GetThreeRandomScenes()
    {
        List<GameSceneSO> pool = currentChapter switch
        {
            Chapter.Chap1 => new List<GameSceneSO>(chap1RandomScenes),
            Chapter.Chap2 => new List<GameSceneSO>(chap2RandomScenes),
            Chapter.Chap3 => new List<GameSceneSO>(chap3RandomScenes),
            _ => new List<GameSceneSO>()
        };

        if (pool.Contains(lastRandomScene)) pool.Remove(lastRandomScene);

        List<GameSceneSO> choices = new();
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

    private void OnLoadNecessaryScene()
    {
        sceneToLoad = currentChapter switch
        {
            Chapter.Chap1 => NecessaryScene,
            Chapter.Chap2 => NecessaryScene2,
            Chapter.Chap3 => NecessaryScene3,
            _ => null
        };
        OnLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnOpenRandomCanvasEvent()
    {
        if (currentLoadScene.sceneType != SceneType.Menu &&
            currentLoadScene.sceneType != SceneType.Boss &&
            currentLoadScene.sceneType != SceneType.Necessary)
        {
            // 第一關結束時顯示回家面板
            if (currentLoadScene.displayName == "教學關卡"|| currentLoadScene.displayName == "夜店")
            {
                unlockSkillEvent.RaiseEvent();
                openGoHomeEvent.RaiseEvent(); // ✅ 顯示回家面板
            }
            else
            {
                unlockSkillEvent.RaiseEvent();
                openRandomCanvaEvent.RaiseEvent(); // 顯示挑戰面板
            }
        }
    }

    private void OnLoadRequestEvent(GameSceneSO _locationToLaod, Vector3 _PosToGo, bool fadeScreen)
    {
        if (isLoading) return;
        isLoading = true;
        sceneToLoad = _locationToLaod;
        positionToGo = _PosToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene != null)
            StartCoroutine(UnLoadPreviousScene());
        else
            LoadNewScene();
    }

    private IEnumerator UnLoadPreviousScene()
    {
        yield return new WaitForSeconds(fadeTime);
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is ISkillEffect)
            {
                Destroy(obj.gameObject);
            }
        }
        if (fadeScreen) transitionEvent.TransitionIn();
        unLoadSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);
        yield return currentLoadScene.sceneReference.UnLoadScene();
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> _handle)
    {
        currentLoadScene = sceneToLoad;
        playerTrans.position = positionToGo;

        // ✅ 根據 GameSceneSO 設定是否開啟 Player
        if (sceneToLoad.hidePlayerOnLoad)
        {
            playerTrans.gameObject.SetActive(false);  // 關閉 Player
            disablePlayerEvent?.RaiseEvent();         // 廣播事件，通知關閉控制
        }
        else
        {
            playerTrans.gameObject.SetActive(true);   // 正常場景開啟 Player
        }

        isLoading = false;
        sceneLoadedEvent.RaiseEvent(currentLoadScene);

        if (currentLoadScene == Boss3Scene)
        {
            var boss = FindObjectOfType<BossController>();
            if (boss != null) boss.canAct = false;

            var player = FindObjectOfType<PlayerController>();
            if (player != null) player.playerInput.GamePlay.Disable(); // 禁用控制

            // 直接從這裡呼叫對話
            DialogManager.Instance?.StartDialog(currentLoadScene.dialogKey); // 請確保這些 BOSS Scene 的 GameSceneSO 有 dialogKey
        }


        StartCoroutine(DelayRaiseAfterSceneLoaded());
    }

    private IEnumerator DelayRaiseAfterSceneLoaded()
    {
        yield return null;
        afterSceneLoadedEvent.RaiseEvent();      
    }

    public DataDefination GetDataID() => GetComponent<DataDefination>();

    public void GetSaveData(Data _data) => _data.SaveGameScene(currentLoadScene);

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
