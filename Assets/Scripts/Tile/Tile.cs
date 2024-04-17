using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class Tile : MonoBehaviour, IPointerClickHandler
{
    InputAction mousePosition;
    InputAction mouseClick;

    GameObject towers;

    [SerializeField] bool isPlaceable = false;
    public bool IsPlaceable { get { return isPlaceable; } }
    [SerializeField] GameObject towerPrefab;

    GridManager gridManager;
    Vector2Int coordinates;

    void Awake()
    {
        // Using the default action asset (InputSystem.actions)
        mouseClick = InputSystem.actions.FindActionMap("UI").FindAction("Click");
        mousePosition = InputSystem.actions.FindActionMap("Player").FindAction("MousePosition");
        towers = GameObject.Find("Towers");
        gridManager = FindObjectOfType<GridManager>();

        // Get the coordinates of the tile and set the corresponding node's isWalkable property based on the isPlaceable property
        coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        if (gridManager.Grid.ContainsKey(coordinates))
        {
            gridManager.Grid[coordinates].isWalkable = isPlaceable;
        }
    }

    void Start()
    {

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

            // Update the isWalkable property of the corresponding node in the grid
            if (gridManager.Grid.ContainsKey(coordinates))
            {
                gridManager.Grid[coordinates].isWalkable = isPlaceable;
            }
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
