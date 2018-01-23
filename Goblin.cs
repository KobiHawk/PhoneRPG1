using UnityEngine;
using System.Collections;

public class Goblin : EnemyController {

    private float[] abilityCooldowns = new float[3];

    private void Start()
    {
        enemyHealth.minValue = 0;
        enemyHealth.maxValue = 100;
        enemyHealth.value = 100;

        actionBar.minValue = 0;
        actionBar.maxValue = 100;
        actionBar.value = 0;

        for(int i = 1; i < abilityCooldowns.Length; i++)
        {
            abilityCooldowns[i] = 0;
        }
    }

    private void Update()
    {
        if (!SystemController.gameOver)
        {
            for (int i = 0; i < abilityCooldowns.Length; i++)
            {
                if (abilityCooldowns[i] > 0)
                {
                    abilityCooldowns[i]--;
                }
            }

            chooseAbility();

            if (currentlyUsingAbility)
            {
                actionBar.value += actionProgressPerFrame;
            }

            if (actionBar.value == actionBar.maxValue)
            {
                useAbility(currentAbility);
            }
        }
    }

    /*
     * Goblin AI:
     * 1. If below half health and Pilfered Potion is available, use Pilfered Potion
     * 2. If Flail Wildly is available, use Flail Wildly
     * 3. Use Nervous Shank
     * Wait WAIT_TIME between attacks
     */
    void chooseAbility()
    {
        if (!currentlyUsingAbility)
        {
            if (waitFramesRemaining == 0)
            {
                if (abilityCooldowns[2] <= 0 && enemyHealth.value <= (enemyHealth.maxValue / 2))
                {
                    setNextAbility((int)AbilityID.PILFERED_POTION);
                }
                else if (abilityCooldowns[1] <= 0)
                {
                    setNextAbility((int)AbilityID.FLAIL_WILDLY);
                }
                else
                {
                    setNextAbility((int)AbilityID.NERVOUS_SHANK);
                }
            }
            else
            {
                waitFramesRemaining--;
            }
        }
    }

    void setNextAbility(int id)
    {
        currentlyUsingAbility = true;
        currentAbility = id;
        actionText.text = Abilities.allAbilities[id].name;
        actionProgressPerFrame = actionBar.maxValue / (Abilities.allAbilities[id].castTime * SystemController.FRAME_RATE);
    }
    
    void useAbility(int id)
    {
        switch(id)
        {
            case (int)AbilityID.NERVOUS_SHANK:
                dealDamage(Abilities.allAbilities[(int)AbilityID.NERVOUS_SHANK].power);
                abilityCooldowns[0] = Abilities.allAbilities[(int)AbilityID.NERVOUS_SHANK].cooldown * SystemController.FRAME_RATE;
                break;

            case (int)AbilityID.FLAIL_WILDLY:
                dealDamage(Abilities.allAbilities[(int)AbilityID.FLAIL_WILDLY].power);
                abilityCooldowns[1] = Abilities.allAbilities[(int)AbilityID.FLAIL_WILDLY].cooldown * SystemController.FRAME_RATE;
                break;

            case (int)AbilityID.PILFERED_POTION:
                enemyHealth.value += Abilities.allAbilities[(int)AbilityID.PILFERED_POTION].power;
                abilityCooldowns[2] = Abilities.allAbilities[(int)AbilityID.PILFERED_POTION].cooldown * SystemController.FRAME_RATE;
                break;
        }
        waitFramesRemaining = WAIT_TIME;
        currentlyUsingAbility = false;
        actionBar.value = 0;
    }
}
