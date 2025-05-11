using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("comboCounter", comboCounter);

        int attackDir = xInput != 0 ? (int)xInput : player.facingDir;
        // 角色攻击时的轻微位移
        player.SetVelocity(player.attacksMovement[comboCounter].x * attackDir, player.attacksMovement[comboCounter].y);
        // 位移持续时长
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetVelocity(0, 0);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
