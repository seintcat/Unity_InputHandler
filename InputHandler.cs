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
    public static Dictionary<string, Action> holdAbleOnEvent = new Dictionary<string, Action>();
    public static Dictionary<string, Action> holdAbleOffEvent = new Dictionary<string, Action>();
    public static Dictionary<string, Action> clickEvent = new Dictionary<string, Action>();

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
        foreach (string button in clickActionNames)
            if (buttons.ContainsKey(button))
                buttons[button] = false;
    }

    public void HoldAbleButton(string button, InputAction.CallbackContext context)
    {
        buttons[button] = context.ReadValue<float>() > 0.5f;

        if (buttons[button] && holdAbleOnEvent.ContainsKey(button))
            holdAbleOnEvent[button].Invoke();
        else if (!buttons[button] && holdAbleOffEvent.ContainsKey(button))
            holdAbleOffEvent[button].Invoke();
    }
    public void ClickButton(string button, InputAction.CallbackContext context)
    {
        buttons[button] = context.ReadValue<float>() > 0.5f;

        if (buttons[button] && clickEvent.ContainsKey(button))
            clickEvent[button].Invoke();
    }
}
