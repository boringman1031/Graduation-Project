/*----------敵人遠程攻擊------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    private Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, 2f); // 自動消失
    }
    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<CharactorBase>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
