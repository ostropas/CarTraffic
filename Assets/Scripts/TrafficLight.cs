using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject ZStop;
    public GameObject XStop;
    public UnityEngine.Events.UnityEvent OnClick;

    public enum EnabledLight
    {
        Random,
        Invert
    }

    public List<SpriteRenderer> ZLights;
    public List<SpriteRenderer> XLights;

    public IntVariable HoldDelay;
    public bool SwitchOnStart = true;

    private int _clickTime = 0;

    private bool _trafficSwitched = true;

    public void Awake()
    {
        if (SwitchOnStart)
            SwitchStop(true, EnabledLight.Random); 
    }

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

                    if (objectHit.Equals(transform) && _trafficSwitched)
                    {
                        OnClick.Invoke();
                        SwitchStop(false, EnabledLight.Invert);
                    }
                }
            }
        }
    }

    private void SwitchStop(bool force, EnabledLight activeAxis)
    {
        bool zActive;
        if (activeAxis == EnabledLight.Invert)
        {
            zActive = !ZStop.activeSelf;
        }
        else
        {
            zActive = Random.Range(0, 2) == 0;
        }

        if (force)
        {
            SetTraffic(zActive);
            ZStop.SetActive(zActive);
            XStop.SetActive(!zActive);

            SetColor(ZLights, zActive ? Color.red : Color.green);
            SetColor(XLights, zActive ? Color.green : Color.red);
        } else
        {
            _trafficSwitched = false;
            ZStop.SetActive(true);
            XStop.SetActive(true);

            SetColor(ZLights, Color.yellow);
            SetColor(XLights, Color.yellow);

            StartCoroutine(SetYellowTraffic(() => SetTraffic(zActive)));
        }
    }

    private IEnumerator SetYellowTraffic(System.Action callback)
    {
        yield return new WaitForSeconds(1f);
        callback.Invoke();
        _trafficSwitched = true;
    }

    private void SetTraffic(bool zActive)
    {
        ZStop.SetActive(zActive);
        XStop.SetActive(!zActive);

        SetColor(ZLights, zActive ? Color.red : Color.green);
        SetColor(XLights, zActive ? Color.green : Color.red);
    }

    private void SetColor(List<SpriteRenderer> lights, Color col)
    {
        foreach (var light in lights)
        {
            light.color = col;
        }
    }
}
