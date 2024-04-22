using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    public LayerMask groundLayerMask;

    private GameObject _buildingPrefab;
    private GameObject _toBuild;

    private Camera _mainCamera;

    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    private void Update()
    {
        if (_buildingPrefab != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if(_toBuild.activeSelf) _toBuild.SetActive(false);
            }
            else if (!_toBuild.activeSelf) _toBuild.SetActive(true);
            //only works in 3d
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                
            if (Physics.Raycast(_ray, out _hit, 1000f, groundLayerMask))
           {
                if (!_toBuild.activeSelf) _toBuild.SetActive(true);

                _toBuild.transform.position = _hit.point;
           }
           
           else if (_toBuild.activeSelf) _toBuild.SetActive(false);
        }
        //////////////////////////////////////////////
        ///

        RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        Ray2D ray2d = Physics2D.
        if (Physics2D.Raycast(ray, ))

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
        _toBuild.SetActive(false);
    }
}
