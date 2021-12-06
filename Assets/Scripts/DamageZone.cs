using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    //created OnTriggerStay2D function calling on the parameter of function Collider2D and variable other when player collider enters and stays
    void OnTriggerStay2D(Collider2D other)
    {
        //Singleton Reference
        PlayerController controller = PlayerController.instance;

        //Check if collider is PlayerController gameobject
        if(other.gameObject == controller.gameObject)
        {
            controller.ChangeHealth(-1);
        }
    }
}
