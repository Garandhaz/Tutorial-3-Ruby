using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : ItemAction
{
    public ParticleSystem healthEffect;
    public AudioClip collectedClip;

    public int healingPower = 1;

    //Conditional, If allowed to be picked up.
    public override bool On_Pickup_Condition(PlayerController controller)
    {
        //Singleton Reference
        GameModel model = GameModel.instance;

        //If player health not maxed, return true.
        return (model.player_health < model.player_maxhealth);
    }

    //Custom Function when picked up
    public override void On_Pickup_Action(PlayerController controller)
    {
        controller.ChangeHealth(healingPower);
        Instantiate(healthEffect, transform.position, Quaternion.identity);
        controller.PlaySound(collectedClip);
    }

}
