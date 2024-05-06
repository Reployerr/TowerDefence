using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask groundLayerMask; // ����� ��� ����������� �����
    public LayerMask roadLayerMask; // ����� ��� ����������� ������
    public LayerMask towerLayerMask; // ����� ��� ����������� ���� �����
    public Button[] towerButtons; // ������ ������ ��� ������ �����
    public GameObject[] towerPrefabs; // ������ �������� �����

    private GameObject _selectedTowerPrefab; // ��������� ������ �����
    private GameObject _currentTower; // ������� ����������� �����
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        // ��������� ������ ������ ���������� �������
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
            // ���������� ������� ����� �� �������� ����
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            _currentTower.transform.position = mousePosition;

            // ��� ������� ����� ������ ���� ��������� �����
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
                Debug.Log("RightClick");
                _selectedTowerPrefab = null;
                _currentTower = null;

            }
        }
    }

    // ����� ��� ������ ����� ����� ������ UI
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            _selectedTowerPrefab = towerPrefabs[towerIndex];
            // ������� ����� �����, ����� ��� ��������� �� ��������
            if (_currentTower != null)
            {
                Destroy(_currentTower);
            }
            _currentTower = Instantiate(_selectedTowerPrefab);
        }
    }

    // ����� ��� ���������� ����� �� �����
    private void PlaceTower(Vector3 position)
    {
        if (_selectedTowerPrefab != null)
        {
            Instantiate(_selectedTowerPrefab, position, Quaternion.identity);
            Debug.Log("����� �������������: " + position);
        }
    }

    //��������� �� ��������� ������� ��� ������
    private bool IsMouseOverGround(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, groundLayerMask);
        return hit.collider != null;
    }

    //��������� �� ��������� ������� �� ������
    private bool IsMouseOverRoad(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, roadLayerMask);
        return hit.collider != null;
    }

    // ����� ��� �������� ����������� � ������� �������
    private bool IsTowerOverlap(GameObject tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 1f, towerLayerMask);
        return colliders.Length > 1; //���� �� ����������, ����� ����� �����
    }
}