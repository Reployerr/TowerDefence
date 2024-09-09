using System.Collections;
using UnityEngine;

public class Troll : Enemy
{
    [Header("Variables")]
    [SerializeField] private float trollSpeed = 1f;
    [SerializeField] private int trollHealth = 6;
    [SerializeField] private int trollDamage = 2;
    private int trollWorth;

    [Header("Enemy Worth")]
   // [SerializeField] public int trollWorth;
    [SerializeField] private EnemyWorth worthScript; // его необходимо перетащить в инспекторе именно с объекта врага, не оригинал

    [Header("References")]
    private Rigidbody2D trollRigidbody;
    [SerializeField] private GameObject bloodEffect;

    public Troll(float moveSpeed, int health, int damage, int worth) : base(moveSpeed, health, damage, worth) { }
    public Troll(Rigidbody2D rb) : base(rb) { }

    private void Awake()
    {
        trollRigidbody = GetComponent<Rigidbody2D>();
        EnemyWorth enemyWorthScript = GetComponent<EnemyWorth>();
        worthScript = enemyWorthScript;
        GetWorthValue(worthScript);

    }

    private void Start()
    {
        EnemyWorth = worthScript.enemyWorth;
        EnemyRigidBody = trollRigidbody;
        EnemyMoveSpeed = trollSpeed;
        EnemyHealth = trollHealth;
        EnemyDamage = trollDamage;

        Debug.Log($"Troll initialized with damage: {EnemyDamage}");
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
        Instantiate(bloodEffect, transform.position, Quaternion.identity);

    }

    public override void GetWorthValue(EnemyWorth worth)
	{
        trollWorth = worth.enemyWorth;   
    }

}
