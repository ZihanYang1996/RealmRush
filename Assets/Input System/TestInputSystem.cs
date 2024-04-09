using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputSystem : MonoBehaviour
{
    public InputActionAsset testInputActionAsset;
    InputAction actionA;
    InputAction actionB;
    void Start()
    {
    }

    void OnEnable()
    {
        actionA = testInputActionAsset.FindActionMap("MapA").FindAction("mousePosition");
        actionB = testInputActionAsset.FindActionMap("MapB").FindAction("mousePosition");
        // Enable the action
        // actionA.Enable();
        // actionB.Enable();
        
        // Enable the action map
        // testInputActionAsset.FindActionMap("MapA").Enable();
        // testInputActionAsset.FindActionMap("MapB").Enable();

        // Enable the entire asset
        testInputActionAsset.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = actionA.ReadValue<Vector2>();
        Debug.Log("Test: " + mousePosition);
        if (actionB.IsPressed())
        {
            Debug.Log("Action B is pressed");
        }
    }
}
