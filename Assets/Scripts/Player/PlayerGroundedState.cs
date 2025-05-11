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
        else if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);
        else if (Input.GetKeyDown(KeyCode.Mouse1) && player.skill.sword.swordUnlocked && !hasSword())
            stateMachine.ChangeState(player.swordAimState);
        else if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
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
            return !player.skill.sword.CanUseSkill(); // 如果技能 CD 已好，返回 false，进入 swordAimState 状态；如果技能在 CD 中，返回 true，不进入 swordAimState 状态
        }
    }
}
