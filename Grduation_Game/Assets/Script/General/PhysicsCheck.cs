using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("ÀË´ú°Ñ¼Æ")]
    public Vector2 bottomOffeset;
    public LayerMask groundLayer;
    public float checkRaduis;

    [Header("ª¬ºA")]
    public bool isGround;

    private void Update()
    {
        Check();
    }

   public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffeset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffeset, checkRaduis);
    }
}
