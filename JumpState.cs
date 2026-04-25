using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerController player) : base(player)
    {

    }
    public override void Enter()
    {

    }

    public override void LogicUpdate()
    {
        // 状态切换判断
        if (player.isGrounded)
        {
            player.HandleLandingState();
        }
        if(player.rig.velocity.y < 0)
        {
            player.SwitchState(player.fallState);
        }
    }
}