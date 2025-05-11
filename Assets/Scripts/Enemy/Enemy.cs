using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Move Info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 1f;
    private float moveSpeedDefault;

    [Header("Player Info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float playerCheckDistance = 10f;

    [Header("Battle Info")]
    public float battleTime = 10f;
    public float attackDistance = 1.5f;
    public float attackCooldown = 1f;
    [HideInInspector] public float lastTimeAttacked;

    [Header("Stunned Info")]
    public float stunnedDuration = .2f;
    public Vector2 stunnedDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    #region States
    public EnemyStateMachine stateMachine { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        moveSpeedDefault = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public RaycastHit2D IsPlayerDetecteded() => Physics2D.Raycast(wallCheck.position, transform.right, playerCheckDistance, whatIsPlayer);

    public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public void FreezeTime(bool frozen)
    {
        if (frozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = moveSpeedDefault;
            anim.speed = 1;
        }
    }

    public virtual IEnumerator FreezeTimeCoroutine(float seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(seconds);

        FreezeTime(false);
    }

    public virtual void FreezeTimeFor(float seconds)
    {
        StartCoroutine(FreezeTimeCoroutine(seconds));
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual void ResetAction() { }

    public override void SlowSpeedBy(float _percentage, float _duration)
    {
        base.SlowSpeedBy(_percentage, _duration);

        moveSpeed = moveSpeedDefault * (1 - _percentage);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = moveSpeedDefault;
    }
}
