using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : ItemAction
{
    public ParticleSystem ammoEffect;
    public AudioClip collectedClip;
    
    //Custom Function when picked up
    public override void On_Pickup_Action(PlayerController controller)
    {
        controller.ChangeAmmo(4);
        Instantiate(ammoEffect, transform.position, Quaternion.identity);
        controller.PlaySound(collectedClip);
    }

}
