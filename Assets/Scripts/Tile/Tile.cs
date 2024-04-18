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
    PathFinder pathFinder;
    Vector2Int coordinates;

    void Awake()
    {
        // Using the default action asset (InputSystem.actions)
        mouseClick = InputSystem.actions.FindActionMap("UI").FindAction("Click");
        mousePosition = InputSystem.actions.FindActionMap("Player").FindAction("MousePosition");
        towers = GameObject.Find("Towers");
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();

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
        if (!isPlaceable || !gridManager.Grid[coordinates].isWalkable)  // If the tile is not placeable or the corresponding node is not walkable, return
        {
            Debug.Log("Cannot place tower here, is it Placeable? " + isPlaceable + " isWalkable? " + gridManager.Grid[coordinates].isWalkable);
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // First, try create the tower
            GameObject towerCreated = towerPrefab.GetComponent<Tower>().CreateTower(towerPrefab,
                                                                        transform.position,
                                                                        Quaternion.identity,
                                                                        towers);


            // If the tower was created, meaning there are sufficient funds and the tower was placed successfully
            if (towerCreated)
            {
                // Check if all paths are blocked after placing the tower
                gridManager.Grid[coordinates].isWalkable = false;
                bool allPathsBlocked = !pathFinder.generateDefaultPath();
                // If all paths are blocked, set the isWalkable back to true, destroy the tower and return
                if (allPathsBlocked)
                {
                    gridManager.Grid[coordinates].isWalkable = true;
                    towerCreated.GetComponent<Tower>().DestroyTower(refound: true);
                    Debug.Log("Cannot place tower here, all paths are blocked.");
                    return;
                }
                // Otherwise, the tower was placed successfully, set isPlaceable to false
                isPlaceable = false;
                // Send a message to the EventManager to ask enemys to recreate the path
                EventManager.Instance.SendRecreatePathMessage(initialPath: false);
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
