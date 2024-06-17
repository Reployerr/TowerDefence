using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayerMask; // ����� ��� ����������� �����
    [SerializeField] private LayerMask roadLayerMask; // ����� ��� ����������� ������
    [SerializeField] private LayerMask towerLayerMask; // ����� ��� ����������� ���� �����
    [SerializeField] private Button[] towerButtons; // ������ ������ ��� ������ �����
    [SerializeField] private Tower[] towers; // ������ �����

    private GameObject _selectedTowerPrefab; // ��������� ������ �����
    private GameObject _currentTower; // ������� ����������� �����
    private Camera _mainCamera;
    private SpriteRenderer _prefabSprite;

    private int playerGold = 100; // ���������� ������ � ������

    private void Awake()
    {
        _mainCamera = Camera.main;

        // ������ ������ ���������� �������
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i; // ��������� ����� ���������� i ��� �����������
            towerButtons[i].onClick.AddListener(() => SelectTower(index));
        }
    }

    private void Update()
    {
        if (_currentTower != null)
        {
            // ���������� ��������� ����� �� �������� ����
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _currentTower.transform.position = mousePosition;

            // ���� ����� � ����������� �� ���� �����������
            if (IsMouseOverRoad(mousePosition))
            {
                _prefabSprite.color = new Color(1, 0, 0, 0.5f); // ������� ����
            }
            else if (IsMouseOverGround(mousePosition))
            {
                _prefabSprite.color = new Color(0, 1, 0, 0.5f); // ������� ����
            }

            if (IsTowerOverlap(_currentTower))
            {
                _prefabSprite.color = new Color(1, 0, 0, 0.5f); // ������� ����
            }

            // ���������� �����
            if (Input.GetMouseButtonDown(0))
            {
                if (IsMouseOverGround(mousePosition) && !IsMouseOverRoad(mousePosition) && !IsTowerOverlap(_currentTower))
                {
                    PlaceTower(mousePosition);
                }
                else
                {
                    Debug.Log("������ ����������");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                // ������� ����� �� ��������� ������
                _selectedTowerPrefab = null;
                if (_currentTower != null)
                {
                    Destroy(_currentTower);
                    _currentTower = null;
                }
            }
        }
    }

    // ����� ��� ������ ����� ����� ������ UI
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towers.Length)
        {
            Tower selectedTower = towers[towerIndex];

            if (playerGold >= selectedTower.cost)
            {
                _selectedTowerPrefab = selectedTower.prefab;

                // ������� ����� �����, ����� ��� ��������� �� ��������
                if (_currentTower != null)
                {
                    Destroy(_currentTower);
                }
                _currentTower = Instantiate(_selectedTowerPrefab);

                // �������� ������ ��������� �����
                _prefabSprite = _currentTower.GetComponent<SpriteRenderer>();
                if (_prefabSprite == null)
                {
                    Debug.LogError("SpriteRenderer �� ������ �� ��������� �����.");
                    return;
                }
                _prefabSprite.color = new Color(_prefabSprite.color.r, _prefabSprite.color.g, _prefabSprite.color.b, 0.5f); // ����������������

                // ��������� ������ TowerShooting
                TowerShooting towerShooting = _currentTower.GetComponent<TowerShooting>();
                if (towerShooting != null)
                {
                    towerShooting.enabled = false;
                }
            }
            else
            {
                Debug.Log("�� ������� ������ ��� ������� ���� �����.");
            }
        }
    }

    // ����� ��� ���������� ����� �� �����
    private void PlaceTower(Vector3 position)
    {
        if (_selectedTowerPrefab != null)
        {
            Instantiate(_selectedTowerPrefab, position, Quaternion.identity);
            Debug.Log("����� �����������: " + position);

            // ��������� ���������� ������ ������
            Tower selectedTower = System.Array.Find(towers, t => t.prefab == _selectedTowerPrefab);
            if (selectedTower != null)
            {
                playerGold -= selectedTower.cost;
            }

            // ������� ����� �� ��������� ������
            _selectedTowerPrefab = null;
            if (_currentTower != null)
            {
                Destroy(_currentTower);
                _currentTower = null;
            }
        }
    }

    #region �������� �� ����������� ���� �����������
    // ��������� �� ��������� ������� �� �����
    private bool IsMouseOverGround(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, groundLayerMask);
        return hit.collider != null;
    }

    // ��������� �� ��������� ������� �� ������
    private bool IsMouseOverRoad(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, roadLayerMask);
        return hit.collider != null;
    }

    // ����������� � ������� �������
    private bool IsTowerOverlap(GameObject tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 1f, towerLayerMask);
        return colliders.Length > 1; // ���� �� ����������, ����� ����� �����
    }
    #endregion
}
