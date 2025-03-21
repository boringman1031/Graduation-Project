using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class MusicGameManager : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO onNoteHit;
    public VoidEventSO onNoteMiss;

    [Header("�S��")]
    public GameObject hitEffect;
    public GameObject missEffect;
    public Transform effectPos;

    [Header("�q�� & �`����")]
    public string[] beatmapFiles; // **������ CSV �`���ɮ�**
    private int currentSongIndex = 0; // ��e��ܪ��q������

    [Header("���ֱ���")]
    public bool startPlaying=false;
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

    [Header("���ƨt��")]
    public int currentScore = 0;  // ��e����
    public int scorePerNote = 10; // �C�өR�������Ť���
    public int scoreMultiplier = 1; // �s���[��
    public int comboCount = 0; // �s����
    public int maxCombo = 0; // �O���̰��s����
    public int missedCount = 0; // �O�� Miss ����
    public int totalNotes; // �`���żƶq

    [Header("�p����")]
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
        LoadBeatMap(currentSongIndex); // Ū�������� Beatmap
        StartCoroutine(SpawnNotes());
        StartCoroutine(WaitForLastNote()); // �ʴ��̫�@�ӭ���      
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
        beatMap.Clear(); // �M���ª� Beatmap
        string fileName = beatmapFiles[songIndex];
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);     
        if (!File.Exists(filePath))
        {
            Debug.LogError($"���~: �䤣���ɮ� {filePath}");
            return;
        }
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // ���L���D��
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
        // **���ݭ��ֶ}�l**
        while (!startPlaying)
        {
            yield return null;
        }

        float songStartTime = Time.time; // **���ֶ}�l���ɶ��I**
        float audioStartTime = songStartTime - songOffset; // **�Ҽ{�����ɶ�**

        foreach (var note in beatMap)
        {
            float noteTime = note.time; // ���Ū��ɶ�
            float waitTime = noteTime - (Time.time - audioStartTime); // **�p�⥿�T�����ݮɶ�**

            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            SpawnNote(note); // �ͦ�����
        }
    }

    private IEnumerator WaitForLastNote()
    {
        if (beatMap.Count == 0)
        {
            Debug.LogError("�S�����ťi�ˬd�I");
            yield break;
        }

        // ���o�̫�@�ӭ��Ū��ɶ�
        float lastNoteTime = beatMap[beatMap.Count - 1].time;

        // ����̫�@�ӭ��ŧ����q�L�e�����ɶ� (���] 10 ���q�L)
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

        comboCount++;  // �s���W�[
        maxCombo = Mathf.Max(maxCombo, comboCount); // ��s�̰��s����

        currentScore += scorePerNote * scoreMultiplier; // �p�����
        scoreMultiplier++; // �s���ɥ[��

        Instantiate(hitEffect, effectPos.position, Quaternion.identity);
    }

    public void NoteMiss()
    {
        Debug.Log("Missed");

        missedCount++; // �O�� Miss ����
        comboCount = 0; // �s�����_
        scoreMultiplier = 1; // �s���[�����m

        Instantiate(missEffect, effectPos.position, Quaternion.identity);
    }

    private void EndGame()
    {
        float accuracy = ((totalNotes - missedCount) / (float)totalNotes) * 100f; // �p��ǽT�v

        string rank = "F"; // �w�]����
        if (accuracy >= 95) rank = "S";
        else if (accuracy >= 85) rank = "A";
        else if (accuracy >= 70) rank = "B";
        else if (accuracy >= 50) rank = "C";
        else rank = "D";

        /*Debug.Log(accuracy);
        Debug.Log("�C������");
        Debug.Log($"�`����: {currentScore}");
        Debug.Log($"�̰��s��: {maxCombo}");
        Debug.Log($"�ǽT�v: {accuracy:F2}%");
        Debug.Log($"����: {rank}");*/

        // **��ܵ���e��**
        scoreBoard.SetActive(true);
        score_text.text = $"�`����: {currentScore}";
        Combo_text.text = $"�̰��s��: {maxCombo}";
        Curracy_text.text = $"�ǽT�v: {accuracy:F2}%";
        Rank_text.text = $"����: {rank}";
        //ShowResults(currentScore, maxCombo, accuracy, rank);
    }
}
