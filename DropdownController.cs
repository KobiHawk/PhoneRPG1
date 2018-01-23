using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour {

    public int index; // set in editor to identify button
    private Dropdown dropdown;


	// Use this for initialization
	void Start () {
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });

        dropdown.ClearOptions();

        Dropdown.OptionData data;

        for(int i = (int)AbilityID.MAGE_ATTACK; i <= Abilities.numberOfMageAbilites; i++)
        {
            data = new Dropdown.OptionData();
            data.text = Abilities.allAbilities[i].name;
            dropdown.options.Add(data);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DropdownValueChanged()
    {
        Abilities.currentAssignedAbilities[index] = dropdown.value + (int)AbilityID.MAGE_ATTACK; // change if adding class past mage
        Debug.Log("You have set button #" + index + " to " + Abilities.currentAssignedAbilities[index]);
    }
}
