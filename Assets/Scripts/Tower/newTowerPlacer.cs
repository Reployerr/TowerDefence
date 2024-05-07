using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayerMask; // Маска для определения земли
    [SerializeField] private LayerMask roadLayerMask; // Маска для определения дороги
    [SerializeField] private LayerMask towerLayerMask; // Маска для определения слоя башен
    [SerializeField] private Button[] towerButtons; // Массив кнопок для выбора башен
    [SerializeField] private GameObject[] towerPrefabs; // Массив префабов башен

    private GameObject _selectedTowerPrefab; // Выбранный префаб башни
    private GameObject _currentTower; // Текущая размещенная башня
    private Camera _mainCamera;
    private SpriteRenderer _prefabSprite;

    private void Awake()
    {
        _mainCamera = Camera.main;

        // каждой кнопке обработчик нажатия
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i; // Локальная копия переменной i для обработчика
            towerButtons[i].onClick.AddListener(() => SelectTower(index));
        }
    }

    private void Update()
    {
        if (_currentTower != null)
        {
            // Перемещаем выбранную башню за курсором мыши
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _currentTower.transform.position = mousePosition;

            // цвет башни в зависимости от типа поверхности
            if (IsMouseOverRoad(mousePosition))
            {
               //Debug.Log("Mouse over road");
                _prefabSprite.color = Color.red;
                _prefabSprite.color = new Color(_prefabSprite.color.r, _prefabSprite.color.g, _prefabSprite.color.b, 0.5f);
            }
            else if (IsMouseOverGround(mousePosition))
            {
                //Debug.Log("Mouse over ground");
                _prefabSprite.color = Color.green;
                _prefabSprite.color = new Color(_prefabSprite.color.r, _prefabSprite.color.g, _prefabSprite.color.b, 0.5f);
            }

            if (IsTowerOverlap(_currentTower))
			{
                _prefabSprite.color = Color.red;
                _prefabSprite.color = new Color(_prefabSprite.color.r, _prefabSprite.color.g, _prefabSprite.color.b, 0.5f);
            }

            // размещение башни
            if (Input.GetMouseButtonDown(0))
            {
                if (IsMouseOverGround(mousePosition) && !IsMouseOverRoad(mousePosition) && !IsTowerOverlap(_currentTower))
                {
                    PlaceTower(mousePosition);
                }
                else
                {
                    Debug.Log("Нельзя разместить");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                //убираем башню из состояния выбора
                _selectedTowerPrefab = null;
                if (_currentTower != null)
                {
                    Destroy(_currentTower);
                    _currentTower = null;
                }
            }
        }
    }

    // Метод для выбора башни через кнопку UI
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            _selectedTowerPrefab = towerPrefabs[towerIndex];

            // Создаем новую башню, чтобы она следовала за курсором
            if (_currentTower != null)
            {
                Destroy(_currentTower);
            }
            _currentTower = Instantiate(_selectedTowerPrefab);

            //получаем спрайт выбранной башни
            _prefabSprite = _currentTower.GetComponent<SpriteRenderer>();
            
            // Отключаем скрипт TowerShooting
            TowerShooting towerShooting = _currentTower.GetComponent<TowerShooting>();
            if (towerShooting != null)
            {
                towerShooting.enabled = false;
            }
        }
    }

    // Метод для размещения башни на земле
    private void PlaceTower(Vector3 position)
    {
        if (_selectedTowerPrefab != null)
        {
            Instantiate(_selectedTowerPrefab, position, Quaternion.identity);
            Debug.Log("Башня расположена: " + position);

            // Убираем башню из состояния выбора
            _selectedTowerPrefab = null;
            if (_currentTower != null)
            {
                Destroy(_currentTower);
                _currentTower = null;
            }
        }
    }

	#region Рейкасты на определение типа поверхности
	//находится ли указанная позиция на земле
	private bool IsMouseOverGround(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, groundLayerMask);
        return hit.collider != null;
    }

    //находится ли указанная позиция на дороге
    private bool IsMouseOverRoad(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, roadLayerMask);
        return hit.collider != null;
    }

    // пересечение с другими башнями
    private bool IsTowerOverlap(GameObject tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 1f, towerLayerMask);
        return colliders.Length > 1; //есть ли коллайдеры, кроме самой башни
    }
	#endregion
}
