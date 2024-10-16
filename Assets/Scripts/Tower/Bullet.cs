using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private TowerShooting towerScript;
	[SerializeField] private GameObject parentTower;

	[Header("Attributes")]
	[SerializeField] private int _bulletDamage = 2;
	[SerializeField] private float _bulletLifetime = 2f;
	[SerializeField] private float trajectoryMaxHeight;

	private float maxMoveSpeed;
	private float _bulletSpeed;

	private AnimationCurve trajectoryAnimationCurve;
	private AnimationCurve axisCorrectionAnimationCurve;
	private AnimationCurve bulletSpeedAnimationCurve;
	private Transform _enemy;

	private Vector3 trajectoryStartPoint; // начальная точка тректории
	private Vector3 bulletMoveDirection;
	private void Awake()
	{
		towerScript = parentTower.GetComponent<TowerShooting>();
	}
	private void Start()
	{
		Destroy(this.gameObject, _bulletLifetime);
		trajectoryStartPoint = transform.position;
	}
	private void Update()
	{

		if (!_enemy) return;

		TowerShooting.ShootingType shootingType = towerScript.GetShootingType();

		if (shootingType == TowerShooting.ShootingType.Canon)
		{

			CanonBulletUpdatePosition();//стрельба по дуге
		}
		if(shootingType == TowerShooting.ShootingType.Default)
		{
			Vector2 direction = (_enemy.position - transform.position).normalized;
			rb.velocity = direction * maxMoveSpeed;
		}	
	}

	public void InitializeBullet(Transform target, float maxMoveSpeed, float trajectoryMaxHeight)
	{
		this._enemy = target;
		this.maxMoveSpeed = maxMoveSpeed;
		this.trajectoryMaxHeight = trajectoryMaxHeight;
	}

	public void InitializeAnimationCurve(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionCurve, AnimationCurve bulletSpeedAnimationCurve)
	{
		this.trajectoryAnimationCurve = trajectoryAnimationCurve;
		this.axisCorrectionAnimationCurve = axisCorrectionCurve;
		this.bulletSpeedAnimationCurve = bulletSpeedAnimationCurve;
	}

	private void FixedUpdate()
	{
		
	}

	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.TryGetComponent(out Enemy enemy))
		{
			enemy.TakeDamage(_bulletDamage);
			//Debug.Log("enemy hitted");
			Destroy(gameObject);
		}	
	}

	private void CanonBulletUpdatePosition()
	{
		if(_enemy != null)
		{
			Vector3 trajectoryRange = _enemy.position - trajectoryStartPoint;

			if (trajectoryRange.x < 0)
			{
				_bulletSpeed = -_bulletSpeed;
			}

			float nextPositionX = transform.position.x + _bulletSpeed * Time.deltaTime;
			float nextPositionXNormalized = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;

			float nextPositionYNormalized = trajectoryAnimationCurve.Evaluate(nextPositionXNormalized);

			float nextPositionYCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPositionXNormalized);
			float nextPositionCorrectionAbsolute = nextPositionYCorrectionNormalized * trajectoryRange.y;

			float nextPositionY = trajectoryStartPoint.y + nextPositionYNormalized * trajectoryMaxHeight + nextPositionCorrectionAbsolute;

			Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);

			CalculateNextBulletSpeed(nextPositionXNormalized);
			bulletMoveDirection = newPosition - transform.position;

			transform.position = newPosition;
		}
		
		else
		{
			Debug.Log("enemy null");
		}

	}

	private void CalculateNextBulletSpeed(float nextPositionXNormalized)
	{
		float nextMoveSpeedNormalized = bulletSpeedAnimationCurve.Evaluate(nextPositionXNormalized);

		_bulletSpeed = nextMoveSpeedNormalized * maxMoveSpeed;
	}

	public Vector3 GetBulletMoveDirection()
	{
		return bulletMoveDirection;
	}

}
