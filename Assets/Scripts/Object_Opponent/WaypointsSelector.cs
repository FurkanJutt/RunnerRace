using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsSelector : MonoBehaviour
{
    public static WaypointsSelector Instance;

    public Transform[] waypointGroups;

    public int groupIndex = 0;

    public Transform[] waypoints;

    // Define the index of the waypoint group you want to access.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        groupIndex = Random.Range(0, waypointGroups.Length);

        // Check if the groupIndex is within the valid range.
        if (groupIndex >= 0 && groupIndex < waypointGroups.Length)
        {
            // Access the specified waypoint group.
            Transform selectedWaypointGroup = waypointGroups[groupIndex];

            // Clear the waypoints array to start fresh.
            waypoints = new Transform[selectedWaypointGroup.childCount];

            // Now, you can access the transform positions of its children.
            for (int i = 0; i < selectedWaypointGroup.childCount; i++)
            {
                Transform childTransform = selectedWaypointGroup.GetChild(i);

                // Add the child position to the waypoints array.
                waypoints[i] = childTransform;
            }
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
}
