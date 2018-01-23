using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuTabController : MonoBehaviour {

    private GameObject[] screens = new GameObject[4];
    private Button[] tabButtons = new Button[4];

	// Use this for initialization
	void Start () {
        screens = GameObject.FindGameObjectsWithTag("Screen");
        tabButtons = GetComponentsInChildren<Button>();

        tabButtons[0].onClick.AddListener(delegate { DisableOtherScreens(0); });
        tabButtons[1].onClick.AddListener(delegate { DisableOtherScreens(1); });
        tabButtons[2].onClick.AddListener(delegate { DisableOtherScreens(2); });
        tabButtons[3].onClick.AddListener(delegate { DisableOtherScreens(3); });
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void DisableOtherScreens(int index)
    {
        Debug.Log(index);
        for(int i = 0; i < screens.Length; i++)
        {
            if(index != i)
            {
                screens[i].SetActive(false);
            }
            else
            {
                screens[i].SetActive(true);
            }
        }
    }
}
