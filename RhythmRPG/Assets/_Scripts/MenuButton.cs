using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public UnityEvent events;
    public RectTransform rectTransform;

    private int index;
    private MenuHandler menuHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(int _index, MenuHandler _menuHandler)
    {
        index = _index;
        menuHandler = _menuHandler;
    }

    public void Clicked() => menuHandler.ClickButton(index);
}
