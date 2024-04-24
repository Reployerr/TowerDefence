using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    public LayerMask groundLayerMask;//маска земли
    public LayerMask roadLayerMask;//маска дороги

    private GameObject _buildingPrefab; //префаб башни
    private GameObject _toBuild; //объект выбранной башни привязанный к мыши на основе префаба 
    private SpriteRenderer prefabSprite; //спрайт выбранной башни
     
    

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    private void Update()
    {
        
        if (_buildingPrefab != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (_toBuild.activeSelf) _toBuild.SetActive(false);
            }

            else if (!_toBuild.activeSelf) _toBuild.SetActive(true);

            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition); // позиция мыши 
            mouseWorldPosition.z = 0f;

            if (Physics2D.Raycast(mouseWorldPosition, _mainCamera.transform.position, 0.3f/*размер луча*/, groundLayerMask))
            {
                Debug.DrawRay(mouseWorldPosition, _mainCamera.transform.position, Color.red);
                if (!_toBuild.activeSelf) _toBuild.SetActive(true);
                Debug.Log("groundLayer");
                _toBuild.transform.position = mouseWorldPosition;

                //изменения цвета выбранной башни на зеленый если на земле 
                prefabSprite.color = new Color(0, 255, 12);

            }
            else if (_toBuild.activeSelf) _toBuild.SetActive(false);

            if(Physics2D.Raycast(mouseWorldPosition, _mainCamera.transform.position, 0.3f/*размер луча*/, roadLayerMask))
            {
                Debug.Log("road triggered");
                //изменения цвета выбранной башни на красный если на дороге 
                prefabSprite.color = new Color(255, 0, 0);
            }
        }  

    }

    public void SetBuildingPrefab(GameObject prefab)
    {
        _buildingPrefab = prefab;
        
        PrepareBuilding();
    }

    private void PrepareBuilding()
    {
        if (_toBuild) Destroy(_toBuild);

        _toBuild = Instantiate(_buildingPrefab);

        prefabSprite = _toBuild.GetComponent<SpriteRenderer>();
        prefabSprite.color = new Color(255, 0, 0);
        _toBuild.SetActive(false);
    }
}
