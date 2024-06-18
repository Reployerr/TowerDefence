using UnityEngine;
using TMPro;

public class UI_Health : MonoBehaviour
{

	[Header("Adressables")]
	[SerializeField] private TMP_Text _healthText;

	public void DecreaseHealth(int health)
	{
		_healthText.text = health.ToString();
	}

	public void InitializeHealth(int health)
	{
		_healthText.text = health.ToString();
	}

}
