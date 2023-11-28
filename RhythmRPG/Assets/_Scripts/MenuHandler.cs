using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField]
    private RectTransform selector;
    [SerializeField]
    private List<MenuButton> buttons;

    private int indexNow;

    private void Awake()
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            buttons[i].index = i;
        }

        indexNow = 0;
        CheckButtonSelected(indexNow);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickButton(int index)
    {
        if (CheckButtonSelected(index))
        {
            buttons[index].events.Invoke();
        }
    }

    private bool CheckButtonSelected(int index)
    {
        if (!gameObject.activeSelf)
            return false;

        if (indexNow == index)
            return true;

        indexNow = index;

        if (indexNow < 0)
            indexNow = buttons.Count - 1;
        if (indexNow >= buttons.Count)
            indexNow = 0;

        selector.SetParent(buttons[indexNow].rectTransform);
        selector.anchorMin = new Vector2(0, 0);
        selector.anchorMax = new Vector2(1, 1);
        selector.offsetMin = new Vector2(0, 0);
        selector.offsetMax = new Vector2(1, 1);
        Debug.Log(selector.offsetMin + ", " + selector.offsetMax);
        return false;
    }

    public void SelectMenu() => buttons[indexNow].events.Invoke();

    private void SetEvent()
    {
        if (InputHandler.buttonDownEvent.ContainsKey("Up"))
            InputHandler.buttonDownEvent["Up"].Add(() => CheckButtonSelected(indexNow - 1));
        else
            InputHandler.buttonDownEvent.Add("Up", new List<System.Action>() { () => CheckButtonSelected(indexNow - 1) });

        if (InputHandler.buttonDownEvent.ContainsKey("Down"))
            InputHandler.buttonDownEvent["Down"].Add(() => CheckButtonSelected(indexNow + 1));
        else
            InputHandler.buttonDownEvent.Add("Down", new List<System.Action>() { () => CheckButtonSelected(indexNow + 1) });

        if (InputHandler.buttonDownEvent.ContainsKey("Select"))
            InputHandler.buttonDownEvent["Select"].Add(() => SelectMenu());
        else
            InputHandler.buttonDownEvent.Add("Select", new List<System.Action>() { () => SelectMenu() });
    }
}
