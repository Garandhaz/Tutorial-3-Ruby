using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectible : ItemAction
{
    public ParticleSystem keyEffect;
    public AudioClip collectedClip;


    //Custom Function when picked up
    public override void On_Pickup_Action(PlayerController controller)
    {
        //Singleton Reference
        GameModel model = GameModel.instance;
        model.PickUp_KeyItem();
        Instantiate(keyEffect, transform.position, Quaternion.identity);
        controller.PlaySound(collectedClip);
    }

}
