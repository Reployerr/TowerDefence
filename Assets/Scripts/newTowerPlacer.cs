using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask groundLayerMask; // Маска для определения земли
    public LayerMask roadLayerMask; // Маска для определения дороги
    public LayerMask towerLayerMask; // Маска для определения слоя башен
    public Button[] towerButtons; // Массив кнопок для выбора башен
    public GameObject[] towerPrefabs; // Массив префабов башен

    private GameObject selectedTowerPrefab; // Выбранный префаб башни
    private GameObject currentTower; // Текущая размещенная башня
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        // Назначаем каждой кнопке обработчик нажатия
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i; // Локальная копия переменной i для обработчика
            towerButtons[i].onClick.AddListener(() => SelectTower(index));
        }
    }

    private void Update()
    {
        if (currentTower != null)
        {
            // Перемещаем текущую башню за курсором мыши
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentTower.transform.position = mousePosition;

            // При нажатии левой кнопки мыши размещаем башню
            if (Input.GetMouseButtonDown(0))
            {
                if (IsMouseOverGround(mousePosition) && !IsMouseOverRoad(mousePosition) && !IsTowerOverlap(currentTower))
                {
                    PlaceTower(mousePosition);
                }
                else
                {
                    Debug.Log("Cannot place tower at this position.");
                }
            }
        }
    }

    // Метод для выбора башни через кнопку UI
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            selectedTowerPrefab = towerPrefabs[towerIndex];
            // Создаем новую башню, чтобы она следовала за курсором
            if (currentTower != null)
            {
                Destroy(currentTower);
            }
            currentTower = Instantiate(selectedTowerPrefab);
        }
    }

    // Метод для размещения башни на земле
    private void PlaceTower(Vector3 position)
    {
        if (selectedTowerPrefab != null)
        {
            Instantiate(selectedTowerPrefab, position, Quaternion.identity);
            Debug.Log("Tower placed at position: " + position);
        }
    }

    // Метод для проверки, находится ли указанная позиция над землей
    private bool IsMouseOverGround(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, groundLayerMask);
        return hit.collider != null;
    }

    // Метод для проверки, находится ли указанная позиция на дороге
    private bool IsMouseOverRoad(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, roadLayerMask);
        return hit.collider != null;
    }

    // Метод для проверки пересечения с другими башнями
    private bool IsTowerOverlap(GameObject tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 0.5f, towerLayerMask);
        return colliders.Length > 1; // Проверяем, есть ли коллайдеры, кроме самой башни
    }
}