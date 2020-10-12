using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject ZStop;
    public GameObject XStop;

    public IntVariable HoldDelay;

    private int _clickTime = 0;

    public void Update()
    {
        _clickTime++;

        if (Input.GetMouseButtonDown(0))
        {
            _clickTime = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Click must be not so long
            if (_clickTime < HoldDelay)
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
}
