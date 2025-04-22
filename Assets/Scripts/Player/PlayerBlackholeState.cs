using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;

    private float defaultGravityScale;

    public PlayerBlackholeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        stateTimer = flyTime;

        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0; // 禁用重力
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = defaultGravityScale; // 恢复重力
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 15);
        else
        {
            rb.linearVelocity = new Vector2(0, -.1f);

            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}
