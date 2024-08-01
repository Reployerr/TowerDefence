using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWorth : MonoBehaviour
{
    [Header("Enemy Worth")]
    public int enemyWorth;
    [SerializeField] public float minWorth = 1;
    [SerializeField] public float maxWorth = 50;
    [SerializeField] public bool worthIsFixed = false;

	private void Awake()
	{
		InitializeWorth();
	}

	private void InitializeWorth()
	{
		if (worthIsFixed == false)
		{
			enemyWorth = (int)Random.Range(minWorth, maxWorth);
		}
	}
}
