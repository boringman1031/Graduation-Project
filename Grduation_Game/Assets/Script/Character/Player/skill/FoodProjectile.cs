using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProjectile : MonoBehaviour
{
    // ���Ӥ��P�����Ϥ��A�Цb Inspector ����
    public Sprite[] foodSprites;

    // �R���S�Ĺw�m���]�����ɲ��͡^
    public GameObject hitEffectPrefab;
    // �R������
    public AudioClip hitSound;

    // ���Y�ɪ�����
    public AudioClip launchSound;

    // �����ˮ`
    private float damage;
    // ���ʤ�V�P�t��
    private Vector2 direction;
    private float speed;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // �C����ਤ��
    public float rotationSpeed = 180f;

    // �]�w�����ˮ`
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    // �]�w���ʤ�V�P�t��
    public void SetDirection(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.velocity = this.direction * this.speed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // �H���D��@�ӭ����Ϥ�
        if (foodSprites != null && foodSprites.Length > 0 && sr != null)
        {
            int index = Random.Range(0, foodSprites.Length);
            sr.sprite = foodSprites[index];
        }
    }

    private void Start()
    {
        // ������Y�ɭ���
        if (launchSound != null)
        {
            AudioSource.PlayClipAtPoint(launchSound, transform.position);
        }

        // �۰ʾP���w�m�� (�קK���ɶ��s�b)
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        // �������������
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ư����a�]���]���a Tag �� "Player"�^
        if (collision.CompareTag("Player"))
            return;

        // �ˬd�I����H�O�_�� CharactorBase �ե�
        CharactorBase target = collision.GetComponent<CharactorBase>();

        if (target != null)
        {
            // ����R������
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            // �ͦ��R���S��
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, target.transform.position, Quaternion.identity);
            }

            // �����ˮ`
            float newHealth = target.CurrentHealth - damage;
            if (newHealth > 0)
            {
                target.CurrentHealth = newHealth;
                target.OnTakeDamage?.Invoke(transform);
            }
            else
            {
                target.CurrentHealth = 0;
                target.OnDead?.Invoke();
            }
            target.OnHealthChange?.Invoke(target);

            // ������P���w�m��
            Destroy(gameObject);
        }
    }
}
