using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    //created OnTriggerStay2D function calling on the parameter of function Collider2D and variable other when player collider enters and stays
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
            //Access the RubyController component on the game object of the collider that enters the trigger
            //RubyController component and variable controller store the result of variable other containing the function GetComponent fetching RubyController from triggering object

        if (controller != null)
            //if the variable controller is not null(nothing/empty)
            //if triggering object does not have RubyController component, it returns null
        {
            controller.ChangeHealth(-1);
             //variable controller contains change health function calling the parameter -1
        }
    }
}
