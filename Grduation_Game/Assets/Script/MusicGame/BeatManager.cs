using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public float songOffset = 0f; // ���ֶ}�l�ɶ�
    public BeatScroller beatScroller; // **�[�J `BeatScroller`**

    [Header("���� Prefab ������")]
    public GameObject notePrefabQ;
    public GameObject notePrefabW;
    public GameObject notePrefabE;

    [Header("���ťͦ��I")]
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

        LoadBeatMap("beatmap.csv"); // Ū�� CSV
        StartCoroutine(SpawnNotes());
    }

    private void LoadBeatMap(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // ���L���D��
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

            // **�����Ŧ��� `BeatScroller` ���l����**
            newNote.transform.SetParent(beatScroller.transform);

            NoteOB noteOB = newNote.GetComponent<NoteOB>();
            if (noteOB != null)
            {
                noteOB.keyToPress = noteData.key; // �]�w��������
            }
        }
        else
        {
            Debug.LogError($"�S������ {noteData.key} ������ Prefab �� SpawnPoint�I");
        }
    }
}
