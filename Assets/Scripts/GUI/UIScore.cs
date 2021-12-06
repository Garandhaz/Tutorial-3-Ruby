using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    public static UIScore instance { get; private set; }
    public string text_prefix = "Robots Fixed : ";
    public Text display;
    public Vector2 snapPosition = new Vector2(0.43f, -2f);
    Vector2 originalPosition;

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
        RectTransform rect_transform = GetComponent<RectTransform>();
        originalPosition = rect_transform.pivot;
        UpdateScore(0);
    }

    public void UpdateScore(int value)
    {
        //Singleton References
        LevelInfo ui_level = LevelInfo.instance;

        display.text = text_prefix + value.ToString() + "/" + ui_level.objective_count.ToString();
    }

    public void SnapRight()
    {
        RectTransform rect_transform = GetComponent<RectTransform>();
        rect_transform.pivot = snapPosition;
    }

    public void ResetPos()
    {
        RectTransform rect_transform = GetComponent<RectTransform>();
        rect_transform.pivot = originalPosition;
    }
}
