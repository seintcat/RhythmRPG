using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset actionAsset;
    [SerializeField]
    private List<string> holdAbleActionNames;
    [SerializeField]
    private List<string> clickActionNames;
    [SerializeField]
    private string defaultActionMap;

    private static Dictionary<string, bool> buttons = new Dictionary<string, bool>();
    public static Dictionary<string, List<Action>> holdAbleOnEvent = new Dictionary<string, List<Action>>();
    public static Dictionary<string, List<Action>> holdAbleOffEvent = new Dictionary<string, List<Action>>();
    public static Dictionary<string, List<Action>> clickEvent = new Dictionary<string, List<Action>>();

    private void Awake()
    {
        actionAsset.Enable();
        InputActionMap map = actionAsset.FindActionMap(defaultActionMap);
        map.Enable();

        foreach (string button in holdAbleActionNames)
        {
            buttons.Add(button, false);
            map.FindAction(button).performed += (x) => HoldAbleButton(button, x);
            map.FindAction(button).canceled += (x) => HoldAbleButton(button, x);
        }

        foreach (string button in clickActionNames)
        {
            buttons.Add(button, false);
            map.FindAction(button).performed += (x) => ClickButton(button, x);
            map.FindAction(button).canceled += (x) => ClickButton(button, x);
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

    private void LateUpdate()
    {
        foreach(string button in clickActionNames)
            if (buttons.ContainsKey(button))
                buttons[button] = false;
    }

    public void HoldAbleButton(string button, InputAction.CallbackContext context)
    {
        buttons[button] = context.ReadValue<float>() > 0.5f;

        if (buttons[button] && holdAbleOnEvent.ContainsKey(button))
            foreach(Action action in holdAbleOnEvent[button])
                action.Invoke();
        else if (!buttons[button] && holdAbleOffEvent.ContainsKey(button))
            foreach (Action action in holdAbleOffEvent[button])
                action.Invoke();
    }
    public void ClickButton(string button, InputAction.CallbackContext context)
    {
        buttons[button] = context.ReadValue<float>() > 0.5f;

        if (buttons[button] && clickEvent.ContainsKey(button))
            foreach (Action action in clickEvent[button])
                action.Invoke();
    }
}
