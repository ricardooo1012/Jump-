using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController player) : base(player)
    {

    }
    public override void Enter()
    {
        //player.Anim.Play("Idle");//用了animator控制动画，这个要不要都行，只是有这个方法可以控制
    }

    public override void LogicUpdate()
    {
        // 状态切换判断
        if (player.isGrounded)
        {
            player.HandleLandingState();
        }
    }

    public override void PhysicsUpdate()
    {
        // 墙壁滑行效果
        if (!player.isGrounded && player.isTouchWall)
        {
            // 减慢下落速度，模拟墙壁滑行
            if (player.rig.velocity.y < 0)
            {
                player.rig.velocity = new Vector2(player.rig.velocity.x, player.rig.velocity.y * 0.5f);
            }
        }
    }
}