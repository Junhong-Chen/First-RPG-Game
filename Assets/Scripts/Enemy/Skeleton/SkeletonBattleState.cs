using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Skeleton skeleton;
    private Transform player;
    private int moveDir;

    public SkeletonBattleState(Enemy enmey, EnemyStateMachine stateMachine, string animBoolName, Skeleton skeleton) : base(enmey, stateMachine, animBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        stateTimer = skeleton.battleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        float distance = System.Math.Abs(player.position.x - skeleton.transform.position.x);

        if (distance < skeleton.attackDistance && CanAttack())
        {
            stateTimer = skeleton.battleTime;
            stateMachine.changeState(skeleton.attackState);
        }
        else if (stateTimer < 0 || distance > 12)
        {
            stateMachine.changeState(skeleton.idleState);
        } 
        else
        {
            if (player.position.x > skeleton.transform.position.x)
            {
                moveDir = 1;
            }
            else
            {
                moveDir = -1;
            }

            skeleton.SetVelocity(skeleton.moveSpeed * moveDir, rb.linearVelocity.y);
        }

    }

    private bool CanAttack()
    {
        return Time.time > skeleton.lastTimeAttacked + skeleton.attackCooldown;
    }
}
