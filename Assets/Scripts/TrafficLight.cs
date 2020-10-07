using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject ZStop;
    public GameObject XStop;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 5000f, 1 << LayerMask.NameToLayer("TrafficLight")))
            {
                Transform objectHit = hit.transform;

                if (objectHit.Equals(transform))
                {
                    var zActive = ZStop.activeSelf;
                    ZStop.SetActive(!zActive);
                    XStop.SetActive(zActive);
                }
            }
        }

    }
}
