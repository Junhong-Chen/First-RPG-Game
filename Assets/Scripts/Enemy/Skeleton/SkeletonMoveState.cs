using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy enmey, EnemyStateMachine stateMachine, string animBoolName, Skeleton skeleton) : base(enmey, stateMachine, animBoolName, skeleton)
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

        skeleton.SetVelocity(skeleton.moveSpeed * skeleton.facingDir, rb.linearVelocity.y);

        if (!skeleton.IsGroundDetecteded() || skeleton.IsWallDetecteded())
        {
            stateMachine.changeState(skeleton.idleState);
        }
    }
}
