using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int EnemyHealth { get; protected set; } = 10;
    public float EnemyMoveSpeed { get; protected set; }

    private int pathIndex = 0;

    public Rigidbody2D EnemyRigidBody { get; protected set; } = null;
    public Transform targetPosition;

    public Enemy(float moveSpeed, int health) 
    {
        EnemyMoveSpeed = moveSpeed;
        EnemyHealth = health;
    }

    public Enemy(Rigidbody2D rb)
    {
        EnemyRigidBody = rb;
    }

    // TAKING DAMAGE
    public abstract void TakeDamage(int damage);
    public virtual void KillEnemy()
	{
        Debug.Log("enemy died");
        Destroy(this.gameObject);
    }

    public void GetTargetPos() //to start
    {
        targetPosition = LevelManager.main.path[pathIndex];
        Debug.Log("keke");
    }

    public void GetDistance()//to update
    {

        if (Vector2.Distance(targetPosition.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            
            if (pathIndex == LevelManager.main.path.Length)
            {
                Destroy(gameObject);
                return;
            }

            else
            {
                targetPosition = LevelManager.main.path[pathIndex];
            }
        }
    }

    public virtual void Move() //to fixedupdate
    {
        Vector2 direction = (targetPosition.position - transform.position).normalized;
        EnemyRigidBody.velocity = direction * EnemyMoveSpeed;
    }



}
