using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : PlayerState
{
    // 受击动画持续时间
    private float hurtDuration = 0.5f;
    private float hurtTimer = 0f;
    // 受击击退力
    private float hurtForce = 5f;
    // 攻击方向
    private Vector2 attackDirection = Vector2.left;

    public HurtState(PlayerController player) : base(player)
    {

    }

    // 设置攻击方向
    public void SetAttackDirection(Vector2 direction)
    {
        attackDirection = direction.normalized;
    }
    public override void Enter()
    {
        // 播放受击动画
        player.Anim.SetTrigger("Hurt");
        // 重置受击计时器
        hurtTimer = 0f;
        // 应用受击击退效果
        ApplyHurtForce();
        // 暂时停止移动
        player.InputX = 0;
    }

    public override void LogicUpdate()
    {
        // 受击动画持续时间
        hurtTimer += Time.deltaTime;
        
        // 受击结束后切换回相应状态
        if (hurtTimer >= hurtDuration)
        {
            if (player.isGrounded)
            {
                player.HandleLandingState();
            }
            else if (player.rig.velocity.y < 0)
            {
                player.SwitchState(player.fallState);
            }
            else
            {
                player.SwitchState(player.jumpState);
            }
        }
    }
    
    public override void PhysicsUpdate()
    {
        // 受击期间可以稍微调整位置
        player.rig.velocity = new Vector2(player.rig.velocity.x * 0.9f, player.rig.velocity.y);
    }

    // 应用受击击退力
    public void ApplyHurtForce()
    {
        // 根据攻击方向计算击退方向
        Vector2 hurtDirection = new Vector2(attackDirection.x, 0.8f).normalized;
        player.rig.velocity = new Vector2(hurtDirection.x * hurtForce, hurtDirection.y * hurtForce);
    }
}