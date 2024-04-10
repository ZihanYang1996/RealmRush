using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    Waypoint waypoint;

    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;

    void Awake()
    {
        label = gameObject.GetComponent<TextMeshPro>();
        label.enabled = false;  // Default to false
        waypoint = gameObject.GetComponentInParent<Waypoint>();
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
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        // Set the label text to the coordinates
        label.text = $"{coordinates.x},{coordinates.y}";
    }

    private void DisplayCoordinatesColor()
    {
        // Set the color of the label based on the waypoint's IsPlaceable property
        if (waypoint.IsPlaceable)
        {
            label.color = defaultColor;
        }
        else
        {
            label.color = blockedColor;
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
            Debug.Log("Toggle labels");
            label.enabled = !label.enabled;  // Or label.enabled = !label.IsActive();
        }
    }
}
