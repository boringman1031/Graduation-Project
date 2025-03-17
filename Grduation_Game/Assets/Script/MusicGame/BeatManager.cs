using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public float songOffset = 0f; // 音樂開始時間
    public BeatScroller beatScroller; // **加入 `BeatScroller`**

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
        notePrefabs = new Dictionary<KeyCode, GameObject>()
        {
            { KeyCode.Q, notePrefabQ },
            { KeyCode.W, notePrefabW },
            { KeyCode.E, notePrefabE }
        };

        spawnPoints = new Dictionary<KeyCode, Transform>()
        {
            { KeyCode.Q, spawnPointQ },
            { KeyCode.W, spawnPointW },
            { KeyCode.E, spawnPointE }
        };

        LoadBeatMap("beatmap.csv"); // 讀取 CSV
        StartCoroutine(SpawnNotes());
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
        yield return new WaitForSeconds(songOffset);

        foreach (var note in beatMap)
        {
            yield return new WaitForSeconds(note.time);
            SpawnNote(note);
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
}
