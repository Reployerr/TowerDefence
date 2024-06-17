using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemy
{
    [Header("Variables")]
    [SerializeField] private float trollSpeed = 1f;
    [SerializeField] private int trollHealth = 6;
    [SerializeField] private int trollDamage = 2;

    [Header("References")]
    private Rigidbody2D trollRigidbody;

    public Troll(float moveSpeed, int health, int damage) : base(moveSpeed, health, damage) { }
    public Troll(Rigidbody2D rb) : base(rb) { }

    private void Awake()
    {
        trollRigidbody = GetComponent<Rigidbody2D>();
        EnemyRigidBody = trollRigidbody;
        EnemyMoveSpeed = trollSpeed;
        EnemyHealth = trollHealth;
        EnemyDamage = trollDamage;

        Debug.Log($"Troll initialized with damage: {EnemyDamage}");
    }

    private void Start()
    {
        GetTargetPos();
        FindingPlayer(); // »щем игрока
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
