using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAimState : PlayerState
{
    public PlayerSwordAimState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, 0); // ��׼ʱֹͣλ��

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ��ɫ�泯������������׼����
        if (mousePosition.x > player.transform.position.x && !player.facingRight || mousePosition.x < player.transform.position.x && player.facingRight)
            player.Flip();
    }
}