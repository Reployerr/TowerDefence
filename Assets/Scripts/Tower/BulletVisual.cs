using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVisual : MonoBehaviour
{
	[SerializeField] private Transform bulletVisual;
	[SerializeField] private Bullet bullet;

	private void Update()
	{
		UpdateBulletRotation();
	}

	private void UpdateBulletRotation()
	{
		Vector3 bulletMoveDir = bullet.GetBulletMoveDirection();

		bulletVisual.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(bulletMoveDir.y, bulletMoveDir.x) * Mathf.Rad2Deg);
	}
}
