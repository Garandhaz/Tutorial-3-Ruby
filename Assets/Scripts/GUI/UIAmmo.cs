using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmo : MonoBehaviour
{
    public static UIAmmo instance { get; private set; }
    public string text_prefix = "Ammo : ";
    public Text display;

    //Then in your Awake function (remember this is called as soon as the object is created, which is our case is when the game starts), you store in the static instance this
    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            instance = this;
        }
        // which is a special C# keyword that means “the object that currently runs that function”.
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateAmmo(0);
    }

    public void UpdateAmmo(int value)
    {
        display.text = text_prefix + value.ToString();
    }
}
