using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    public static UIDialogue instance { get; private set; }
    private float timerDisplay = 0;
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

    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                Hide();
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Show(float time)
    {
        timerDisplay = time;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        timerDisplay = -1;
        gameObject.SetActive(false);
    }

    public void SetText(string value)
    {
        display.text = value;
    }
}
