
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]

public class TowersUpgrades : MonoBehaviour
{
	public int UpgradeCost;
	public TypesOfNextUpgrades upgradeType;
	

}
public enum TypesOfNextUpgrades
{
	none = 0,
	ArcherLVL2, ArcherFire, ArcherPoison,
	CanonLVL2, CanonFire, CanonIce,
	MageLVL2, MageFire, MageIce
}
