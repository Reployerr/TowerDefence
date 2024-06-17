using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int EnemyHealth { get; protected set; } = 10;
    public float EnemyMoveSpeed { get; protected set; }
    public int EnemyDamage { get; protected set; } = 1;

    private int pathIndex = 0;
    protected Player player; // —сылка на игрока

    public Rigidbody2D EnemyRigidBody { get; protected set; } = null;
    public Transform targetPosition;

   

    public Enemy(float moveSpeed, int health, int damage)
    {
        EnemyMoveSpeed = moveSpeed;
        EnemyHealth = health;
        EnemyDamage = damage;
    }

    public Enemy(Rigidbody2D rb)
    {
        EnemyRigidBody = rb;
    }

    public void FindingPlayer()
    {
        // Finding Player by tag
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();

            if (player == null)
            {
                Debug.LogError("Player is not found");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    // TAKING DAMAGE
    public abstract void TakeDamage(int damage);

    public virtual void KillEnemy()
    {
        Debug.Log("Enemy died");
        WaveSpawner.onEnemyDestroy.Invoke();
        Destroy(this.gameObject);
    }

    public void GetTargetPos() // to start
    {
        targetPosition = LevelManager.main.path[pathIndex];
    }

    public void GetDistance() // to update
    {
        if (Vector2.Distance(targetPosition.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                // Deal damage to player
                if (player != null)
                {
                    Debug.Log($"Enemy reached end and is dealing {EnemyDamage} damage to player.");
                    player.PlayerTakeDamage(EnemyDamage);
                }
                else
                {
                    Debug.LogError("Player reference is null when trying to deal damage!");
                }

                WaveSpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                targetPosition = LevelManager.main.path[pathIndex];
            }
        }
    }

    public virtual void Move() // to fixedupdate
    {
        Vector2 direction = (targetPosition.position - transform.position).normalized;
        EnemyRigidBody.velocity = direction * EnemyMoveSpeed;
    }
}
