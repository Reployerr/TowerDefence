using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildPointTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject selectedBuildPoint;
    [SerializeField] private Tower[] towers; // Массив башен
    [SerializeField] private Button[] towerButtons;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private Player _playerScript;

    [Header("Attributes")]
    [SerializeField] private Vector2 offsetTop;
    [SerializeField] private Vector2 offsetBot;


    private GameObject _selectedTowerPrefab; // Выбранный префаб башни
    private void Awake()
	{
        _playerScript = _playerObj.GetComponent<Player>();

        for (int i = 0; i < towerButtons.Length; i++)
        {
            int id = i;
            towerButtons[i].onClick.AddListener(() => PlaceTower(id));
        }
    }
	private void Update()
	{
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (hit != false)
                {
                    selectedBuildPoint = hit.transform.gameObject;
                    if (selectedBuildPoint.tag == "BuildPoint")
                    {
                        Debug.Log("tag is build point");

                        ShowBuildButton();
                        buildMenu.transform.position = Camera.main.WorldToScreenPoint(selectedBuildPoint.transform.position);
                       /* if (buildMenu.transform.localPosition.x > offsetTop.x)
						{
                            buildMenu.transform.localPosition = new Vector2(offsetTop.x, buildMenu.transform.localPosition.y);
                        }
                        if (buildMenu.transform.localPosition.y > offsetTop.y)
                        {
                            buildMenu.transform.localPosition = new Vector2(buildMenu.transform.localPosition.x, offsetTop.y);
                        }
                        if (buildMenu.transform.localPosition.x < offsetTop.x)
                        {
                            buildMenu.transform.localPosition = new Vector2(offsetTop.x, buildMenu.transform.localPosition.y);
                        }
                        if (buildMenu.transform.localPosition.y > offsetTop.y)
                        {
                            buildMenu.transform.localPosition = new Vector2(buildMenu.transform.localPosition.x, offsetTop.y);
                        }*/
                    }
                    else if (selectedBuildPoint.tag == "Ground")
                    {
                        HideBuildButton();
                    }
                }
                else if (buildMenu.activeInHierarchy)
                {
                    buildMenu.SetActive(false);
                }
            }

        }
    }
    public void PlaceTower(int towerIndex)
	{
        if(towerIndex >= 0 && towerIndex < towers.Length)
		{
            Tower selectedTower = towers[towerIndex];
            _selectedTowerPrefab = selectedTower.prefab;

            if (_playerScript._playerMoney >= selectedTower.cost)
            { 
                
                Instantiate(towers[towerIndex], selectedBuildPoint.transform.position, Quaternion.identity);
                BoxCollider2D pointCollider = selectedBuildPoint.GetComponent<BoxCollider2D>();
                pointCollider.enabled = false;
                buildMenu.SetActive(false);
            }
            
        }
        
	}

    private void ShowBuildButton()
	{
        buildMenu.SetActive(true);
    }

    private void HideBuildButton()
    {
        buildMenu.SetActive(false);
    }
}
