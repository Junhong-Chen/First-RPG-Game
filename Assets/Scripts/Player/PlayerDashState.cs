using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState 
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.clone.CreateCloneOnDashStart(player.transform);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.clone.CreateCloneOnDashEnd(player.transform);

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (!player.IsGroundDetecteded() && player.IsWallDetecteded())
            stateMachine.ChangeState(player.wallSlideState);
        else if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
