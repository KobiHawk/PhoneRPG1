using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour {

    private Button changeAbilities;
    public static int FRAME_RATE = 30; // target FPS is 30 for mobile
    public static bool gameOver = false;

    // Use this for initialization
    void Start () {
        changeAbilities = GameObject.FindGameObjectWithTag("ReturnToBattle").GetComponent<Button>();
        changeAbilities.onClick.AddListener(ChangeAbilities);
	}

    //toggle between status screen and battle
    void ChangeAbilities()
    {
        Debug.Log("hey");
        if(changeAbilities.GetComponentInChildren<Text>().text == "Change Abilities")
        {
            //SceneManager.LoadScene(1);
            SceneManager.LoadScene("Status");
        }
        else
        {
            //SceneManager.LoadScene(0);
            SceneManager.LoadScene("Combat");
        }
    }
}
