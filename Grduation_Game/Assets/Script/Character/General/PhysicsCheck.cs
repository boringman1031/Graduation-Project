using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D collder;  
    [Header("�˴��Ѽ�")]
    public bool manual;
    public Vector2 bottomOffeset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public LayerMask groundLayer;
    public float checkRaduis;

    [Header("���A")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    private void Awake()
    {
        collder = GetComponent<CapsuleCollider2D>();
        if(!manual)
        {
            rightOffset = new Vector2((collder.bounds.size.x+collder.offset.x)/2,collder.size.y/2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }
    private void Update()
    {
        Check();
    }

   public void Check()
    {        
        //�˴��a��
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffeset, checkRaduis, groundLayer);
        //�˴�����
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
        //�˴��k��
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffeset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
