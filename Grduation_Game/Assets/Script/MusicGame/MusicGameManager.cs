using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MusicGameManager : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO onNoteHit;
    public VoidEventSO onNoteMiss;

    [Header("�S��")]
    public GameObject hitEffect;
    public GameObject missEffect;
    public Transform effectPos;

    [Header("���ֱ���")]
    public bool startPlaying;
    public BeatScroller beatScroller;
    public AudioDefination audioDefination;
    public float songOffset = 0f; // ���ֶ}�l�ɶ�

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
        // �]�w���Ź��� Prefab
        notePrefabs = new Dictionary<KeyCode, GameObject>()
        {
            { KeyCode.Q, notePrefabQ },
            { KeyCode.W, notePrefabW },
            { KeyCode.E, notePrefabE }
        };

        // �]�w������ spawnPoint
        spawnPoints = new Dictionary<KeyCode, Transform>()
        {
            { KeyCode.Q, spawnPointQ },
            { KeyCode.W, spawnPointW },
            { KeyCode.E, spawnPointE }
        };

        LoadBeatMap("beatmap.csv"); // Ū�� CSV
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
        yield return new WaitForSeconds(songOffset); // ���ݭ��ֶ}�l�ɶ�

        if (audioDefination == null || audioDefination.audioClip == null)
        {
            Debug.LogError("AudioDefination �� audioClip ���]�w�I");
            yield break;
        }

        float startTime = Time.time; // �O���}�l�ɶ�
        foreach (var note in beatMap)
        {
            float waitTime = note.time - (Time.time - startTime); // �p�����ӵ��ݪ��ɶ�
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime); // �����ū��Ӯɶ����j�ͦ�
            }

            SpawnNote(note); // �ͦ�����
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
