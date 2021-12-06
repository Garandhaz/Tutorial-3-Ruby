using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuide : MonoBehaviour
{
    public static UIGuide instance { get; private set; }
    public GameObject content;
    bool isactive = true;
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        UIScore ui_score = UIScore.instance;

        isactive = !isactive;
        content.SetActive(isactive);
        if (isactive)
        {
            ui_score.ResetPos();
        }
        else
        {
            ui_score.SnapRight();
        }
    }
}
