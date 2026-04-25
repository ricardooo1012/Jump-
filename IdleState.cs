using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player)
    {

    }
    public override void Enter()
    {
        //player.Anim.Play("Idle");//用了animator控制动画，这个要不要都行，只是有这个方法可以控制
    }

    public override void LogicUpdate()
    {
        // 状态切换判断
        if (Mathf.Abs(player.InputX) > 0.1f)
            player.SwitchState(player.runState);
    }
}