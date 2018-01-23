using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //Unity public variables, set in inspector
    private Button[] abilityButtons = new Button[6];
    public Slider health;
    public Slider mana;
    public Slider actionBar;
    private Text actionText;
    public Slider enemyHealth;

    //Scripting private variables
    private int currentAbility = (int)AbilityID.NULL; // The current ability, will be used once actionBar fills
    private int queuedAbility = (int)AbilityID.NULL; // The queued ability, will be set to currentAbility once that is used
    private int currentAbilityButton = -1; // 
    private int queuedAbilityButton = -1;

    private float actionProgressPerFrame = 0;

    private int[] abilityCooldowns = new int[6]; // represents the amount of time in frames remaining until the ability in each button slot can be used again

    //Gameplay variables
    private float focusManaRemaining = 0;
    private float focusIncrementPerFrame;

    private int shieldFramesRemaining = 0;

    private float magicDamageMultipler = 1;
    private bool channelActive = false;



    private void Awake()
    {
        Application.targetFrameRate = SystemController.FRAME_RATE; 
    }
    // Use this for initialization
    void Start () {

        SystemController.gameOver = false;
        actionText = actionBar.GetComponentInChildren<Text>();

        //Populate the list of ability buttons with Buttons
        string name;
        for (int i = 1; i < abilityButtons.Length+1;  i++)
        {
            name = "AbilityButton" + i;
            abilityButtons[i - 1] = GameObject.FindGameObjectWithTag(name).GetComponent<Button>();
        }

        //Assign names to ability buttons, and also set defaults if abilities were not set
        for(int i = 0; i < abilityButtons.Length; i++)
        {
            if (Abilities.currentAssignedAbilities[i] == 0) //don't want to set a name to null if it was unselected, so use default layout
            {
                Abilities.currentAssignedAbilities[i] = i + (int)AbilityID.MAGE_ATTACK;
            }
            abilityButtons[i].GetComponentInChildren<Text>().text = Abilities.allAbilities[Abilities.currentAssignedAbilities[i]].name;
        }

        //Set listeners for buttons after player has set their abilities
        //TODO This is bad practice but when inserted into the loop above I get out of bounds exceptions, it is a spooky mystery
        
        abilityButtons[0].onClick.AddListener(delegate { ButtonClickedFlag(Abilities.currentAssignedAbilities[0], 0); });
        abilityButtons[1].onClick.AddListener(delegate { ButtonClickedFlag(Abilities.currentAssignedAbilities[1], 1); });
        abilityButtons[2].onClick.AddListener(delegate { ButtonClickedFlag(Abilities.currentAssignedAbilities[2], 2); });
        abilityButtons[3].onClick.AddListener(delegate { ButtonClickedFlag(Abilities.currentAssignedAbilities[3], 3); });
        abilityButtons[4].onClick.AddListener(delegate { ButtonClickedFlag(Abilities.currentAssignedAbilities[4], 4); });
        abilityButtons[5].onClick.AddListener(delegate { ButtonClickedFlag(Abilities.currentAssignedAbilities[5], 5); });

        //Set base values for stats
        health.minValue = 0;
        health.maxValue = 100;
        health.value = 100;

        mana.minValue = 0;
        mana.maxValue = 100;
        mana.value = 100;

        actionBar.minValue = 0;
        actionBar.maxValue = 100;
        actionBar.value = 0;

        //Make sure all cooldowns are at 0
        for(int i = 0; i < abilityCooldowns.Length; i++)
        {
            abilityCooldowns[i] = 0;
        }

        //
        focusIncrementPerFrame = (Abilities.allAbilities[4].power / (float)(SystemController.FRAME_RATE * 8)); // takes 8 seconds to fully replenish mana, TODO magic number
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!SystemController.gameOver)
        {
            //First, do housekeeping for ongoing effects.
            if (focusManaRemaining > 0.0f)
            {
                focusManaRemaining -= focusIncrementPerFrame;
                mana.value += focusIncrementPerFrame;
            }

            if (shieldFramesRemaining > 0) { shieldFramesRemaining--; }

            //Then, manage cooldowns for used abilities, and add a gradient to the button to show how much longer the cooldown is.
            for (int i = 0; i < abilityCooldowns.Length; i++)
            {
               
                if (abilityCooldowns[i] > 0)
                {
                    abilityCooldowns[i]--;

                    ColorBlock cb = abilityButtons[i].colors;
                    float gradient = ((Abilities.allAbilities[Abilities.currentAssignedAbilities[i]].cooldown * SystemController.FRAME_RATE) - abilityCooldowns[i] * 0.75f) 
                                       / (Abilities.allAbilities[Abilities.currentAssignedAbilities[i]].cooldown * SystemController.FRAME_RATE);
                    Color newColor = new Color(gradient, gradient, gradient, 1);
                    cb.normalColor = newColor;
                    cb.highlightedColor = newColor;
                    abilityButtons[i].colors = cb;
                }
            }

            //Finally, see if the player clicked a button, and handle it if they did.
            if (currentAbility != (int)AbilityID.NULL)
            {
                actionBar.value += actionProgressPerFrame;
                if (actionBar.value >= actionBar.maxValue)
                {
                    useAbility(currentAbility);

                    actionBar.value = 0;
                    if (queuedAbility == (int)AbilityID.NULL)
                    {
                        currentAbility = (int)AbilityID.NULL;
                    }
                    else //set the next ability to be used from the queued ability 
                    {
                        currentAbility = queuedAbility;
                        currentAbilityButton = queuedAbilityButton;
                        actionText.text = Abilities.allAbilities[(int)currentAbility].name;

                        actionProgressPerFrame = actionBar.maxValue / (Abilities.allAbilities[(int)currentAbility].castTime * SystemController.FRAME_RATE);

                        queuedAbility = (int)AbilityID.NULL;
                        queuedAbilityButton = -1;
                    }

                }
            }
        }
	}

    /*
     * void useAbility(int id)
     * 
     * This function actually uses the abilities.
     * Each case in the switch statement has a different effect based on the ability used.
     * For more details, check /Assets/Notes for ability descriptions.
     */
    void useAbility(int id)
    {
        switch(id)
        {
            case (int)AbilityID.MAGE_ATTACK:
                dealDamage(Abilities.allAbilities[1].power, Abilities.allAbilities[1].damageType);
                Debug.Log("Hey it's " + currentAbilityButton);
                abilityCooldowns[currentAbilityButton] = Mathf.RoundToInt((Abilities.allAbilities[1].cooldown * SystemController.FRAME_RATE));
                mana.value -= Abilities.allAbilities[1].cost;
                break;

            case (int)AbilityID.TORCH:
                dealDamage(Abilities.allAbilities[2].power, Abilities.allAbilities[2].damageType);
                abilityCooldowns[currentAbilityButton] = Mathf.RoundToInt((Abilities.allAbilities[2].cooldown * SystemController.FRAME_RATE));
                mana.value -= Abilities.allAbilities[2].cost;
                break;

            case (int)AbilityID.FIREBALL:
                dealDamage(Abilities.allAbilities[3].power, Abilities.allAbilities[3].damageType);
                abilityCooldowns[currentAbilityButton] = Mathf.RoundToInt((Abilities.allAbilities[3].cooldown * SystemController.FRAME_RATE));
                mana.value -= Abilities.allAbilities[3].cost;
                break;

            case (int)AbilityID.FOCUS:
                focusManaRemaining = (float)Abilities.allAbilities[4].power;
                abilityCooldowns[currentAbilityButton] = Mathf.RoundToInt((Abilities.allAbilities[4].cooldown * SystemController.FRAME_RATE));
                mana.value -= Abilities.allAbilities[4].cost;
                break;

            case (int)AbilityID.SHIELD:
                shieldFramesRemaining = Abilities.allAbilities[5].power * SystemController.FRAME_RATE;
                abilityCooldowns[currentAbilityButton] = Mathf.RoundToInt((Abilities.allAbilities[5].cooldown * SystemController.FRAME_RATE));
                mana.value -= Abilities.allAbilities[5].cost;
                break;

            case (int)AbilityID.CHANNEL:
                magicDamageMultipler *= 2;
                channelActive = true;
                abilityCooldowns[currentAbilityButton] = Mathf.RoundToInt((Abilities.allAbilities[6].cooldown * SystemController.FRAME_RATE));
                mana.value -= Abilities.allAbilities[6].cost;
                break;

            default:
                break;
        }
        actionText.text = "";
        currentAbilityButton = -1;
    }

    void dealDamage(int power, string damageType)
    {
        if (damageType == "physical")

        {
            enemyHealth.value -= power;
        }
        else if (damageType == "magical")
        {
            enemyHealth.value -= power * magicDamageMultipler;
            if(channelActive)
            {
                channelActive = false;
                magicDamageMultipler /= 2;
            }
        }
        if(enemyHealth.value == 0)
        {
            Debug.Log("You win!");
            SystemController.gameOver = true;
        }
    }

    public void takeDamage(int power)
    {
        if(shieldFramesRemaining > 0)
        {
            //don't take damage
        }
        else
        {
            health.value -= power;
            if(health.value == 0)
            {
                Debug.Log("You lose!");
                SystemController.gameOver = true;
            }
        }
    }

    /* Actions
     * 
     * When selecting an action (pressing a button), we do several things.
     * 
     * 1. Make sure we can actually afford the skill.
     * 2. If the currentAbility is NULL (value == 0), add it. Otherwise, queue it up. Queue only holds one ability; the last one pressed.
     * 3. Set the amount to add to the actionBar each frame such that it takes castTime seconds to use the ability. actionBar increments from 0 to 100.
     */
    void setNextAbility(AbilityID nextAbility, int buttonIndex)
    {
        if (Abilities.allAbilities[(int)nextAbility].cost <= mana.value)
        {
            if (abilityCooldowns[buttonIndex] == 0)
            {
                if (currentAbility == (int)AbilityID.NULL)
                {
                    currentAbility = (int)nextAbility;
                    currentAbilityButton = buttonIndex;
                    actionText.text = Abilities.allAbilities[(int)nextAbility].name;

                    actionProgressPerFrame = actionBar.maxValue / (Abilities.allAbilities[(int)nextAbility].castTime * SystemController.FRAME_RATE);
                }
                else
                {
                    if ((currentAbility != (int)nextAbility || Abilities.allAbilities[(int)currentAbility].cooldown == 0.0f))
                    {
                        queuedAbility = (int)nextAbility;
                        queuedAbilityButton = buttonIndex;
                    }
                    
                }
            }
            else
            {
                Debug.Log("Ability still on cooldown!");
            }
        }
        else
        {
            Debug.Log("No mana to cast!");
        }
    }

    void ButtonClickedFlag(int newAbilityId, int buttonIndex)
    {
        Debug.Log(buttonIndex);
        setNextAbility((AbilityID)newAbilityId, buttonIndex);
    }
}
