using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{

	[Header("Adressables")]
	[SerializeField] private TMP_Text _healthText;
	[SerializeField] private GameObject deathPanel;

	private bool deathPanelIsShow = false;

	public void DecreaseHealth(int health)
	{
		_healthText.text = health.ToString();
	}

	public void InitializeHealth(int health)
	{
		_healthText.text = health.ToString();
	}

	public void DeathPanelUpdateState()
	{
		deathPanelIsShow = true;

		if (deathPanelIsShow)
		{
			deathPanel.SetActive(true);
		}
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1.0f;
	}
}
