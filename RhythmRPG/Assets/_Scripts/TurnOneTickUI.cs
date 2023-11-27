using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOneTickUI : MonoBehaviour
{
    [SerializeField]
    private Image guage;
    [SerializeField]
    private Image arrow;
    [SerializeField]
    private Sprite downArrow;
    [SerializeField]
    private Sprite upArrow;
    [SerializeField]
    private Sprite none;

    public float ratio
    {
        set
        {
            guage.fillAmount = value;
        }
    }
    public Color barColor
    {
        set
        {
            guage.color = value;
        }
    }
    public Color arrowColor
    {
        set
        {
            arrow.color = value;
        }
    }
    public bool push
    {
        set
        {
            arrow.sprite = value ? downArrow : upArrow;
        }
    }

    public void Initialize()
    {
        arrow.color = Color.white;
        guage.color = Color.white;
        arrow.sprite = none;
    }
}
