using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetecteded())
            stateMachine.ChangeState(player.airState);
        else if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);
        else if (Input.GetKeyDown(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttackState);
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);
        else if (Input.GetKeyDown(KeyCode.Mouse1) && !hasSword())
            stateMachine.ChangeState(player.swordAimState);
        else if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackholeState);
    }

    private bool hasSword()
    {
        if (player.sword)
        {
            player.sword.GetComponent<Skill_Sword_Controller>().ReturnSword();
            return true;
        }
        else
        {
            return false;
        }
    }
}
