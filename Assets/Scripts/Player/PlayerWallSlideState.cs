using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // �����û�а������·����ʱ�����»����ٶȼ���
        if (yInput >= 0)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * .7f);

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.wallJumpState);
        else if (xInput != 0 && player.facingDir != xInput || player.IsGroundDetecteded())
            stateMachine.ChangeState(player.idleState);
    }
}
