using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMover : MonoBehaviour
{
    public IntVariable HoldDelay;
    public float MoveSpeed;

    private Vector3 _cameraStartDragPosition;
    private Vector3 _mouseStartDragPosition;

    private Camera _camera;

    private int _holdTime;
    private bool _clicked;

    public void Awake()
    {
        _camera = Camera.main;
    }

    public void Update()
    {
        if (_clicked)
            _holdTime++;

        if (Input.GetMouseButtonDown(0))
        {
            _holdTime = 0;

            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, 5000, 1 << LayerMask.NameToLayer("Movement")))
            {
                _clicked = true;
                _mouseStartDragPosition = hit.point;
                _cameraStartDragPosition = transform.position;
            }
        }

        if (_clicked && _holdTime > HoldDelay)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, 5000, 1 << LayerMask.NameToLayer("Movement")))
            {
                var currentMousePosition = hit.point;
                _cameraStartDragPosition = transform.position;
                var offset = currentMousePosition - _mouseStartDragPosition;
                var newPos = transform.position - offset;
                newPos.y = _cameraStartDragPosition.y;

                transform.position = newPos;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _clicked = false;
        }
    }
}
