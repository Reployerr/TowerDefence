using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseShop : MonoBehaviour
{
    public bool pressCheck = false;
    public bool FirstpressCheck = false;
    [SerializeField] private Animator shopAnimator; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressingButton()
	{
        FirstpressCheck = true;

        pressCheck = !pressCheck;

        if(FirstpressCheck == true)
		{
            shopAnimator.SetBool("FirstCheck", true);
        }

        if(pressCheck == true)
		{
            shopAnimator.SetBool("Check", true);
        }
        else if (pressCheck == false)
		{
            shopAnimator.SetBool("Check", false);
        }
	}
}
