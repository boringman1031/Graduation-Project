using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin :EnemyBase
{
    override public void Move()
    {
        base.Move();
        anim.SetBool("Run", true);
    }

}
