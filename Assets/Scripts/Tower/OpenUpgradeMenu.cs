using UnityEngine;
using UnityEngine.EventSystems;

public class OpenUpgradeMenu : MonoBehaviour
{
    private bool menuIsOpened = false;
    public GameObject upgradeMenu;
    public GameObject selectedUpgradePoint;

	private void Update()
	{
        if(Input.GetMouseButtonDown(0))
		{
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);


                if(hit != false)
				{
                    selectedUpgradePoint = hit.transform.gameObject;
                    if(selectedUpgradePoint.tag == "Tower")
					{
                    Debug.Log("tag is tower");
                        upgradeMenu.SetActive(true);
                        upgradeMenu.transform.position = Camera.main.WorldToScreenPoint(selectedUpgradePoint.transform.position);
                    }
                    else if(selectedUpgradePoint.tag == "Ground")
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
}
	/*public void OpenMenu()
    {
        
        menuIsOpened = !menuIsOpened;
        if (menuIsOpened)
		{
            upgradeMenu.SetActive(true);

        }
		else
		{
            upgradeMenu.SetActive(false);
        }
    }*/

