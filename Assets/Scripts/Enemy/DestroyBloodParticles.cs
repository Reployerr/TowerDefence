using UnityEngine;

public class DestroyBloodParticles : MonoBehaviour
{
	[SerializeField] private float timeLeft = 1f;
	private void Update()
	{
		timeLeft -= Time.deltaTime;
		if(timeLeft <= 0f)
		{
			Destroy(this.gameObject);
		}
	}
}
