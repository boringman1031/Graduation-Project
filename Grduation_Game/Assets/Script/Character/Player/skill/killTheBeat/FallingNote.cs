using System.Collections;
using UnityEngine;

public class FallingNote : MonoBehaviour
{
    public GameObject hitEffect;
    public float fallDelay = 0.3f;
    public float fallSpeed = 8f;
    private float damage = 50f;

    private Transform target;
    private bool isFalling = false;

    public void SetTarget(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
        StartCoroutine(StartFall());
    }

    private IEnumerator StartFall()
    {
        yield return new WaitForSeconds(fallDelay);
        isFalling = true;
    }

    private void Update()
    {
        if (!isFalling) return;

        // ❗ 如果目標已不存在（死了或場景清除），直接爆炸
        if (target == null)
        {
            Debug.Log("目標已消失，音符自爆");
            HitTarget(); // 當作命中處理
            return;
        }

        Vector3 targetPos = target.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, fallSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            HitTarget();
        }
    }



    private void HitTarget()
    {
        if (hitEffect)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        if (target != null)
        {
            CharactorBase enemy = target.GetComponent<CharactorBase>();
            if (enemy != null)
                enemy.TakeDamage(damage, transform);
        }

        Destroy(gameObject);
    }

}
