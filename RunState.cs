using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player)
    {

    }
    public override void Enter()
    {

    }
    public override void LogicUpdate()
    {
        // 状态切换判断
        if (Mathf.Abs(player.InputX) < 0.1f)
            player.SwitchState(player.idleState);
    }
}