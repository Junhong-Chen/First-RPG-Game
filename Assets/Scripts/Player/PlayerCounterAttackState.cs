using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool cloneCreated;

    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("counterAttackSuccessful", false);

        cloneCreated = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, 0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null && collider.GetComponent<Enemy>().CanBeStunned())
            {
                stateTimer = 99;
                player.anim.SetBool("counterAttackSuccessful", true);

                if (!cloneCreated) // to prevent multiple clones
                {
                    cloneCreated = true;
                    player.skill.clone.CreateCloneOnCounterAttack(collider.transform);
                }

            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
