using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "stunned", this);
        deadState = new SkeletonDeadState(this, stateMachine, "die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.changeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.changeState(deadState);
    }

    public override void ResetAction()
    {
        base.ResetAction();

        stateMachine.changeState(idleState);
    }
}
