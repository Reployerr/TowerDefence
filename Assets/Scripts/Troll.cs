using UnityEngine;

public class Troll : Enemy
{
    [Header("Variables")]
    [SerializeField] private float trollSpeed = 1f;
    [SerializeField] private int trollHealth = 6;
    [SerializeField] private int trollDamage = 2;

    [Header("Enemy Worth")]
    [SerializeField] public int trollWorth;
    [SerializeField] public float minWorth = 1;
    [SerializeField] public float maxWorth = 50;
    [SerializeField] public bool worthIsFixed = false;

    [Header("References")]
    private Rigidbody2D trollRigidbody;

    public Troll(float moveSpeed, int health, int damage, int worth) : base(moveSpeed, health, damage, worth) { }
    public Troll(Rigidbody2D rb) : base(rb) { }

    private void Awake()
    {
        trollWorth = Random.Range(20, 20);
        trollRigidbody = GetComponent<Rigidbody2D>();
        EnemyRigidBody = trollRigidbody;
        EnemyMoveSpeed = trollSpeed;
        EnemyHealth = trollHealth;
        EnemyDamage = trollDamage;

        EnemyWorth = trollWorth;
        Debug.Log($"Troll initialized with damage: {EnemyDamage}");
    }

    private void Start()
    {
		if (worthIsFixed == false)
		{
            trollWorth = (int)Random.Range(minWorth, maxWorth);
        }

        GetTargetPos();
        FindingPlayer();
    }

    private void Update()
    {
        GetDistance();

        if (EnemyHealth < 0)
        {
            EnemyHealth = 0;
        }
        if (EnemyHealth == 0)
        {
            KillEnemy();
            GivingWorth(EnemyWorth);
            Debug.Log($"Enemy died and give a {EnemyWorth} coins");
        }
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    public void MoveEnemy()
    {
        base.Move();
    }

    public override void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
    }


}
