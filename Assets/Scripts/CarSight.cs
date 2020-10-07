using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CarSight : MonoBehaviour
{
    public UnityEvent<GameObject> ObjectInView;

    private GameObject _objectInView = null;

    public float ViewDistance = 10f;
    public float ViewOffset = 1f;

    private void Awake()
    {
        //TrafficLightDetected = new Action<TrafficLight>()
    }


    private void Update()
    {
        DetectObjects();
    }

    //Detect perspective field of view for the AI Character
    void DetectObjects()
    {
        if (Physics.Raycast(transform.position + transform.forward * ViewOffset, transform.forward, out var hit, ViewDistance) && !hit.collider.isTrigger)
        {
            if (!_objectInView)
            {
                ObjectInView.Invoke(hit.transform.gameObject);
                _objectInView = hit.transform.gameObject;
            }
        } else
        {
            if (_objectInView)
            {
                ObjectInView.Invoke(null);
                _objectInView = null;
            }
        }
        
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        DrawLine(transform.position + transform.forward * ViewOffset, transform.position + transform.forward * ViewDistance + transform.forward * ViewOffset, 4);
    }

    public static void DrawLine(Vector3 p1, Vector3 p2, float width)
    {
        int count = Mathf.CeilToInt(width); // how many lines are needed.
        if (count == 1)
            Gizmos.DrawLine(p1, p2);
        else
        {
            Camera c = Camera.current;
            if (c == null)
            {
                Debug.LogError("Camera.current is null");
                return;
            }
            Vector3 v1 = (p2 - p1).normalized; // line direction
            Vector3 v2 = (c.transform.position - p1).normalized; // direction to camera
            Vector3 n = Vector3.Cross(v1, v2); // normal vector
            for (int i = 0; i < count; i++)
            {
                Vector3 o = n * ((float)i / (count - 1) - 0.5f);
                Gizmos.DrawLine(p1 + o, p2 + o);
            }
        }
    }
}
