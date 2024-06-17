using UnityEngine;
using TMPro;

public class UI_Health : MonoBehaviour
{

	[Header("Adressables")]
	[SerializeField] private TMP_Text _healthText;

	private void Start()
	{
		_healthText.text = 10.ToString();
	}

	public void DecreaseHealth(int health)
	{
		_healthText.text = health.ToString();
		_healthText.text = _healthText.text;
	}
}
