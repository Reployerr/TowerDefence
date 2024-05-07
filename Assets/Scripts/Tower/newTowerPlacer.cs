using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask groundLayerMask; // ����� ��� ����������� �����
    [SerializeField] private LayerMask roadLayerMask; // ����� ��� ����������� ������
    [SerializeField] private LayerMask towerLayerMask; // ����� ��� ����������� ���� �����
    [SerializeField] private Button[] towerButtons; // ������ ������ ��� ������ �����
    [SerializeField] private GameObject[] towerPrefabs; // ������ �������� �����

    private GameObject _selectedTowerPrefab; // ��������� ������ �����
    private GameObject _currentTower; // ������� ����������� �����
    private Camera _mainCamera;
    private SpriteRenderer _prefabSprite;

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
                //������� ����� �� ��������� ������
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
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            _selectedTowerPrefab = towerPrefabs[towerIndex];

            // ������� ����� �����, ����� ��� ��������� �� ��������
            if (_currentTower != null)
            {
                Destroy(_currentTower);
            }
            _currentTower = Instantiate(_selectedTowerPrefab);

            //�������� ������ ��������� �����
            _prefabSprite = _currentTower.GetComponent<SpriteRenderer>();
            
            // ��������� ������ TowerShooting
            TowerShooting towerShooting = _currentTower.GetComponent<TowerShooting>();
            if (towerShooting != null)
            {
                towerShooting.enabled = false;
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
	//��������� �� ��������� ������� �� �����
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

    // ����������� � ������� �������
    private bool IsTowerOverlap(GameObject tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 1f, towerLayerMask);
        return colliders.Length > 1; //���� �� ����������, ����� ����� �����
    }
	#endregion
}
