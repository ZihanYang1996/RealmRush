using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class Waypoint : MonoBehaviour, IPointerClickHandler
{
    InputAction mousePosition;
    InputAction mouseClick;

    GameObject towers;

    [SerializeField] bool isPlaceable = false;
    public bool IsPlaceable { get { return isPlaceable; } }
    [SerializeField] GameObject towerPrefab;

    void Start()
    {
        // Using the default action asset (InputSystem.actions)
        mouseClick = InputSystem.actions.FindActionMap("UI").FindAction("Click");
        mousePosition = InputSystem.actions.FindActionMap("Player").FindAction("MousePosition");
        towers = GameObject.Find("Towers");
    }

    void Update()
    {
        // OnMouseOverRaycast();
        // getMousePosition();
        // getMouseClick();
        // Debug.Log(mouseClick.triggered);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPlaceable) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isPlaceable = towerPrefab.GetComponent<Tower>().CreateTower(towerPrefab,
                                                                        transform.position,
                                                                        Quaternion.identity,
                                                                        towers);
            isPlaceable = !isPlaceable;
        }
    }

    void OnMouseOverRaycast()
    {
        if (mouseClick.triggered)
        {
            Vector2 mousePos = mousePosition.ReadValue<Vector2>();
            // Debug.Log(mousePos);

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log(gameObject.name);
                }
            }
        }

    }

    void getMousePosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Debug.Log(mousePos);
    }

    void getMouseClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left mouse button was pressed");
        }
    }


}
