using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Skeleton skeleton;

    public SkeletonAttackState(Enemy enmey, EnemyStateMachine stateMachine, string animBoolName, Skeleton skeleton) : base(enmey, stateMachine, animBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        skeleton.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        skeleton.SetVelocity(0, 0); // ¹¥»÷Ê±£¬Í£Ö¹ÒÆ¶¯

        if (triggerCalled)
        {
            stateMachine.changeState(skeleton.battleState);
        }
    }
}
