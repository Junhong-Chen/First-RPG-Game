using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private Skeleton skeleton;

    public SkeletonDeadState(Enemy enmey, EnemyStateMachine stateMachine, string animBoolName, Skeleton skeleton) : base(enmey, stateMachine, animBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        skeleton.SetVelocity(0, 0);
    }
}
