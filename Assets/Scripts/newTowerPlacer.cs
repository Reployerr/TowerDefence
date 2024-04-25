using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask groundLayerMask; // ����� ��� ����������� �����
    public LayerMask roadLayerMask; // ����� ��� ����������� ������
    public LayerMask towerLayerMask; // ����� ��� ����������� ���� �����
    public Button[] towerButtons; // ������ ������ ��� ������ �����
    public GameObject[] towerPrefabs; // ������ �������� �����

    private GameObject selectedTowerPrefab; // ��������� ������ �����
    private GameObject currentTower; // ������� ����������� �����
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        // ��������� ������ ������ ���������� �������
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i; // ��������� ����� ���������� i ��� �����������
            towerButtons[i].onClick.AddListener(() => SelectTower(index));
        }
    }

    private void Update()
    {
        if (currentTower != null)
        {
            // ���������� ������� ����� �� �������� ����
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentTower.transform.position = mousePosition;

            // ��� ������� ����� ������ ���� ��������� �����
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

    // ����� ��� ������ ����� ����� ������ UI
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            selectedTowerPrefab = towerPrefabs[towerIndex];
            // ������� ����� �����, ����� ��� ��������� �� ��������
            if (currentTower != null)
            {
                Destroy(currentTower);
            }
            currentTower = Instantiate(selectedTowerPrefab);
        }
    }

    // ����� ��� ���������� ����� �� �����
    private void PlaceTower(Vector3 position)
    {
        if (selectedTowerPrefab != null)
        {
            Instantiate(selectedTowerPrefab, position, Quaternion.identity);
            Debug.Log("Tower placed at position: " + position);
        }
    }

    // ����� ��� ��������, ��������� �� ��������� ������� ��� ������
    private bool IsMouseOverGround(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, groundLayerMask);
        return hit.collider != null;
    }

    // ����� ��� ��������, ��������� �� ��������� ������� �� ������
    private bool IsMouseOverRoad(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, roadLayerMask);
        return hit.collider != null;
    }

    // ����� ��� �������� ����������� � ������� �������
    private bool IsTowerOverlap(GameObject tower)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 0.5f, towerLayerMask);
        return colliders.Length > 1; // ���������, ���� �� ����������, ����� ����� �����
    }
}