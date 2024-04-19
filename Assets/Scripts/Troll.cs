using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemy
{
    private float trollSpeed;
    
  [SerializeField] private Rigidbody2D rb;

    Transform target = Enemy.targetPosition;

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * trollSpeed;
    }
}
