using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MusicGameManager : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO onNoteHit;
    public VoidEventSO onNoteMiss;

    [Header("特效")]
    public GameObject hitEffect;
    public GameObject missEffect;
    public Transform effectPos;

    [Header("音樂控制")]
    public bool startPlaying;
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

        LoadBeatMap("beatmap.csv"); // 讀取 CSV
        StartCoroutine(SpawnNotes());
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            startPlaying = true;
            beatScroller.hasStarted = true;
            audioDefination.PlayAudioClip();
        }
    }

    private void LoadBeatMap(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // 跳過標題行
        {
            string[] splitData = lines[i].Split(',');
            float time = float.Parse(splitData[0]);
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), splitData[1]);

            beatMap.Add(new NoteData { time = time, key = key });
        }
    }

    private IEnumerator SpawnNotes()
    {
        yield return new WaitForSeconds(songOffset); // 等待音樂開始時間

        if (audioDefination == null || audioDefination.audioClip == null)
        {
            Debug.LogError("AudioDefination 或 audioClip 未設定！");
            yield break;
        }

        float startTime = Time.time; // 記錄開始時間
        foreach (var note in beatMap)
        {
            float waitTime = note.time - (Time.time - startTime); // 計算應該等待的時間
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime); // 讓音符按照時間間隔生成
            }

            SpawnNote(note); // 生成音符
        }
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
        Instantiate(hitEffect, effectPos.position, Quaternion.identity);
    }

    public void NoteMiss()
    {
        Debug.Log("Missed");
        Instantiate(missEffect, effectPos.position, Quaternion.identity);
    }
}
