using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MusicGameManager : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO onNoteHit;
    public VoidEventSO onNoteMiss;

    [Header("特效")]
    public GameObject hitEffect;
    public GameObject missEffect;
    public Transform effectPos;

    [Header("歌曲 & 節奏表")]
    public string[] beatmapFiles; // **對應的 CSV 節奏檔案**
    private int currentSongIndex = 0; // 當前選擇的歌曲索引

    [Header("音樂控制")]
    public bool startPlaying=false;
    public BeatScroller beatScroller;
    public AudioDefination audioDefination;
    public float songOffset = 0f; // 音樂開始時間

    [Header("音符 Prefab 對應表")]
    public GameObject notePrefabQ;
    public GameObject notePrefabW;
    public GameObject notePrefabE;

    [Header("音符生成點")]
    public Transform spawnPointQ;
    public Transform spawnPointW;
    public Transform spawnPointE;

    [Header("分數系統")]
    public int currentScore = 0;  // 當前分數
    public int scorePerNote = 10; // 每個命中的音符分數
    public int scoreMultiplier = 1; // 連擊加成
    public int comboCount = 0; // 連擊數
    public int maxCombo = 0; // 記錄最高連擊數
    public int missedCount = 0; // 記錄 Miss 次數
    public int totalNotes; // 總音符數量

    [Header("計分表")]
    public GameObject scoreBoard;
    public Text score_text;
    public Text Combo_text;
    public Text Curracy_text;
    public Text Rank_text;


    private Dictionary<KeyCode, GameObject> notePrefabs;
    private Dictionary<KeyCode, Transform> spawnPoints;

    [System.Serializable]
    public class NoteData
    {
        public float time;
        public KeyCode key;
    }

    public List<NoteData> beatMap = new List<NoteData>();

    private void Start()
    {      
        // 設定音符對應 Prefab
        notePrefabs = new Dictionary<KeyCode, GameObject>()
        {
            { KeyCode.Q, notePrefabQ },
            { KeyCode.W, notePrefabW },
            { KeyCode.E, notePrefabE }
        };

        // 設定對應的 spawnPoint
        spawnPoints = new Dictionary<KeyCode, Transform>()
        {
            { KeyCode.Q, spawnPointQ },
            { KeyCode.W, spawnPointW },
            { KeyCode.E, spawnPointE }
        };       
        LoadBeatMap(currentSongIndex); // 讀取對應的 Beatmap
        StartCoroutine(SpawnNotes());
        StartCoroutine(WaitForLastNote()); // 監測最後一個音符      
    }

    private void OnEnable()
    {
        onNoteHit.OnEventRaised += NoteHit;
        onNoteMiss.OnEventRaised += NoteMiss;
    }

    private void OnDisable()
    {
        onNoteHit.OnEventRaised -= NoteHit;
        onNoteMiss.OnEventRaised -= NoteMiss;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            startPlaying = true;
            beatScroller.hasStarted = true;
            audioDefination.PlayAudioClip();
        }

    }

    private void LoadBeatMap(int songIndex)
    {
        beatMap.Clear(); // 清空舊的 Beatmap
        string fileName = beatmapFiles[songIndex];
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);     
        if (!File.Exists(filePath))
        {
            Debug.LogError($"錯誤: 找不到檔案 {filePath}");
            return;
        }
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // 跳過標題行
        {
            string[] splitData = lines[i].Split(',');
            float time = float.Parse(splitData[0]);
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), splitData[1]);

            beatMap.Add(new NoteData { time = time, key = key });
        }
        totalNotes = beatMap.Count;
    }

    private IEnumerator SpawnNotes()
    {
        // **等待音樂開始**
        while (!startPlaying)
        {
            yield return null;
        }

        float songStartTime = Time.time; // **音樂開始的時間點**
        float audioStartTime = songStartTime - songOffset; // **考慮偏移時間**

        foreach (var note in beatMap)
        {
            float noteTime = note.time; // 音符的時間
            float waitTime = noteTime - (Time.time - audioStartTime); // **計算正確的等待時間**

            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            SpawnNote(note); // 生成音符
        }
    }

    private IEnumerator WaitForLastNote()
    {
        if (beatMap.Count == 0)
        {
            Debug.LogError("沒有音符可檢查！");
            yield break;
        }

        // 取得最後一個音符的時間
        float lastNoteTime = beatMap[beatMap.Count - 1].time;

        // 估算最後一個音符完全通過畫面的時間 (假設 10 秒內通過)
        float endTime = lastNoteTime + 10.0f;

        yield return new WaitForSeconds(endTime);      
        EndGame();
    }
    private void SpawnNote(NoteData noteData)
    {
        if (notePrefabs.TryGetValue(noteData.key, out GameObject notePrefab) &&
            spawnPoints.TryGetValue(noteData.key, out Transform spawnPoint))
        {
            GameObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

            // **讓音符成為 `BeatScroller` 的子物件**
            newNote.transform.SetParent(beatScroller.transform);

            NoteOB noteOB = newNote.GetComponent<NoteOB>();
            if (noteOB != null)
            {
                noteOB.keyToPress = noteData.key; // 設定對應按鍵
            }
        }
        else
        {
            Debug.LogError($"沒有對應 {noteData.key} 的音符 Prefab 或 SpawnPoint！");
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        comboCount++;  // 連擊增加
        maxCombo = Mathf.Max(maxCombo, comboCount); // 更新最高連擊數

        currentScore += scorePerNote * scoreMultiplier; // 計算分數
        scoreMultiplier++; // 連擊時加成

        Instantiate(hitEffect, effectPos.position, Quaternion.identity);
    }

    public void NoteMiss()
    {
        Debug.Log("Missed");

        missedCount++; // 記錄 Miss 次數
        comboCount = 0; // 連擊中斷
        scoreMultiplier = 1; // 連擊加成重置

        Instantiate(missEffect, effectPos.position, Quaternion.identity);
    }

    private void EndGame()
    {
        float accuracy = ((totalNotes - missedCount) / (float)totalNotes) * 100f; // 計算準確率

        string rank = "F"; // 預設評價
        if (accuracy >= 95) rank = "S";
        else if (accuracy >= 85) rank = "A";
        else if (accuracy >= 70) rank = "B";
        else if (accuracy >= 50) rank = "C";
        else rank = "D";

        /*Debug.Log(accuracy);
        Debug.Log("遊戲結束");
        Debug.Log($"總分數: {currentScore}");
        Debug.Log($"最高連擊: {maxCombo}");
        Debug.Log($"準確率: {accuracy:F2}%");
        Debug.Log($"評價: {rank}");*/

        // **顯示結算畫面**
        scoreBoard.SetActive(true);
        score_text.text = $"總分數: {currentScore}";
        Combo_text.text = $"最高連擊: {maxCombo}";
        Curracy_text.text = $"準確率: {accuracy:F2}%";
        Rank_text.text = $"評價: {rank}";
        //ShowResults(currentScore, maxCombo, accuracy, rank);
    }
}
