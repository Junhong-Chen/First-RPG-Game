using UnityEngine;

public class Skill_Thunder_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;

    private Animator anim;

    private int damage;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        Invoke("ActiveSkill", .5f); // �ӳ� 0.5 ��󴥷����缼��
    }

    public void Setup(CharacterStats stats, int damage)
    {
        this.stats = stats;
        this.damage = damage;
    }

    private void ActiveSkill()
    {
        transform.position = new Vector2(stats.transform.position.x, stats.transform.position.y + 1f); // �ڼ���ʩ��ʱ�������������ʼλ��ΪĿ���ͷ��
        transform.localScale = new Vector3(3, 3, 3);

        stats.TakeDamage(damage); // ����˺�
        anim.SetTrigger("hit"); // �������綯��

        Destroy(gameObject, .5f);
    }
}
