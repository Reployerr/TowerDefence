using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Rigidbody2D rb;

	[Header("Attributes")]
	[SerializeField] private float _bulletSpeed = 2f;
	[SerializeField] private int _bulletDamage = 2;
	[SerializeField] private float _bulletLifetime = 2f;

	private Transform _enemy;
	//private GameObject _enemyObj;

	private void Start()
	{
		Destroy(this.gameObject, _bulletLifetime);
		
	}
	public void SetTarget(Transform target)
	{
		_enemy = target;
	}

	private void FixedUpdate()
	{
		if (!_enemy) return;

		Vector2 direction = (_enemy.position - transform.position).normalized;

		rb.velocity = direction * _bulletSpeed;
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

}
