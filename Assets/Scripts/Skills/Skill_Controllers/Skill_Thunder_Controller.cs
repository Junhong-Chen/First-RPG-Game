using UnityEngine;

public class Skill_Thunder_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;

    private Animator anim;

    private int damage;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        Invoke("ActiveSkill", .5f); // 延迟 0.5 秒后触发闪电技能
    }

    public void Setup(CharacterStats stats, int damage)
    {
        this.stats = stats;
        this.damage = damage;
    }

    private void ActiveSkill()
    {
        transform.position = new Vector2(stats.transform.position.x, stats.transform.position.y + 1f); // 在技能施放时，设置闪电的起始位置为目标的头顶
        transform.localScale = new Vector3(3, 3, 3);

        stats.TakeDamage(damage); // 造成伤害
        anim.SetTrigger("hit"); // 播放闪电动画

        Destroy(gameObject, .5f);
    }
}
