using UnityEngine;

public class Skill_IceAndFire_Controller : MonoBehaviour
{
    [SerializeField] float speed = 10f; // 冰火技能的移动速度

    Rigidbody2D rb;

    private Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ActiveSkill();
    }

    public void Setup(Player player)
    {
        this.player = player;
    }

    private void ActiveSkill()
    {
        Destroy(gameObject, 5f);

        rb.linearVelocityX = speed * player.facingDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 判断碰撞体是否是敌人
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // 获取敌人身上的CharacterStats组件
            CharacterStats enemyStats = collision.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                player.stats.DoDamageOfMagic(enemyStats); // 造成魔法伤害

            }
        }
    }
}
