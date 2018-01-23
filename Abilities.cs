using UnityEngine;
using System.Collections;

//TODO: Redo anything in PlayerController that says allAbilities[int] to instead say allAbilities[(int)AbilityID.THING_HERE], anything past mage is fine
public enum AbilityID
{
    //mage abilities
    NULL,
    MAGE_ATTACK,
    TORCH,
    FIREBALL,
    FOCUS,
    SHIELD,
    CHANNEL,
    //goblin abilities
    NERVOUS_SHANK,
    FLAIL_WILDLY,
    PILFERED_POTION
};

public class Abilities : MonoBehaviour
{
    public static Ability[] allAbilities;
    public static int numberOfMageAbilites = 6;
    public static int[] currentAssignedAbilities = new int[6]; // can only have 6 assigned abilities at once
    //Example of typical currentAssignedAbilities: [3, 2, 1, 4, 5, 6]
    //This has, in order, [Fireball, Torch, Attack, Focus, Shield, Channel]. Can't be 0, check DropdownController for more details


    private void Awake()
    {
        allAbilities = new Ability[System.Enum.GetNames(typeof(AbilityID)).Length];

        #region
        allAbilities[0] = new Ability("Level Select", AbilityID.NULL, 0, 0.0f, 0.0f, 0, "");
        allAbilities[1] = new Ability("Attack", AbilityID.MAGE_ATTACK, 0, 0.5f, 1.0f, 4, "physical");
        allAbilities[2] = new Ability("Torch", AbilityID.TORCH, 10, 1.5f, 0.0f, 10, "magical");
        allAbilities[3] = new Ability("Fireball", AbilityID.FIREBALL, 25, 3.0f, 10.0f, 35, "magical");
        allAbilities[4] = new Ability("Focus", AbilityID.FOCUS, 0, 1.0f, 15.0f, 50, "");
        allAbilities[5] = new Ability("Shield", AbilityID.SHIELD, 20, 0.5f, 15.0f, 1, "");
        allAbilities[6] = new Ability("Channel", AbilityID.CHANNEL, 15, 2.0f, 6.0f, 0, "");
        allAbilities[7] = new Ability("Nervous Shank", AbilityID.NERVOUS_SHANK, 0, 3.0f, 0.0f, 5, "physical");
        allAbilities[8] = new Ability("Flail Wildly", AbilityID.FLAIL_WILDLY, 0, 5.0f, 10.0f, 25, "physical");
        allAbilities[9] = new Ability("Pilfered Potion", AbilityID.PILFERED_POTION, 0, 4.0f, 30.0f, 40, "");
        #endregion
    }
}

public struct Ability
{
    public string name;
    public AbilityID id;
    public int cost;
    public float castTime;
    public float cooldown;
    public int power;
    public string damageType;

    public Ability(string name, AbilityID id, int cost, float castTime, float cooldown, int power, string damageType)
    {
        this.name = name;
        this.id = id;
        this.cost = cost;
        this.castTime = castTime;
        this.cooldown = cooldown;
        this.power = power;
        this.damageType = damageType;
    }
}
