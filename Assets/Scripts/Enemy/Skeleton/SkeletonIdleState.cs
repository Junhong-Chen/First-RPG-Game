using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enmey, EnemyStateMachine stateMachine, string animBoolName, Skeleton skeleton) : base(enmey, stateMachine, animBoolName, skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = skeleton.idleTime;

        skeleton.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            skeleton.Flip();
            stateMachine.changeState(skeleton.moveState);
        }
    }
}
