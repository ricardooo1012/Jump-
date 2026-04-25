using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : PlayerState
{
    //跳跃方向
    private Vector2 wallJumpDirection;

    public WallJumpState(PlayerController player) : base(player)
    {

    }
    public override void Enter()
    {
        // 计算墙跳方向
        bool isFacingRight = player.facingDir > 0;//facingDir>0往右
        wallJumpDirection = isFacingRight ? Vector2.left : Vector2.right;

        // 执行墙跳
        player.rig.velocity = new Vector2(wallJumpDirection.x * player.WallJumpForce, player.JumpForce);
        // 重置跳跃次数
        player.JumpCount = player.maxJumpCount - 1;
        // 播放墙跳动画
        player.Anim.SetTrigger("WallJump");
    }

    public override void LogicUpdate()
    {
        //状态切换判断
        if (player.isGrounded)
        {
            player.HandleLandingState();
        }
        else if (player.rig.velocity.y < 0)
        {
            player.SwitchState(player.fallState);
        }

    }
}