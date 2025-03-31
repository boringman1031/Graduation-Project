using UnityEngine;

public class SuoZuiProjectile : MonoBehaviour
{
    public float speed;           // ���ʳt��
    public float damage;         // �ˮ`��
    public float lifeTime;        // �w�m���s�b�ɶ�

    // �ھڪ��a�¦V�A1 ��ܦV�k�A-1 ��ܦV��
    public float direction = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // �]�w�@�q�ɶ���۰ʾP��
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // �u�۹w�m����e���k���V���ʡA�����W direction�A�H�F��̾ڴ¦V�M�w���ʤ�V
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������a�]���]���a�� Tag �� "Player"�^
        if (collision.CompareTag("Player"))
            return;

        // ���ը��o�ĤH���}���]���]�ĤH�ϥ� CharactorBase�^
        CharactorBase enemy = collision.GetComponent<CharactorBase>();
        if (enemy != null)
        {
            // �y���ˮ`�G�o��i�H�ھڧA�� Attack �޿�i��վ�

            enemy.TakeDamage(damage, transform);

            // �i�b���ͦ������S�ġB����R������
            // (�Ҧp�GInstantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);)

            // �R����P����g��
            Destroy(gameObject);
        }
    }
}
