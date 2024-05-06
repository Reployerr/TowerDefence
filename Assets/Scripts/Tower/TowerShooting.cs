using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TowerShooting : MonoBehaviour
{
    private Transform _startingRotation;

    [Header("References")]
    [SerializeField] private Transform _towerRotationPoint; // точка вращения оружия башни (если оно есть)
    [SerializeField] private LayerMask _enemyMask; // маска врагов
    [SerializeField] private GameObject _towerBullet; // снаряд которой стреляет башня
    [SerializeField] private Transform _shootPoint; // точка откуда стреляет башня
    

    [Header("Attributes")]
    [SerializeField] private float _attackRange = 0f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _firingRate = 1f;

    private Transform _target = null;
    private float _timeUntilFire;

    private void Awake()
    {
        Quaternion startRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    private void Update()
    {
        if(_target == null)
        {
            FindTarget();
            return;
        }
        RotateToTarget();

        if (!CheckTargetInRange())
        {
            _target = null;
        }
		else
		{
            _timeUntilFire += Time.deltaTime;

            if(_timeUntilFire >= 1f / _firingRate)
			{
                ShootEnemy();
                _timeUntilFire = 0f;
			}
        }

    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _attackRange, (Vector2)transform.position, 0f, _enemyMask); //CircleCastAll
        if (hits.Length > 0)
        {
            _target = hits[0].transform;
        }
        if(hits.Length <= 0)
        {
            Quaternion defaultRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            _towerRotationPoint.rotation = Quaternion.RotateTowards(_towerRotationPoint.rotation, defaultRotation, 200f /*скорость поворота*/ * Time.deltaTime);
        }
        
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(_target.position, transform.position) <= _attackRange;
    }

    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f; // ATAN

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        _towerRotationPoint.rotation = Quaternion.RotateTowards(_towerRotationPoint.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    public void ShootEnemy()
	{
        GameObject bulletObj = Instantiate(_towerBullet, _shootPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(_target);

    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, _attackRange);
    }
}
