using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    Tile waypoint;

    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = Color.red;

    GridManager gridManager;
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = gameObject.GetComponent<TextMeshPro>();
        waypoint = gameObject.GetComponentInParent<Tile>();
        if (Application.isPlaying)
        {
            label.enabled = false;  // Default to false in play mode
        }
        else
        {
            label.enabled = true;  // Default to true in edit mode
        }

        DisplayCoordinates();
        UpdateObjectName();
        DisplayCoordinatesColor();
    }
    void Update()
    {
        if (!Application.isPlaying)  // This will only run in the editor
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
        DisplayCoordinatesColor();
        ToggleLabels();
    }

    void DisplayCoordinates()
    {
        // Set the coordinates of the label to the parent's position divided by the editor snap settings
        // Only when in the editor
#if UNITY_EDITOR
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
#else
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z);
#endif

        // Set the label text to the coordinates
        label.text = $"{coordinates.x},{coordinates.y}";
    }

    private void DisplayCoordinatesColor()
    {
        // // Set the color of the label based on the node's properties
        if (gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);
        if (node == null) { return; }


        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            // Set the color of the label based on the waypoint's IsPlaceable property first
            if (waypoint.IsPlaceable)
            {
                label.color = defaultColor;
            }
            else
            {
                label.color = blockedColor;
            }
        }

    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    void ToggleLabels()
    {
        if (Keyboard.current[Key.C].wasPressedThisFrame)
        {
            label.enabled = !label.enabled;  // Or label.enabled = !label.IsActive();
        }
    }
}
