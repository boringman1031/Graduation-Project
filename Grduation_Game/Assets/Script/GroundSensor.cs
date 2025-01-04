using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*------------------by喇叭鎖-----------------------*/
public class GroundSensor : MonoBehaviour
{
    [SerializeField] int collisionType;         // 碰撞類型  ( 0 = Capsule, 1 = Box, 2 = Circle )

    Vector2 colliderSize_Origin;                // 原始碰撞範圍 XY     [備用]
    CapsuleCollider2D capsule;                  // [0] 從本物件定義 capsule
    BoxCollider2D box;                          // [1] 從本物件定義 box
    CircleCollider2D circle;                    // [2] 從本物件定義 circle

    Rigidbody2D body;                           // 定義父物件 body

    [SerializeField] bool onGround;             // 是否在"地板上"狀態

    // 判斷圖層 1 << 6
    // 前面的1是開 0是關
    // 後面是要檢測的圖層 ( Soild在Unity的Layer是6 )
    LayerMask solidMask = 1 << 6;              // 判斷圖層     (Solid專用)       [外部調用]

    private void Awake()
    {
        body = this.transform.parent.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        // 定義物件碰撞類型
        switch (collisionType)
        {
            case 0: capsule = this.GetComponent<CapsuleCollider2D>(); colliderSize_Origin = capsule.size; break;
            case 1: box = this.GetComponent<BoxCollider2D>(); colliderSize_Origin = box.size; break;
            case 2: circle = this.GetComponent<CircleCollider2D>(); colliderSize_Origin = new Vector2(circle.radius, circle.radius); break;
        }
    }
    // 檢測到地板的時候判斷父物件是否正在下落
    // 如果是的話執行檢查
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
                case 6: onGround |= collision.GetContact(i).normal.y >= 0.35f; break; // 如果物件下方受到碰撞時，判定物件在地板上
            }
        }
    }
    // -------------提取，調用方案---------------
    public bool GetGround() { return onGround; }

    // 提取 判斷圖層
    public LayerMask GetSolidMask() { return solidMask; }
}
