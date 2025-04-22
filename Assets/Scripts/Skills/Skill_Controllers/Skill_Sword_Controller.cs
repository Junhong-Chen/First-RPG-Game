using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning = false;

    private float freezeTimeDuration;
    private float returnSpeed = 12;

    [Header("Pierce Info")]
    private int pierceAmount;

    [Header("Bounce Info")]
    private float bounceSpeed = 20;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTargets = new List<Transform>();
    private int targetIndex;

    [Header("Spin Info")]
    private bool isSpinning;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > 64)
        {
            DestroyMe();
        }

        if (canRotate)
            transform.right = rb.linearVelocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (distance < 1)
                player.CatchTheSword();
        }

        Bounce();
        Spin();
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player p, float _freezeTimeDuration, float _returnSpeed)
    {
        rb.linearVelocity = dir;
        rb.gravityScale = gravityScale;

        player = p;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;


        if (pierceAmount <= 0)
            anim.SetBool("rotation", true);

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);
    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;
    }

    public void SetuPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void ReturnSword()
    {
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    private void Bounce()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
            {
                SwordDamageEnemy(enemyTargets[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    private void Spin()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), Time.deltaTime);

                if (spinTimer < 0)
                {
                    wasStopped = false;
                    isSpinning = false;
                    isReturning = true;
                }

                // ��ͣʱ�����˺�
                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.GetComponent<Enemy>() != null)
                            SwordDamageEnemy(collider.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            SwordDamageEnemy(enemy);
        }

        SetupTargetsForBounce(collision);

        StuchInto(collision);
    }

    private void SwordDamageEnemy(Enemy enemy)
    {
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
        player.stats.DoDamage(enemy.stats);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.GetComponent<Enemy>() != null)
                        enemyTargets.Add(collider.transform);
                }
            }
        }
    }

    private void StuchInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            if (!wasStopped) StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0)
            return;

        transform.parent = collision.transform;

        anim.SetBool("rotation", false);
    }
}
