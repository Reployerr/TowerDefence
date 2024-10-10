using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlacementMode
{
    Fixed,
    Valid,
    Invalid
}

public class TowerManager : MonoBehaviour
{
    /*[SerializeField] private int _obstacles;
    [HideInInspector] public bool hasValidPlacement;
    [HideInInspector] public bool isFixed;
    private bool isInTrigger = false; // Флаг для отслеживания нахождения в триггере

    private void Awake()
    {
        hasValidPlacement = true;
        isFixed = true;
        _obstacles = 0;
    }
    private void Update()
    {
        Debug.Log(_obstacles);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFixed) return;

        // ignore ground objects
        if (_IsGround(other.gameObject)) return;
        if (other.CompareTag("Ground"))
        {
            Debug.Log("Ground TRIGGERED");
        }
        if (other.gameObject.CompareTag("Env") && !isInTrigger) // Проверяем, что объект еще не находится в триггере
        {
            _obstacles++;
            SetPlacementMode(PlacementMode.Invalid);
            isInTrigger = true; // Устанавливаем флаг в true, теперь объект находится в триггере
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isFixed) return;

        // ignore ground objects
        if (_IsGround(other.gameObject)) return;
        if (other.CompareTag("Ground"))
        {
            Debug.Log("Ground TRIGGERED");
        }
        if (other.gameObject.CompareTag("Env") && isInTrigger) // Проверяем, что объект уже находится в триггере
        {
            _obstacles--;
            if (_obstacles == 0)
            {
                SetPlacementMode(PlacementMode.Valid);
            }
            isInTrigger = false; // Устанавливаем флаг в false, теперь объект вышел из триггера
        }
    }

    public void SetPlacementMode(PlacementMode mode)
    {
        if (mode == PlacementMode.Fixed)
        {
            isFixed = true;
            hasValidPlacement = true;
        }
        else if (mode == PlacementMode.Valid)
        {
            hasValidPlacement = true;
        }
        else
        {
            hasValidPlacement = false;
        }
    }

   /* private bool _IsGround(GameObject o) // маска groundLayerMask из класса TowerPlacer
    {
        return ((1 << o.layer) & TowerPlacer.instance.groundLayerMask.value) != 0;
    }*/
}