using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public static LevelInfo instance { get; private set; } //Singleton

    public string mapName; // Name of Map

    public string nextLevel; // Next Scene (Prefab)
    public bool finalLevel; // Is final level?

    public int objective_count;

    public string starting_text;
    public string objective_text; //Text When
    public string victory_text; //Text When Key Item is picked up

    public GameObject victory_item; //Key Item (Prefab)

    //Storing Static Instance , Singleton
    void Awake()
    {
        // Check if singleton exists already.
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
}
