using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemy
{
    [SerializeField] private float trollSpeed = 1f;
    [SerializeField] private int trollHealth = 5;
    private Rigidbody2D trollRigidbody;

    public Troll(float moveSpeed, int health) : base(moveSpeed, health)
    {
    }

    public Troll(Rigidbody2D rb) : base(rb)
    {
    }

    private void Awake()
    {
        trollRigidbody = GetComponent<Rigidbody2D>();
        EnemyRigidBody = trollRigidbody;
    }
    private void Start()
    {
        GetTargetPos();
    }

    private void Update()
    {
        GetDistance();
        EnemyMoveSpeed = trollSpeed;
        EnemyHealth = trollHealth;
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }


    public void MoveEnemy()
    {
        base.Move();
    }
}
