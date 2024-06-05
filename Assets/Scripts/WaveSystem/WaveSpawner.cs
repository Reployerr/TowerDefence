using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject[] enemyPrefabs;

	[Header("Attributes")]
	[SerializeField] private int _baseEnemies = 3; // count of enemies by default
	[SerializeField] private float _enemiesPerSecond = 0.5f;
	[SerializeField] private float _timeBetweenWaves = 5f; // time before new wave starts
	[SerializeField] private float difficultyScalingFactor = 0.75f;

	[Header("Events")]
	public static UnityEvent onEnemyDestroy = new UnityEvent();

	private int _currentWave = 1;
	private float _timeSinceLastSpawn; // time after last enemy's wave has spawned
	private int _enemiesAlive;
	private int _enemiesLeftToSpawn; // осталось заспавнить
	private bool isSpawning;

	private void Awake()
	{
		onEnemyDestroy.AddListener(EnemyDestroy);
	}

	private void Start()
	{
		StartCoroutine(StartWave());
	}

	private void Update()
	{
		if (!isSpawning) return;

		_timeSinceLastSpawn += Time.deltaTime;

		if(_timeSinceLastSpawn >= (1f / _enemiesPerSecond) && _enemiesLeftToSpawn > 0)
		{
			SpawnEnemy();
			_enemiesLeftToSpawn--;
			_enemiesAlive++;
			_timeSinceLastSpawn = 0f;
		}

		if(_enemiesAlive == 0 && _enemiesLeftToSpawn == 0)
		{
			EndWave();
		}

	}

	private void EnemyDestroy()
	{
		_enemiesAlive--;
	}

	private IEnumerator StartWave()
	{
		yield return new WaitForSeconds(_timeBetweenWaves);

		isSpawning = true;
		_enemiesLeftToSpawn = EnemiesPerWave();
	}

	private void EndWave()
	{
		isSpawning = false;
		_timeSinceLastSpawn = 0f;
		_currentWave++;
		StartCoroutine(StartWave());
	}

	private void SpawnEnemy()
	{
		Debug.Log("Enemy spawned");
		GameObject prefabToSpawn = enemyPrefabs[0];
		Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);

	}

	private int EnemiesPerWave()
	{
		return Mathf.RoundToInt(_baseEnemies * Mathf.Pow(_currentWave, difficultyScalingFactor)); // умножаем базовое количество врагов на текущую волну возведя  в степень показателя сложности
	}
}
