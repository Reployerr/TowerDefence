using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefence.Utils;

public class Testing : MonoBehaviour
{
    private Grid grid;

    [SerializeField]  private int x = 4;
    [SerializeField]  private int y = 2;
    [SerializeField]  private float cellSize;

    private void Start()
    {
         grid = new Grid(x, y, cellSize);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(Utils.GetMouseWorldPosition(), 56);
        }
    }
}
