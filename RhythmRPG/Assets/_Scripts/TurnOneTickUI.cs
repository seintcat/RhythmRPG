using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOneTickUI : MonoBehaviour
{
    [SerializeField]
    private Image guage;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
