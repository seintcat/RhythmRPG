using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SaveDataUIButton : MonoBehaviour
{
    [SerializeField]
    private MenuButton button;
    [SerializeField]
    private TextMeshProUGUI saveDataText;

    public string text { set { saveDataText.text = value; } }
    public UnityEvent events
    {
        get => button.events;
        set { button.events = value; }
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
