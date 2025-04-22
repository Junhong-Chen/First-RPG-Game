using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration = 0.2f;
    protected bool isKnocked; // 是否被击退

    #region Components
    public Animator anim { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public CharacterStats stats { get; private set; }
    #endregion

    public int facingDir { get; private set; } = 1;
    public bool facingRight = true;

    public System.Action onFlip; // 事件：翻转

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SetVelocity(float x, float y)
    {
        if (isKnocked)
            return;

        rb.linearVelocity = new Vector2(x, y);

        FlipController(x);
    }

    #region Collision
    public bool IsGroundDetecteded() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetecteded() => Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        onFlip?.Invoke(); // 触发翻转事件
    }

    public virtual void SlowSpeedBy(float _percentage, float _duration)
    {
        CancelInvoke("ReturnDefaultSpeed"); // 取消之前的调用
        Invoke("ReturnDefaultSpeed", _duration);

        anim.speed = 1 - _percentage;
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    protected virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
            Flip();
        else if (x < 0 && facingRight)
            Flip();
    }
    #endregion

    public virtual void DamageImpact()
    {
        StartCoroutine("HitKnockback"); // 击退
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    public virtual void Die() { }
}
