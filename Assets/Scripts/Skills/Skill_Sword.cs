using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Skill_Sword : Skill
{
    public SwordType type = SwordType.Regular;

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity = 4;
    [SerializeField] private float freezeTimeDuration = 1;
    [SerializeField] private float returnSpeed = 12;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount = 4;
    [SerializeField] private float bounceGravity = 4;
    [SerializeField] private float bounceSpeed = 20;

    [Header("Pierce Info")]
    [SerializeField] private int pierceAmount = 2;
    [SerializeField] private float pierceGravity = 1;

    [Header("Spin Info")]
    [SerializeField] private float maxTravelDistance = 8;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float hitCooldown = .5f;

    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravite();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Mouse1)) // 鼠标右键
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1)) // 鼠标右键弹起
        {
            Vector2 dir = AimDirction();
            finalDir = new Vector2(dir.x * launchForce.x, dir.y * launchForce.y);

            DotsActive(false);
        }
    }

    private void SetupGravite()
    {
        switch (type)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Skill_Sword_Controller newSwordScript = newSword.GetComponent<Skill_Sword_Controller>();

        switch(type)
        {
            case SwordType.Bounce:
                newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetuPierce(pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
                break;
        }

        SetupGravite();

        player.AssignNewSword(newSword);

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);
    }

    #region Aim
    public Vector2 AimDirction()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirction = mousePosition - playerPosition;

        return dirction.normalized;
    }

    public void DotsActive(bool active)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].SetActive(active);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false); // 禁用/使其隐藏
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 dir = AimDirction();
        Vector2 position = (Vector2)player.transform.position + new Vector2(dir.x * launchForce.x, dir.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t); // D = vt + 1/2at^2 匀加速直线运动的位移公式

        return position;
    }
    #endregion
}
