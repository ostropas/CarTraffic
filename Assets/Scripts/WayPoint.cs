using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WaypointCollection WayPoints;
    public bool IsFinish;

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    private void OnEnable()
    {
        WayPoints.Add(this);
    }

    private void OnDisable()
    {
        WayPoints.Remove(this);
    }
}
