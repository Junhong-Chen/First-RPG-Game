using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength = new(); // 力量：1点力量=1点攻击力/1%暴击伤害
    public Stat agility = new(); // 敏捷：1点敏捷=1%暴击率/1%闪避率
    public Stat intelligence = new(); // 智力：1点智力=1点法术攻击力/1%法术抵抗
    public Stat vitality = new(); // 体力：1点体力=5点生命值/1%物理抵抗

    [Header("Offensive Stats")]
    public Stat damage = new(); // 伤害
    public Stat critDamage = new(); // 暴击伤害
    public Stat critChance = new(); // 暴击率

    [Header("Defensive Stats")]
    public Stat maxHealth = new(); // 最大生命值
    public Stat armor = new(); // 物理防御
    public Stat evasion = new(); // 闪避率
    public Stat magicResist = new(); // 法术抵抗

    [Header("Magic Stats")]
    public Stat fireDamage = new(); // 火焰伤害
    public Stat iceDamage = new(); // 冰霜伤害
    public Stat lightningDamage = new(); // 雷电伤害

    public bool isBurned; // 是否被燃烧
    public bool isFrozen; // 是否被冰冻
    public bool isElectrified; // 是否被电击

    private float burnedTime = 1.5f; // 燃烧时间
    private float burnedTimer = 0; // 燃烧计时器
    private float burnedDamageTimer = 0; // 燃烧持续伤害计时器
    private float burnedDamageCooldown = .3f; // 燃烧持续伤害间隔
    private int burnedDamage; // 燃烧伤害

    private float frozenTime = 3f; // 冰冻时间
    private float frozenTimer = 0; // 冰冻计时器
    private float frozenStrength = .5f; // 冰冻减速效果

    [SerializeField] private int health;

    private Entity entity;
    private EntityFX fx;

    private bool isDead = false; // 是否死亡

    public System.Action onHealthChange; // 事件：生命值变化

    protected virtual void Start()
    {
        health = GetMaxHealth();
        critDamage.SetValue(150); // 暴击伤害150%

        entity = GetComponent<Entity>();
        fx = GetComponent<EntityFX>();

        onHealthChange?.Invoke();
    }

    protected virtual void Update()
    {
        HandleBurnEffect();
        HandleFrozenEffect();
        HandleElectrifiedEffect();
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealth(_damage);
        entity.DamageImpact();
        fx.StartCoroutine("FlashFX");

        //Debug.Log("Damage: " + _damage);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void DoDamage(CharacterStats _targetStats)
    {
        if (canAvoidAttack(_targetStats) || isDead) return; // 闪避或死亡

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (canCritAttack())
        {
            totalDamage = CalcCritDamage(totalDamage);
        }

        totalDamage = Mathf.Max(0, totalDamage - _targetStats.armor.GetValue()); // 物理防御
        //totalDamage *= (1 - _targetStats.vitality.GetValue() / 100f; // 物理抵抗

        _targetStats.TakeDamage(totalDamage);
        //DoDamageOfMagic(_targetStats); // 魔法伤害
    }

    public virtual void Die()
    {
        entity.Die();
        isDead = true;
    }

    #region Magic Damage & Ailments
    public virtual void DoDamageOfMagic(CharacterStats _targetStats)
    {
        if (isDead) return; // 死亡

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
            _targetStats.SetupBurnedDamage(Mathf.Max(1, Mathf.RoundToInt(_fireDamage * .2f))); // 设置燃烧伤害，最低1点
        }

        _targetStats.ApplyAliments(_fireDamage > 0, _iceDamage > 0, _lightningDamage > 0);
    }

    public void ApplyAliments(bool _isBurned, bool _isFrozen, bool _isElectrified)
    {
        // 火焰效果：燃烧敌人
        isBurned = _isBurned;
        if (isBurned)
        {
            burnedTimer = burnedTime;
            fx.BurnedFxFor(burnedTime, burnedDamageCooldown); // 燃烧特效
        }

        // 冰冻效果：减速敌人
        isFrozen = _isFrozen;
        if (isFrozen)
        {
            frozenTimer = frozenTime;
            fx.FrozenFxFor(frozenTime); // 冰冻特效
            entity.SlowSpeedBy(frozenStrength, frozenTimer); // 冰冻减速
        }

        // 电击效果：打断敌人
        isElectrified = _isElectrified;
        if (isElectrified)
        {
            fx.ElectrifiedFxFor(.3f); // 电击特效
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
                isBurned = false; // 燃烧死亡
            }
        }
    }

    public void SetupBurnedDamage(int _damage) => burnedDamage = _damage;
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

    protected virtual void DecreaseHealth(int _amount)
    {
        health -= _amount;

        onHealthChange?.Invoke();
    }

    private bool canAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
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
