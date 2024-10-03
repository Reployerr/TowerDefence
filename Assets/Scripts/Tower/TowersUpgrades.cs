
using System.Collections.Generic;
using UnityEngine;




public class TowersUpgrades : MonoBehaviour
{
	public int UpgradeCost;
	public enum TypesOfNextUpgrades
	{
		none = 0,
		ArcherLVL2, ArcherFire, ArcherPoison, 
		CanonLVL2, CanonFire, CanonIce, 
		MageLVL2, MageFire, MageIce
	}

}
