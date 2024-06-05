using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Health : MonoBehaviour
{
	[Header("Adressables")]
	[SerializeField] private TMP_Text _healthText;

	Player _playerScript;

	private void Start()
	{
		_healthText.text = 10.ToString();
	}

	public void DecreaseHealth(int damage)
	{
		_healthText.text = damage.ToString();
	}
}
