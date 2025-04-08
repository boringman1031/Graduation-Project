using System.Collections;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    private Transform player;
    private bool isTracking = false;
    public float moveSpeed = 5f;
    public float trackSpeed = 4f;

    [Header("擊破特效")]
    public GameObject deathEffectPrefab;
    public AudioClip deathSFX;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // 先朝 Y 軸飛向玩家高度
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

        // 高度到位後開始追蹤玩家
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

    // 被打爆時通知 BOSS 扣血
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
