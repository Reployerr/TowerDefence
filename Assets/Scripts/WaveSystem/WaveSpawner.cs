using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject[] enemyPrefabs;
	[SerializeField] private TMP_Text wavesText;

	[Header("Attributes")]
	[SerializeField] private int _baseEnemies = 3; // count of enemies by default
	[SerializeField] private float _enemiesPerSecond = 0.5f;
	[SerializeField] private float _timeBetweenWaves = 5f; // time before new wave starts
	[SerializeField] private float difficultyScalingFactor = 0.75f;
	[SerializeField] private float enemiesPerSecondCap = 15f;

	[Header("Events")]
	public static UnityEvent onEnemyDestroy = new UnityEvent();

	public int _currentWave = 1;
	public int _maxWaves = 6;

	private float _timeSinceLastSpawn; // time after last enemy's wave has spawned
	private int _enemiesAlive;
	private int _enemiesLeftToSpawn; // осталось заспавнить
	private bool isSpawning;
	private float eps; //enemies per second

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

		if(_timeSinceLastSpawn >= (1f / eps) && _enemiesLeftToSpawn > 0)
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
		wavesText.text = "ВОЛНА " + _currentWave + "/" + _maxWaves;
		isSpawning = true;
		_enemiesLeftToSpawn = EnemiesPerWave();
		eps = EnemiesPerSecond();
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
		
		if(_currentWave < 4)
		{
			GameObject prefabToSpawn = enemyPrefabs[0];
			Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
		}

		if(_currentWave == 4)
		{
			/*GameObject prefabToSpawnImp = enemyPrefabs[1];

			for (int i = 0; i < _baseEnemies; i++)
			{
				
				Instantiate(prefabToSpawnImp, LevelManager.main.startPoint.position, Quaternion.identity);
			}*/
			GameObject prefabToSpawn = enemyPrefabs[1];
			Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
		}

		if(_currentWave > 4)
		{
			int index = Random.Range(0, enemyPrefabs.Length);
			GameObject prefabToSpawn = enemyPrefabs[index];
			Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
		}
	}

	private int EnemiesPerWave()
	{
		return Mathf.RoundToInt(_baseEnemies * Mathf.Pow(_currentWave, difficultyScalingFactor)); // умножаем базовое количество врагов на текущую волну возведя  в степень показателя сложности
	}

	private float EnemiesPerSecond()
	{
		return Mathf.Clamp(_enemiesPerSecond * Mathf.Pow(_currentWave, difficultyScalingFactor), 0, enemiesPerSecondCap); // умножаем базовое количество врагов на текущую волну возведя  в степень показателя сложности
	}
}
