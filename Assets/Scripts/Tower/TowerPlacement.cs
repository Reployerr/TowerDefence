using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TowerPlacement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float upgradePanelHideTime;

    [Header("References")]
    [SerializeField] private LayerMask groundLayerMask; // ����� ��� ����������� �����
    [SerializeField] private LayerMask roadLayerMask; // ����� ��� ����������� ������
    [SerializeField] private LayerMask towerLayerMask; // ����� ��� ����������� ���� �����
    [SerializeField] private Button[] towerButtons; // ������ ������ ��� ������ �����
    [SerializeField] private Tower[] towers; // ������ �����
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private Player _playerScript;
    [SerializeField] private AudioSource _coinAudioSource;
    [SerializeField] private AudioClip _coinAudioClip;
    [SerializeField] private Animator _upgradeMenuAnimator;
    [SerializeField] private Animator _towerAnimator;

   // [SerializeField] private TowersUpgrades.TypesOfNextUpgrades type = TowersUpgrades.TypesOfNextUpgrades.none;

    public GameObject upgradeMenu;//���� � ���������� � �������� �����
    public GameObject selectedUpgradePoint;//����� ����� ��� ������ ���� � ���������� � ��������
    public GameObject selectedTowerToUpgrade; //����� ��� ���������
    private GameObject _selectedTowerPrefab; // ��������� ������ �����
    private GameObject _currentTower; // ������� ����������� �����
    private Camera _mainCamera;
    private SpriteRenderer _prefabSprite;
    


    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerScript = _playerObj.GetComponent<Player>();

        // ������ ������ ���������� �������
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i; 
            towerButtons[i].onClick.AddListener(() => SelectTower(index));
        }
    }

    private void Update()
    {
		#region UpgradeSellMenu

		if (Input.GetMouseButtonDown(1))
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
                        Animator towerAnimator = selectedUpgradePoint.GetComponent<Animator>();
                        _towerAnimator = towerAnimator;
                        _towerAnimator.Play("Selected");

                        Debug.Log("tag is tower");

                        ShowPanel();
                        upgradeMenu.transform.position = Camera.main.WorldToScreenPoint(selectedUpgradePoint.transform.position);
                    }
                    else if (selectedUpgradePoint.tag == "Ground")
                    {
                        _towerAnimator.Play("Idle");
                        HidePanel();  
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
            upgradeMenu.SetActive(false);
            // ���������� ��������� ����� �� �������� ����
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = -3f;
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
    private void ShowPanel()
	{
        upgradeMenu.SetActive(true);
       // _upgradeMenuAnimator.SetBool("isShow", false);
        _upgradeMenuAnimator.Play("UpgradeMenuShow");
    }
    private void HidePanel()
    {
        //_upgradeMenuAnimator.SetBool("isShow", false);
        _upgradeMenuAnimator.Play("UpgradeMenuHide");
        StartCoroutine(ExecuteAfterTime(upgradePanelHideTime)); // ��������� ���� � ��������� upgradePanelHideTime

    }
    IEnumerator ExecuteAfterTime(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        upgradeMenu.SetActive(false);
    }
    public void UpgradeTower()
	{

        Debug.Log("upgradeClicked");

        Tower selectedTower = selectedUpgradePoint.GetComponent<Tower>();
        TowersUpgrades upgrading = selectedUpgradePoint.GetComponent<TowersUpgrades>(); 

        if (upgrading != null) 
        {
            TypesOfNextUpgrades upgradeType = upgrading.upgradeType; //��� ���������

            if (_playerScript._playerMoney >= selectedTower.cost) // ���� ������� �����
			{
                //  ��� ���������
                if (upgradeType == TypesOfNextUpgrades.ArcherLVL2)
                {
                    Destroy(selectedTower.gameObject);
                    SpawnUpgradedTower(selectedTower, upgrading);
                    //Debug.Log("Archer upgraded to lvl 2");
                }
                else if (upgradeType == TypesOfNextUpgrades.CanonLVL2)
                {
                    SpawnUpgradedTower(selectedTower, upgrading);

                }
                else if (upgradeType == TypesOfNextUpgrades.MageLVL2)
                {
                    SpawnUpgradedTower(selectedTower, upgrading);
                }
            }
                
            else
            {
                Debug.Log("�� ������� ���������");
            }
        }
        else
        {
            Debug.LogError("�� ������ ������ TowersUpgrades �� �������");
        }


    }

    public void SpawnUpgradedTower(Tower selectedTower, TowersUpgrades upgrading)
    {
        Debug.Log("ArcherLVL2 selected for upgrade");
        _playerScript.UpgradingTower(upgrading.UpgradeCost);
        Instantiate(selectedTower.NextUpdates[0], selectedUpgradePoint.transform.position, Quaternion.identity);//����� ����� �����
        Destroy(selectedUpgradePoint.gameObject); // �������� ������ �����
        upgradeMenu.SetActive(false);
    }

    public void SellTower()
    {
        Debug.Log("sellClicked");
        Tower selectedTower = selectedUpgradePoint.GetComponent<Tower>();
        _playerScript.GotWorth(selectedTower.cost);
        upgradeMenu.SetActive(false);
        Destroy(selectedUpgradePoint.gameObject);
        _coinAudioSource.pitch = UnityEngine.Random.Range(1f, 1.5f);
        _coinAudioSource.PlayOneShot(_coinAudioClip);
    }

    // ����� ��� ������ ����� ����� ������ UI
    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towers.Length)
        {
            Tower selectedTower = towers[towerIndex];

            if (_playerScript._playerMoney >= selectedTower.cost)
            {
                _selectedTowerPrefab = selectedTower.prefab;

                // ������� ����� �����, ����� ��� ��������� �� ��������
                if (_currentTower != null)
                {
                    Destroy(_currentTower);
                }
                _currentTower = Instantiate(_selectedTowerPrefab);

                // �������� ������ ��������� �����
                Transform childSprite = _currentTower.transform.Find("Sprite");
                _prefabSprite = childSprite.GetComponent<SpriteRenderer>();
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
                _playerScript._playerMoney -= selectedTower.cost;
                _playerScript.BuyingTower();
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tower.transform.position, 0.5f, towerLayerMask);
        return colliders.Length > 1; // ���� �� ����������, ����� ����� �����
    }
    #endregion
}
