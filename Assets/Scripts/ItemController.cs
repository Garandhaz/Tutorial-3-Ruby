using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemController : MonoBehaviour
{
  public CircleCollider2D detectionRadius;
  public ItemAction action;
  bool has_action;
  public float pickup_Time = 0.5f;
  float pickupTimer;
  bool isFollowing;

  Vector3 oldposition;

  // Start is called before the first frame update
  void Start()
  {
    has_action = (action != null);
    isFollowing = false;
  }

  // Update is called once per frame
  void Update()
  {
    //Check if Following. Follow if so.
    if (isFollowing)
    {
      Follow();
    }
  }

  void StartFollow()
  {
    //Store Old Position Incase player fails conditional on pickup.
    oldposition = transform.position;
    //Stop Detecting Radius
    detectionRadius.enabled = false;
    //Reset Timer and Start Following.
    pickupTimer = 0;
    isFollowing = true;

    //if action then On_Pickup_Follow
    if (has_action)
    {
      //Singleton Reference
      PlayerController controller = PlayerController.instance;
      action.On_Pickup_Follow(controller);
    }
  }

  void StopFollow()
  {
    //Restore old position
    transform.position = oldposition;
    //Start Detecting Radius
    detectionRadius.enabled = true;
    //Stop Following.
    isFollowing = false;

    //if action then On_Pickup_Stop_Follow
    if (has_action)
    {
      //Singleton Reference
      PlayerController controller = PlayerController.instance;
      action.On_Pickup_Stop_Follow(controller);
    }
  }

  void Follow()
  {
    //Singleton Reference
    PlayerController controller = PlayerController.instance;

    //Pickup Conditional
    bool pickup_allowed = true;

    //Set pickup_allowed with action conditional
    if (has_action)
    {
      pickup_allowed = action.On_Pickup_Condition(controller);
    }

    //Check if pickup_allowed
    if (pickup_allowed)
    {
      pickupTimer += Time.deltaTime;
      float position_ratio = pickupTimer / pickup_Time;
      transform.position = Vector3.Lerp(transform.position, controller.transform.position, position_ratio);
    }
    else
    {
      StopFollow();
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    //Singleton Reference
    PlayerController controller = PlayerController.instance;

    //Pickup Conditional
    bool pickup_allowed = true;

    //Set pickup_allowed with action conditional
    if (has_action)
    {
      pickup_allowed = action.On_Pickup_Condition(controller);
    }

    //Check if collider is player controller
    if (other.gameObject == controller.gameObject)
    {
      //If not following
      if (!isFollowing)
      {
        //if pickup_allowed then start follow.
        if (pickup_allowed)
        {
          StartFollow();
        }
      }
      //IS following
      else
      {
        //if pickup_allowed then stop follow.
        if (pickup_allowed)
        {
          StopFollow();
          //If has_action On_Pickup_Action
          if (has_action)
          {
            transform.position = controller.transform.position;
            action.On_Pickup_Action(controller);
          }
          //Destroy this gameobject.
          Destroy(gameObject);
        }
        else
        {
          //Conditional Failed, Reset to Starting Position.
          StopFollow();
        }
      }
    }
  }
}
