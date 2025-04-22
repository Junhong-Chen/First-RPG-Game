using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordCatchState : PlayerState
{
    private Transform sword;

    public PlayerSwordCatchState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        // ��ɫ�泯�������ķ���
        if (sword.position.x > player.transform.position.x && !player.facingRight || sword.position.x < player.transform.position.x && player.facingRight)
            player.Flip();

        // ���ɻ���ʱ�Ļ�����
        rb.linearVelocity = new Vector2(player.swordReturnImpact * -player.facingDir, 0);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
