using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Skeleton skeleton;

    public SkeletonStunnedState(Enemy enmey, EnemyStateMachine stateMachine, string animBoolName, Skeleton skeleton) : base(enmey, stateMachine, animBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = skeleton.stunnedDuration;

        rb.linearVelocity = new Vector2(-skeleton.facingDir * skeleton.stunnedDirection.x, skeleton.stunnedDirection.y);

        skeleton.fx.InvokeRepeating("RedColorBlink", 0, .1f);
    }

    public override void Exit()
    {
        base.Exit();

        skeleton.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.changeState(skeleton.idleState);
    }
}
