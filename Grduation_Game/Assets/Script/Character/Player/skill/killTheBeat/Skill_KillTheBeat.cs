using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_KillTheBeat : MonoBehaviour, ISkillEffect
{
    public GameObject notePrefab; // ­µ²Å±¼¸¨ªº prefab
    public float waveInterval = 0.5f;
    public int waveCount = 3;
    public int notePerWave = 3;
    public float damage = 50f;

    private Transform origin;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        StartCoroutine(SpawnNotesRoutine());
    }

    public void SetPlayerAnimator(Animator animator) { }

    private IEnumerator SpawnNotesRoutine()
    {
        for (int i = 0; i < waveCount; i++)
        {
            List<CharactorBase> targets = FindClosestEnemies(origin.position, notePerWave);

            foreach (var enemy in targets)
            {
                Vector3 spawnPos = enemy.transform.position + new Vector3(0, 1.5f, 0);
                GameObject note = Instantiate(notePrefab, spawnPos, Quaternion.identity);

                FallingNote fn = note.GetComponent<FallingNote>();
                if (fn != null)
                {
                    fn.SetTarget(enemy.transform, damage);
                }
            }

            yield return new WaitForSeconds(waveInterval);
        }

        Destroy(gameObject);
    }

    private List<CharactorBase> FindClosestEnemies(Vector3 originPos, int count)
    {
        List<CharactorBase> result = new List<CharactorBase>();
        CharactorBase[] all = GameObject.FindObjectsOfType<CharactorBase>();

        List<CharactorBase> valid = new List<CharactorBase>();
        foreach (var enemy in all)
        {
            if (!enemy.CompareTag("Player")) valid.Add(enemy);
        }

        valid.Sort((a, b) =>
            Vector2.Distance(originPos, a.transform.position).CompareTo(
                Vector2.Distance(originPos, b.transform.position)));

        for (int i = 0; i < Mathf.Min(count, valid.Count); i++)
        {
            result.Add(valid[i]);
        }

        return result;
    }
}
