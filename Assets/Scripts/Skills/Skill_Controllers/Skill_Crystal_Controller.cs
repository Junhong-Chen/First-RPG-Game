using UnityEngine;

public class Skill_Crystal_Controller: MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cc => GetComponent<CircleCollider2D>();

    private float duration = 5f;

    private bool canExplode = false;
    private bool canMove = false;
    private bool canGrow = false;
    private float moveSpeed = 1f;
    private float growSpeed = 4f;

    private Transform closestEnemy;

    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCrystal(float _duration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy)
    {
        duration = _duration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    public void DestroyCrystal() => Destroy(gameObject);
    public void ChooseEnemy(Transform _closestEnemy)
    {
        closestEnemy = _closestEnemy;
    }

    public void Completed()
    {
        if (canExplode)
        {
            anim.SetTrigger("explode");
            canGrow = true;
        }
        else
            DestroyCrystal();
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Completed();
        }

        if (canMove && closestEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.position) < .5f)
            {
                Completed();
                canMove = false;
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cc.radius);

        foreach (var collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Player player = PlayerManager.instance.player;

                player.stats.DoDamageOfMagic(enemy.stats); // 造成伤害

                Inventory.Instance.GetEquipmentByType(EquipmentType.Accessory)?.Effects(player, enemy); // 触发饰品特效
            }
        }
    }
}
