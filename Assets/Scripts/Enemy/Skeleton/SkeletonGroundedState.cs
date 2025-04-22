using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Skeleton skeleton;
    protected Transform player;

    public SkeletonGroundedState(Enemy enmey, EnemyStateMachine stateMachine, string animBoolName, Skeleton skeleton) : base(enmey, stateMachine, animBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (skeleton.IsPlayerDetecteded() || Vector2.Distance(enemy.transform.position, player.position) < 2)
        {
            stateMachine.changeState(skeleton.battleState);
        }
    }
}
