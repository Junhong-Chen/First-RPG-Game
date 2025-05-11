using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Info")]
    public Vector2[] attacksMovement;
    public float counterAttackDuration = .2f;
    public bool isBusy { get; private set; }

    [Header("Move Info")]
    public float moveSpeed = 7;
    public float jumpForce = 12;
    public float swordReturnImpact = 2;
    private float moveSpeedDefault;

    [Header("Dash Info")]
    public float dashDuration = 0.2f;
    public float dashSpeed = 25f;
    public float dashDir { get; private set; }
    private float dashSpeedDefault;

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerSwordAimState swordAimState { get; private set; }
    public PlayerSwordCatchState swordCatchState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        skill = SkillManager.instance;

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "idle");
        moveState = new PlayerMoveState(this, stateMachine, "move");
        airState = new PlayerAirState(this, stateMachine, "jump");
        jumpState = new PlayerJumpState(this, stateMachine, "jump");
        dashState = new PlayerDashState(this, stateMachine, "dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "counterAttack");
        swordAimState = new PlayerSwordAimState(this, stateMachine, "swordAim");
        swordCatchState = new PlayerSwordCatchState(this, stateMachine, "swordCatch");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "jump");
        deadState = new PlayerDeadState(this, stateMachine, "die");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        moveSpeedDefault = moveSpeed;
        dashSpeedDefault = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.UseFlask();
        }
    }

    private void CheckForDashInput()
    {
        if (!SkillManager.instance.dash.dashUnlocked)
            return;

        if (stateMachine.currentState != blackholeState && !IsWallDetecteded() && Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    public IEnumerator BusyFor(float s)
    {
        isBusy = true;

        yield return new WaitForSeconds(s);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void AssignNewSword(GameObject _sword)
    {
        sword = _sword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(swordCatchState);

        Destroy(sword);
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public override void SlowSpeedBy(float _percentage, float _duration)
    {
        base.SlowSpeedBy(_percentage, _duration);

        moveSpeed = moveSpeedDefault * (1 - _percentage);
        dashSpeed = dashSpeedDefault * (1 - _percentage);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = moveSpeedDefault;
        dashSpeed = dashSpeedDefault;
    }
}
