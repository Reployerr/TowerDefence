using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyWorth))]
public class EnemyEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EnemyWorth enemy = (EnemyWorth)target;


		if (enemy.worthIsFixed == false)
		{
			enemy.minWorth = (int)EditorGUILayout.Slider("Minimum worth ", enemy.minWorth, 1f, 50f);
			enemy.maxWorth = (int)EditorGUILayout.Slider("Maximum worth ", enemy.maxWorth, 1f, 50f);

		}
		else
		{
			enemy.minWorth = (int)EditorGUILayout.Slider("Minimum worth ", enemy.minWorth, enemy.enemyWorth, enemy.enemyWorth);
			enemy.maxWorth = (int)EditorGUILayout.Slider("Maximum worth ", enemy.maxWorth, enemy.enemyWorth, enemy.enemyWorth);
			enemy.enemyWorth = 13;
		}

		if (enemy.worthIsFixed == false && enemy.minWorth >= enemy.maxWorth)
		{
			enemy.minWorth = enemy.minWorth / 2;
		}

		
		
	}
	
}
