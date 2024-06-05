using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UI_Health
{
	[Header("Variables")]
	[SerializeField] public int BasePlayerHealth = 10;
	[SerializeField] private int _playerHealth;
	[SerializeField] public int BasePlayerMoney = 50;
	[SerializeField] private int _playerMoney;


	private void Awake()
	{
		_playerHealth = BasePlayerHealth;
		_playerMoney = BasePlayerMoney;
	}

	public void TakeDamage(int damage)
	{
		if (_playerHealth <= 0)
		{
			Debug.Log("Player Died");
			return;
		}

		else
		{
			Debug.Log("Player get damage");
			_playerHealth -= damage;
			DecreaseHealth(damage);
		}
	}
}
