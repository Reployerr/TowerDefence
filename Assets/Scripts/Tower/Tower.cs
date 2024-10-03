using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tower : MonoBehaviour
{
	public string towerName;
	public int cost;
	public GameObject prefab;

	public List<GameObject> NextUpdates;
}
