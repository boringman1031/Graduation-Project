using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Transform player;

    [Header("°òÂ¦¼Æ­È")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();       
        currentSpeed = normalSpeed;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Update()
    {
        faceDir = (player.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x, rb.velocity.y);
    }
}
