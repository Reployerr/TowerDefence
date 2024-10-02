using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayerMask; // Маска для определения земли
    [SerializeField] private LayerMask roadLayerMask; // Маска для определения дороги
    [SerializeField] private LayerMask towerLayerMask; // Маска для определения слоя башен
    [SerializeField] private Button[] towerButtons; // Массив кнопок для выбора башен
    [SerializeField] private Tower[] towers; // Массив башен
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private Player _playerScript;

    private GameObject _selectedTowerPrefab; // Выбранный префаб башни
    private GameObject _currentTower; // Текущая размещенная башня
    private Camera _mainCamera;
    private SpriteRenderer _prefabSprite;
    public GameObject upgradeMenu;
    public GameObject selectedUpgradePoint;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerScript = _playerObj.GetComponent<Player>();

        // каждой кнопке обработчик нажатия
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i; // Локальная копия переменной i для обработчика
            towerButtons[i].onClick.AddListener(() => SelectTower(index));
        }
    }

    private void Update()
    {
		#region UpgradeSellMenu

		if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
			if (!EventSystem.current.IsPointerOverGameObject())
			{
                if (hit != false)
                {
                    selectedUpgradePoint = hit.transform.gameObject;
                    if (selectedUpgradePoint.tag == "Tower")
                    {
                        Debug.Log("tag is tower");
                        upgradeMenu.SetActive(true);
                        upgradeMenu.transform.position = Camera.main.WorldToScreenPoint(selectedUpgradePoint.transform.position);
                    }
                    else if (selectedUpgradePoint.tag == "Ground")
                    {
                        upgradeMenu.SetActive(false);
                    }
                }
                else if (upgradeMenu.activeInHierarchy)
                {
                    upgradeMenu.SetActive(false);
                }
            }   

        }
		#endregion

		if (_currentTower != null)
        {
            // Перемещаем выбранную башню за курсором мыши
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _currentTower.transform.position = mousePosition;

            // цвет башни в зависимости от типа поверхности
            if (IsMouseOverRoad(mousePosition))
            {
                _prefabSprite.color = new Color(1, 0, 0, 0.5f); // красный цвет
            }
            else if (IsMouseOverGround(mousePosition))
            {
                _prefabSprite.color = new Color(0, 1, 0, 0.5f); // зеленый цвет
            }

            if (IsTowerOverlap(_currentTower))
            {
                _prefabSprite.color = new Color(1, 0, 0, 0.5f); // красный цвет
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
                // убираем башню из состояния выбора
                _selectedTowerPrefab = null;
                if (_currentTower != null)
                {
                    Destroy(_currentTower);
                    _currentTower = null;
                }
            }
        }


    }
    public void SellTower()
    {
        Debug.Log("sellClicked");
        Tower selectedTower = selectedUpgradePoint.GetComponent<Tower>();
        _playerScript.GotWorth(selectedTower.cost);
        upgradeMenu.SetActive(false);
        Destroy(selectedUpgradePoint.gameObject);
    }

    // Метод для выбора башни через кнопку UI
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towers.Length)
        {
            Tower selectedTower = towers[towerIndex];

            if (_playerScript._playerMoney >= selectedTower.cost)
            {
                _selectedTowerPrefab = selectedTower.prefab;

                // Создаем новую башню, чтобы она следовала за курсором
                if (_currentTower != null)
                {
                    Destroy(_currentTower);
                }
                _currentTower = Instantiate(_selectedTowerPrefab);

                // получаем спрайт выбранной башни
                _prefabSprite = _currentTower.GetComponent<SpriteRenderer>();
                if (_prefabSprite == null)
                {
                    Debug.LogError("SpriteRenderer не найден на выбранной башне.");
                    return;
                }
                _prefabSprite.color = new Color(_prefabSprite.color.r, _prefabSprite.color.g, _prefabSprite.color.b, 0.5f); // полупрозрачность

                // Отключаем скрипт TowerShooting
                TowerShooting towerShooting = _currentTower.GetComponent<TowerShooting>();
                if (towerShooting != null)
                {
                    towerShooting.enabled = false;
                }
            }
            else
            {
                Debug.Log("Не хватает золота для покупки этой башни.");
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

            // Уменьшаем количество золота игрока
            Tower selectedTower = System.Array.Find(towers, t => t.prefab == _selectedTowerPrefab);
            if (selectedTower != null)
            {
                _playerScript._playerMoney -= selectedTower.cost;
                _playerScript.BuyingTower();
            }

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
    // находится ли указанная позиция на земле
    private bool IsMouseOverGround(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, groundLayerMask);
        return hit.collider != null;
    }

    // находится ли указанная позиция на дороге
    private bool IsMouseOverRoad(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, roadLayerMask);
        return hit.collider != null;
    }

    // пересечение с другими башнями
    private bool IsTowerOverlap(GameObject tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 0.5f, towerLayerMask);
        return colliders.Length > 1; // есть ли коллайдеры, кроме самой башни
    }
    #endregion
}
