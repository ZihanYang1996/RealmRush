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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPlaceable) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
            tower.transform.SetParent(towers.transform); // or tower.transform.parent = towers.transform;
            isPlaceable = false;
        }
    }
}
