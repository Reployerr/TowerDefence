using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Troll))]
public class EnemyEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		Troll enemy = (Troll)target;


		if (enemy.worthIsFixed == false)
		{
			enemy.minWorth = (int)EditorGUILayout.Slider("Minimum worth ", enemy.minWorth, 1f, 50f);
			enemy.maxWorth = (int)EditorGUILayout.Slider("Maximum worth ", enemy.maxWorth, 1f, 50f);
		}
		else
		{
			enemy.minWorth = (int)EditorGUILayout.Slider("Minimum worth ", enemy.minWorth, enemy.trollWorth, enemy.trollWorth);
			enemy.maxWorth = (int)EditorGUILayout.Slider("Maximum worth ", enemy.maxWorth, enemy.trollWorth, enemy.trollWorth);
			enemy.trollWorth = 13;
		}

		if (enemy.worthIsFixed == false && enemy.minWorth >= enemy.maxWorth)
		{
			enemy.minWorth = enemy.minWorth / 2;
		}

		
		
	}
}
