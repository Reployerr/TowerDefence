using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
	[Header("Adressables")]
	[SerializeField] private TMP_Text _goldText;

	private void Start()
	{
		_goldText.text = 100.ToString();
	}
}
