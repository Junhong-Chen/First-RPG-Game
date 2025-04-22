using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Clone_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed = 1f;

    private float timer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    private Transform closestEnemy;
    private int facingDir = 1;

    private bool canDuplicate;
    private float duplicateRatio = .33f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * colorLoosingSpeed);
        }

        if (sr.color.a < 0) {
            Destroy(gameObject);
        }
    }

    public void SetupClone(Transform _transform, Transform _closestEnemy, float stayDuration, bool canAttack, Vector3 offset, bool _canDuplicate, float _duplicateRatio)
    {
        transform.position = _transform.position + offset;
        closestEnemy = _closestEnemy;
        timer = stayDuration;
        canDuplicate = _canDuplicate;
        duplicateRatio = _duplicateRatio;

        if (canAttack) {
            anim.SetInteger("attackNumber", Random.Range(1, 4));
        }

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        timer = 0; // 攻击动作完成后，让残影消失
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
                PlayerManager.instance.player.stats.DoDamage(enemy.stats); // 造成伤害

            if (canDuplicate)
            {
                if (Random.Range(0f, 1f) < duplicateRatio) // 概率重复生成克隆体
                {
                    SkillManager.instance.clone.CreateClone(collider.transform, new Vector3(facingDir, 0)); // 生成克隆体
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
                facingDir = -1;
            }
        }
    }
}
