using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{

    public UnityEvent playerDeath;
    

    [Header("Variables")]
    public int _playerMoney;
    public int _playerHealth = 12;

    [Header("References")]
    [SerializeField] private GameObject playerUI;
    [SerializeField] private UI_Health playerHealthUI;
    [SerializeField] private UI_Gold playerGoldUI;

	private void Start()
	{
        playerDeath.AddListener(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PlayerDeath);

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
        if (_playerHealth <= 0)
        {
            Debug.Log("Player dead");
            playerDeath.Invoke();

        }

    }

    public void PlayerDeath()
	{
        playerHealthUI.DeathPanelUpdateState();
        Time.timeScale = 0f;
        AudioListener.pause = true;

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
