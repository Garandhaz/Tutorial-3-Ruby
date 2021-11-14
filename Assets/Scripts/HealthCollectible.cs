using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public ParticleSystem healthEffect;
    public AudioClip collectedClip;

    //created OnTriggerEnter2D function calling on the parameter of function Collider2D and variable other when player collider enters
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
            //Access the RubyController component on the game object of the collider that enters the trigger
            //RubyController component and variable controller store the result of variable other containing the function GetComponent fetching RubyController from triggering object

        if(controller != null)
            //if the variable controller is not null(nothing/empty)
            //if triggering object does not have RubyController component, it returns null
        {
            if(controller.health < controller.maxHealth)
                //if variable controller containing health is less than variable controller containing maxHealth
                //used to check to make sure that the object isnt deleted when ruby is at full health
            {
                controller.ChangeHealth(1);
                    //variable controller contains change health function calling the parameter 1

                Instantiate(healthEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
                    //the Destroy function is built into unity. it will Destroy the game object this script is attached to
                    //Destroy function contaning the gameObject

                controller.PlaySound(collectedClip);
            }
        }        
    }
}
