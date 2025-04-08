using System.Collections;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    private Transform player;
    private bool isTracking = false;
    public float moveSpeed = 5f;
    public float trackSpeed = 4f;

    [Header("���}�S��")]
    public GameObject deathEffectPrefab;
    public AudioClip deathSFX;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ���� Y �b���V���a����
        Vector3 target = new Vector3(transform.position.x, player.position.y, transform.position.z);
        StartCoroutine(MoveToHeight(target));
    }

    IEnumerator MoveToHeight(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // ���ר���}�l�l�ܪ��a
        isTracking = true;
    }

    void Update()
    {
        if (isTracking && player != null)
        {
            Vector3 targetPos = new Vector3(player.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, trackSpeed * Time.deltaTime);
        }
    }

    // �Q���z�ɳq�� BOSS ����
    public void OnDead()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        }

        FindObjectOfType<BossController>().OnTentacleDestroyed();
        Destroy(gameObject);
    }

}
