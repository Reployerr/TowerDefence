using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int _playerHealth;
    [SerializeField] public int _playerMoney;

    [Header("References")]
    [SerializeField] private GameObject playerUI;
    [SerializeField] private UI_Health playerHealthUI;
    [SerializeField] private UI_Gold playerGoldUI;

	private void Start()
	{
        UI_Health health = playerUI.GetComponent<UI_Health>();
        playerHealthUI = health;
        playerHealthUI.InitializeHealth(_playerHealth);


        UI_Gold gold = playerUI.GetComponent<UI_Gold>();
        playerGoldUI = gold;
        playerGoldUI.InitializeGold(_playerMoney);
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

    public void BuyingTower()
	{
        playerGoldUI.DecreaseGold(_playerMoney);
    }

    public void UpgradingTower(int cost)
	{
        _playerMoney -= cost;
        playerGoldUI.DecreaseGold(_playerMoney);
    }
    public void GotWorth(int count)
	{
        _playerMoney += count;
        playerGoldUI.InitializeGold(_playerMoney);
    }
}
