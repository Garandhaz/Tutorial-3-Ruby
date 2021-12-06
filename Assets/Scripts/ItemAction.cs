using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAction : MonoBehaviour
{
  //Default Function On_Pickup_Condition
  public virtual bool On_Pickup_Condition(PlayerController controller)
  {
    return true;
  }

  //Default Function On_Pickup_Follow
  public virtual void On_Pickup_Follow(PlayerController controller) { }

  //Default Function On_Pickup_Stop_Follow
  public virtual void On_Pickup_Stop_Follow(PlayerController controller) { }


  //Default Function On_Pickup_Action
  public virtual void On_Pickup_Action(PlayerController controller) { }
}
