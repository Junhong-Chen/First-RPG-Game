using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * xInput * 0.7f, rb.linearVelocity.y);

        if (player.IsGroundDetecteded())
            stateMachine.ChangeState(player.idleState);
        else if (player.IsWallDetecteded())
            stateMachine.ChangeState(player.wallSlideState);
    }
}
