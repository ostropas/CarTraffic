using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WayPointSystem;

[RequireComponent(typeof(CarSight))]
public class CarMovement : MonoBehaviour
{
    #region Initialization
    void Awake()
    {
    }
    #endregion

    #region Public Fields
    public float MoveSpeed;
    public float CheckTurnDistance;
    public WayPoint StartWayPoint;
    public WayPointSystem WayPointSystem;
    public bool IsMoving;
    #endregion

    #region Private Fields
    private int _nextWayPointIndex = 1;
    private List<WayPoint> _path;
    private Vector3 _finalRotation;
    private Vector3 _startRotation;
    private float _rotationProgress;
    #endregion

    void Start()
    {
        _path = WayPointSystem.GetAllPathForWaypoint(StartWayPoint)[0];
        _rotationProgress = 0;
        _finalRotation = transform.eulerAngles;
        _startRotation = transform.eulerAngles;

        //var cs = GetComponent<CarSight>();
        //cs.TrafficLightDetected += CheckTrafficLight;
    }

    void Update()
    {
        if (IsMoving)
            MoveOverThePath();
    }

    #region Public Methods
    public void CheckTrafficLight(TrafficLight tl)
    {
        IsMoving = tl.CurrentAvailablePath == TrafficLight.AvailablePath.ZAvailable;
    }
    #endregion

    #region Private Methods
    private void MoveOverThePath()
    {
        // Move forward
        transform.position = Vector3.MoveTowards(transform.position, _path[_nextWayPointIndex].transform.position, MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _path[_nextWayPointIndex].transform.position) < CheckTurnDistance && _rotationProgress == 0f)
        {
            if (_nextWayPointIndex != _path.Count - 1)
            {
                var currentWayPoint = _path[_nextWayPointIndex];
                var nextWayPoint = _path[_nextWayPointIndex + 1];

                var diff = currentWayPoint.transform.position - nextWayPoint.transform.position;
                diff.Normalize();

                _finalRotation = new Vector3(0f, diff.x * -90f, 0f);
                _startRotation = transform.eulerAngles;
                _rotationProgress = 0;
            }
        }

        if (transform.eulerAngles != _finalRotation)
        {
            _rotationProgress += Time.deltaTime * 20;
            _rotationProgress = Mathf.Clamp01(_rotationProgress);
            transform.eulerAngles = Vector3.Lerp(_startRotation, _finalRotation, _rotationProgress);
        }

        if (transform.position == _path[_nextWayPointIndex].transform.position)
        {
            _nextWayPointIndex++;
            if (_nextWayPointIndex == _path.Count)
                Destroy(gameObject);
        }
    }
    #endregion
}
