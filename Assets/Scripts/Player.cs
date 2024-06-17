using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int _playerHealth = 10;
    [SerializeField] private int _playerMoney = 100;

    [SerializeField] private GameObject playerUI;
    UI_Health playerHealthUI;
    UI_Gold playerGoldUI;

	private void Start()
	{
        UI_Health health = playerUI.GetComponent<UI_Health>();
        playerHealthUI = health;
    }

	public void PlayerTakeDamage(int damage)
    {
        if(_playerHealth > 0)
		{
            _playerHealth -= damage;
            Debug.Log($"Player received {damage} damage");
            playerHealthUI.DecreaseHealth(_playerHealth);
        }
		else
		{
            Debug.Log("Player died");
		}

    }
}
