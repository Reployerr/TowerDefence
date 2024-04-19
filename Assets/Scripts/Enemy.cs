using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health{ get; protected set; } = 10;
    public float moveSpeed { get; protected set; } = 10;

    public static Transform targetPosition;
    private int pathIndex = 0;

    private void Start()
    {
        targetPosition = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        if(Vector2.Distance(targetPosition.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if(pathIndex == LevelManager.main.path.Length)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

}
