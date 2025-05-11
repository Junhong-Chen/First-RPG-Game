using UnityEngine;

public class Skill_IceAndFire_Controller : MonoBehaviour
{
    [SerializeField] float speed = 10f; // �����ܵ��ƶ��ٶ�

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
        // �ж���ײ���Ƿ��ǵ���
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // ��ȡ�������ϵ�CharacterStats���
            CharacterStats enemyStats = collision.GetComponent<CharacterStats>();
            if (enemyStats != null)
            {
                player.stats.DoDamageOfMagic(enemyStats); // ���ħ���˺�

            }
        }
    }
}
