using System.Collections;
using System.Collections.Generic;
using Autodesk.Fbx;
using UnityEngine;

public class Skill_Blackhole_Controller : MonoBehaviour
{
    //[SerializeField] private GameObject hotKeyPrefab;
    //[SerializeField] private List<KeyCode> keyCodeList;

    public bool canExit = false; // �Ƿ�����˳�����״̬

    private bool canGrow = true;
    private bool canShrink = false;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed; // �����ٶ�

    private int amountOfAttacks = 4; // ����Ĺ�������
    private float attackCooldown = 0.3f; // �������
    private float attackTimer; // ������ʱ��
    private float timer = 4; // ���ܳ���ʱ��

    private List<Transform> targets = new List<Transform>();

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _attackCooldown, float _duration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        attackCooldown = _attackCooldown;
        timer = _duration;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        if (timer <= 0) // ���ܳ���ʱ�����
        {
            timer = Mathf.Infinity;
            AttackFinish();
        }

        if (attackTimer <= 0)
        {
            attackTimer = attackCooldown; // ���ù�����ʱ��

            // ����е��˱�����ڶ�
            if (targets.Count > 0)
            {
                if (amountOfAttacks > 0) // ���й���
                {
                    Attack();
                    amountOfAttacks--;
                }
                else // ���ܽ���
                {
                    Invoke("AttackFinish", .3f);
                }
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), shrinkSpeed * Time.deltaTime);

            // ����ڶ���С��һ���̶ȣ����ٺڶ�
            if (transform.localScale.x <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.FreezeTime(true);
            AddEnemyToList(enemy.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.FreezeTime(false);
        }
    }

    private void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform);

    private void Attack()
    {
        int randomIndex = Random.Range(0, targets.Count);
        Transform chooseEnemy = targets[randomIndex];

        if (SkillManager.instance.clone.useCrystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            SkillManager.instance.crystal.ChooseEnemy(chooseEnemy);
        }
        else
        {
            float xOffset = Random.Range(0f, 1f) > .5f ? 1f : -1f;
            SkillManager.instance.clone.CreateClone(chooseEnemy, new Vector3(xOffset, 0));

            PlayerManager.instance.player.fx.MakeTransparent(true);
        }

    }
    private void AttackFinish()
    {
        targets.Clear(); // ��ռ�¼�ĵ���λ��
        canGrow = false; // ֹͣ����
        canShrink = true; // ��������
        canExit = true;
        PlayerManager.instance.player.fx.MakeTransparent(false);
    }
}
