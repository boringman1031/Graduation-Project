using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*------------------by��z��-----------------------*/
public class GroundSensor : MonoBehaviour
{
    [SerializeField] int collisionType;         // �I������  ( 0 = Capsule, 1 = Box, 2 = Circle )

    Vector2 colliderSize_Origin;                // ��l�I���d�� XY     [�ƥ�]
    CapsuleCollider2D capsule;                  // [0] �q������w�q capsule
    BoxCollider2D box;                          // [1] �q������w�q box
    CircleCollider2D circle;                    // [2] �q������w�q circle

    Rigidbody2D body;                           // �w�q������ body

    [SerializeField] bool onGround;             // �O�_�b"�a�O�W"���A

    // �P�_�ϼh 1 << 6
    // �e����1�O�} 0�O��
    // �᭱�O�n�˴����ϼh ( Soild�bUnity��Layer�O6 )
    LayerMask solidMask = 1 << 6;              // �P�_�ϼh     (Solid�M��)       [�~���ե�]

    private void Awake()
    {
        body = this.transform.parent.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        // �w�q����I������
        switch (collisionType)
        {
            case 0: capsule = this.GetComponent<CapsuleCollider2D>(); colliderSize_Origin = capsule.size; break;
            case 1: box = this.GetComponent<BoxCollider2D>(); colliderSize_Origin = box.size; break;
            case 2: circle = this.GetComponent<CircleCollider2D>(); colliderSize_Origin = new Vector2(circle.radius, circle.radius); break;
        }
    }
    // �˴���a�O���ɭԧP�_������O�_���b�U��
    // �p�G�O���ܰ����ˬd
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int nLayer = collision.gameObject.layer;
        if (nLayer == 6)
        {
            if (body.velocity.y <= 0.01f) CheckCollision(collision, nLayer);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        int nLayer = collision.gameObject.layer;
        if (nLayer == 6) CheckCollision(collision, nLayer);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        int nLayer = collision.gameObject.layer;
        if (nLayer == 6) onGround = false;
    }

    void CheckCollision(Collision2D collision, int layerNumber)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            switch (layerNumber)
            {
                case 6: onGround |= collision.GetContact(i).normal.y >= 0.35f; break; // �p�G����U�����I���ɡA�P�w����b�a�O�W
            }
        }
    }
    // -------------�����A�եΤ��---------------
    public bool GetGround() { return onGround; }

    // ���� �P�_�ϼh
    public LayerMask GetSolidMask() { return solidMask; }
}
