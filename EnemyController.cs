using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    //Unity components
    public PlayerController player;
    public Slider enemyHealth;
    public Slider actionBar;
    public Text actionText;

    //Scripting private variables
    protected int currentAbility = (int)AbilityID.NULL;
    protected float actionProgressPerFrame = 0;

    //Variables
    protected int WAIT_TIME = SystemController.FRAME_RATE; // time in frames to wait between actions
    protected int waitFramesRemaining = 0;
    protected bool currentlyUsingAbility = false;


    protected void dealDamage(int power)
    {
        player.takeDamage(power);
    }
}