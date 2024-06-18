using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
	[Header("Adressables")]
	[SerializeField] private TMP_Text _goldText;

	public void DecreaseGold(int curGold)
	{
		_goldText.text = curGold.ToString();
	}

	public void InitializeGold(int gold)
	{
		_goldText.text = gold.ToString();
	}
}
