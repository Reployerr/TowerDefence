using UnityEngine;

public class Player : UI_Health
{
    [Header("Variables")]
    [SerializeField] private int _playerHealth = 10;
    [SerializeField] private int _playerMoney;

    public void PlayerTakeDamage(int damage)
    {
        if(_playerHealth > 0)
		{
            _playerHealth -= damage;
            Debug.Log($"Player received {damage} damage");
            DecreaseHealth(_playerHealth);
        }
		else
		{
            Debug.Log("Player died");
		}

    }
}
