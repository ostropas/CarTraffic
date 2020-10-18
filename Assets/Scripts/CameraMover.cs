using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public IntVariable HoldDelay;
    public Transform MinPos;
    public Transform MaxPos;

    private Vector3 _cameraStartDragPosition;
    private Vector3 _mouseStartDragPosition;

    private Camera _camera;

    private int _holdTime;
    private bool _clicked;
    private bool _isBlock;

    public void GameFinished()
    {
        _isBlock = true;
    }

    public void Awake()
    {
        _camera = Camera.main;
    }

    public void Update()
    {
        if (_isBlock)
            return;

        // Delay before start drag
        if (_clicked)
            _holdTime++;

        if (Input.GetMouseButtonDown(0))
        {
            _holdTime = 0;

            // Detect start drag pos
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, 5000, 1 << LayerMask.NameToLayer("Movement")))
            {
                _clicked = true;
                _mouseStartDragPosition = hit.point;
                _cameraStartDragPosition = transform.position;
            }
        }

        // Drag
        if (_clicked && _holdTime > HoldDelay)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, 5000, 1 << LayerMask.NameToLayer("Movement")))
            {
                var currentMousePosition = hit.point;
                _cameraStartDragPosition = transform.position;
                var offset = currentMousePosition - _mouseStartDragPosition;
                var newPos = transform.position - offset;
                newPos.y = _cameraStartDragPosition.y;

                newPos.x = Mathf.Clamp(newPos.x, MinPos.position.x, MaxPos.position.x);
                newPos.z = Mathf.Clamp(newPos.z, MinPos.position.z, MaxPos.position.z);

                transform.position = newPos;
            }
        }

        // End drag
        if (Input.GetMouseButtonUp(0))
        {
            _clicked = false;
        }
    }
}
