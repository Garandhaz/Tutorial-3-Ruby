using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }

    public Image mask;
    
    float originalSize;

    //Then in your Awake function (remember this is called as soon as the object is created, which is our case is when the game starts), you store in the static instance this
    void Awake()
    {
        instance = this;
            // which is a special C# keyword that means “the object that currently runs that function”.
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
