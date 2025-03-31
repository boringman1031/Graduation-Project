using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossIdelState : BossBaseState
{
  
    public override void OnEnter(BossBase boss)
    {
        Debug.Log("¶i¤JªÅ¶¢ª¬ºA");
        currentBoss = boss;             
    }
    public override void LogicUpdate()
    {
       
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExit()
    {
       
    }
}
