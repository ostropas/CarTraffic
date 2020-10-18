using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CarSight : MonoBehaviour
{
    [Serializable]
    public class ViewParam
    {
        public float Angle;
        public float Offset;
    }

    public UnityEvent<bool> ObjectInView;

    private GameObject _objectInView = null;

    public float ViewDistance = 10f;
    public float ViewOffset = 1f;

    public List<ViewParam> ViewRays;

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
        var rays = ViewRays.Select(x =>
        {
            var view = Quaternion.Euler(0, x.Angle, 0) * transform.forward;

            var rightVec = transform.right;
            rightVec *= x.Offset;

            var ray = new Ray(transform.position + rightVec + transform.forward * ViewOffset, view);
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            return ray;
        }).ToList();

        foreach (var ray in rays)
        {
            if (Physics.Raycast(ray, out var hit, ViewDistance))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.green);

                if (hit.collider.isTrigger)
                    continue;

                ObjectInView.Invoke(true);
                return;
            } 
        }

        ObjectInView.Invoke(false);
    }
}
