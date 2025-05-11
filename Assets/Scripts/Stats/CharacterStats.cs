using System.Collections;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CritDamage,
    CritChance,
    MaxHealth,
    Armor,
    Evasion,
    MagicResist,
    FireDamage,
    IceDamage,
    LightningDamage
}

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength = new(); // ������1������=1�㹥����/1%�����˺�
    public Stat agility = new(); // ���ݣ�1������=1%������/1%������
    public Stat intelligence = new(); // ������1������=1�㷨��������/1%�����ֿ�
    public Stat vitality = new(); // ������1������=5������ֵ/1%����ֿ�

    [Header("Offensive Stats")]
    public Stat damage = new(); // �˺�
    public Stat critDamage = new(); // �����˺�
    public Stat critChance = new(); // ������

    [Header("Defensive Stats")]
    public Stat maxHealth = new(); // �������ֵ
    public Stat armor = new(); // �������
    public Stat evasion = new(); // ������
    public Stat magicResist = new(); // �����ֿ�

    [Header("Magic Stats")]
    public Stat fireDamage = new(); // �����˺�
    public Stat iceDamage = new(); // ��˪�˺�
    public Stat lightningDamage = new(); // �׵��˺�

    public bool isBurned; // �Ƿ�ȼ��
    public bool isFrozen; // �Ƿ񱻱���
    public bool isElectrified; // �Ƿ񱻵��

    private float burnedTime = 1.5f; // ȼ��ʱ��
    private float burnedTimer = 0; // ȼ�ռ�ʱ��
    private float burnedDamageTimer = 0; // ȼ�ճ����˺���ʱ��
    private float burnedDamageCooldown = .3f; // ȼ�ճ����˺����
    private int burnedDamage; // ȼ���˺�

    private float frozenTime = 3f; // ����ʱ��
    private float frozenTimer = 0; // ������ʱ��
    private float frozenStrength = .5f; // ��������Ч��

    private bool isVulnerable = false; // �Ƿ�����״̬

    [SerializeField] private GameObject thunderPrefab;

    [SerializeField] private int health;

    protected Entity entity;
    private EntityFX fx;

    public bool isDead { get; private set; } = false; // �Ƿ�����

    public System.Action onHealthChange; // �¼�������ֵ�仯

    private ItemDrop dropSystem;

    protected virtual void Start()
    {
        health = GetMaxHealth();
        critDamage.SetValue(150); // �����˺�150%

        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFX>();

        onHealthChange?.Invoke();

        dropSystem = GetComponent<ItemDrop>(); // ��Ʒ����ϵͳ
    }

    protected virtual void Update()
    {
        HandleBurnEffect();
        HandleFrozenEffect();
        HandleElectrifiedEffect();
    }

    public Stat GetStatByType(StatType statType)
    {
        return statType switch
        {
            StatType.Strength => strength,
            StatType.Agility => agility,
            StatType.Intelligence => intelligence,
            StatType.Vitality => vitality,
            StatType.Damage => damage,
            StatType.CritDamage => critDamage,
            StatType.CritChance => critChance,
            StatType.MaxHealth => maxHealth,
            StatType.Armor => armor,
            StatType.Evasion => evasion,
            StatType.MagicResist => magicResist,
            StatType.FireDamage => fireDamage,
            StatType.IceDamage => iceDamage,
            StatType.LightningDamage => lightningDamage,
            _ => null
        };
    }

    public virtual void TakeDamage(int _damage)
    {
        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.1f); // ����״̬����� 10% ���˺���ֻ�����������˺�
        }

        DecreaseHealth(_damage);
        //Debug.Log(entity);
        entity.DamageImpact();
        fx.StartCoroutine("FlashFX");

        //Debug.Log("Damage: " + _damage);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void DoDamage(CharacterStats _targetStats, float multiplier = 1f)
    {
        if (canAvoidAttack(_targetStats) || isDead) return; // ���ܻ�����

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (canCritAttack())
        {
            totalDamage = CalcCritDamage(totalDamage);
        }

        totalDamage = Mathf.Max(0, totalDamage - _targetStats.armor.GetValue()); // �������
        //totalDamage *= (1 - _targetStats.vitality.GetValue() / 100f; // ����ֿ�

        totalDamage = Mathf.RoundToInt(totalDamage * multiplier); // �˺�����

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void Die()
    {
        isDead = true;

        entity.Die();

        dropSystem.Drop();
    }

    #region Magic Damage & Ailments
    public virtual void DoDamageOfMagic(CharacterStats _targetStats)
    {
        if (isDead) return; // ����

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int _magicDamage = intelligence.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + _magicDamage;
        totalMagicDamage -= _targetStats.magicResist.GetValue() + _targetStats.intelligence.GetValue();
        totalMagicDamage = Mathf.Max(0, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (_fireDamage > 0)
        {
            _targetStats.SetupBurnedDamage(Mathf.Max(1, Mathf.RoundToInt(_fireDamage * .2f))); // ����ȼ���˺������1��
        }

        _targetStats.ApplyAliments(_fireDamage > 0, _iceDamage > 0, _lightningDamage > 0);
    }

    public void ApplyAliments(bool _isBurned, bool _isFrozen, bool _isElectrified)
    {
        // ����Ч����ȼ�յ���
        isBurned = _isBurned;
        if (isBurned)
        {
            burnedTimer = burnedTime;
            fx.BurnedFxFor(burnedTime, burnedDamageCooldown); // ȼ����Ч
        }

        // ����Ч�������ٵ���
        isFrozen = _isFrozen;
        if (isFrozen)
        {
            frozenTimer = frozenTime;
            fx.FrozenFxFor(frozenTime); // ������Ч
            entity.SlowSpeedBy(frozenStrength, frozenTimer); // ��������
        }

        // ���Ч������ϵ���
        isElectrified = _isElectrified;
        if (isElectrified)
        {
            fx.ElectrifiedFxFor(.3f); // �����Ч
        }
    }

    protected virtual void HandleElectrifiedEffect()
    {
        if (isElectrified)
        {
            isElectrified = false;
        }
    }

    protected virtual void HandleFrozenEffect()
    {
        frozenTimer -= Time.deltaTime;

        if (frozenTimer < 0)
        {
            isFrozen = false;
        }
    }

    protected virtual void HandleBurnEffect()
    {
        burnedTimer -= Time.deltaTime;
        burnedDamageTimer -= Time.deltaTime;

        if (burnedTimer < 0)
        {
            isBurned = false;
        }

        if (isBurned && burnedDamageTimer < 0)
        {
            burnedDamageTimer = burnedDamageCooldown;

            DecreaseHealth(burnedDamage);

            if (health <= 0)
            {
                Die();
                isBurned = false; // ȼ������
            }
        }
    }

    public void SetupBurnedDamage(int _damage) => burnedDamage = _damage;

    public void MakeVulnerableFor(float duration)
    {
        StartCoroutine(VulnerableCoroutine(duration));
    }

    private IEnumerator VulnerableCoroutine(float duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(duration);
        isVulnerable = false;
    }
    #endregion

    #region Stat calculation
    public int GetMaxHealth()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public int GetHealth()
    {
        return health;
    }

    public virtual void IncreaseHealth(int _amount)
    {
        health += _amount;
        if (health > GetMaxHealth())
            health = GetMaxHealth();

        onHealthChange?.Invoke();
    }

    protected virtual void DecreaseHealth(int _amount)
    {
        health -= _amount;

        onHealthChange?.Invoke();
    }

    protected virtual void OnEvasion() { }

    private bool canAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    private bool canCritAttack()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }
        return false;
    }

    private int CalcCritDamage(int _damage)
    {
        int totalCritDamage = critDamage.GetValue() + strength.GetValue();
        return (int)(_damage * (totalCritDamage / 100f));
    }
    #endregion
}
