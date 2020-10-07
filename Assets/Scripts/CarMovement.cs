using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static WayPointSystem;

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
    public IntVariable CurrentScene;
    #endregion

    #region Private Fields
    private int _nextWayPointIndex = 1;
    private List<Vector3> _path;
    private Vector3 _finalRotation;
    private Vector3 _startRotation;
    private float _rotationProgress;
    private int _curLine = 0;
    private float _lineWidth = 2.5f;
    #endregion

    NavMeshAgent _nva;

    void Start()
    {
        //var path = WayPointSystem.GetRandomPath(StartWayPoint);

        //_path = new List<Vector3>();

        //for (int i = 0; i < path.Count; i++)
        //{
        //    _path.Add(path[i].Start.transform.position);
        //}
        //_path.Add(path.Last().Finish.transform.position);

        //_rotationProgress = 0;
        //_finalRotation = transform.eulerAngles;
        //_startRotation = transform.eulerAngles;



        //var startPos = _path[0];

        //startPos += transform.right * _curLine * _lineWidth;

        //transform.position = startPos;

        _nva = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 1000f))
            {
                _nva.SetDestination(hit.point);
                var path = new NavMeshPath();
                //path.corners
                //NavMesh.CalculatePath()                    
            }
        } 

        return;

        if (IsMoving)
            MoveOverThePath();
    }

    #region Public Methods
    public void ObjectDetected(GameObject go)
    {
        IsMoving = go == null;
    }
    #endregion

    private Vector3? _nextPos;
    #region Private Methods
    private void MoveOverThePath()
    {
        if (_nextPos == null)
        {
            _nextPos = _path[_nextWayPointIndex];
            _nextPos -= transform.forward * _curLine * _lineWidth;
            _nextPos += transform.right * _curLine * _lineWidth;
        }

        var nextPos = _nextPos.Value;

        // Move forward
        transform.position = Vector3.MoveTowards(transform.position, nextPos, MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextPos) < CheckTurnDistance && _rotationProgress == 0f)
        {
            if (_nextWayPointIndex != _path.Count - 1)
            {
                var currentWayPoint = nextPos;

                var nextWayPoint = _path[_nextWayPointIndex + 1];
                var xDiff = Mathf.Abs(currentWayPoint.x - nextWayPoint.x);
                var zDiff = Mathf.Abs(currentWayPoint.z - nextWayPoint.z);

                if (xDiff > zDiff)
                {
                    nextWayPoint.z = currentWayPoint.z;
                }
                else
                {
                    nextWayPoint.x = currentWayPoint.x;
                }

                var diff = currentWayPoint - nextWayPoint;
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

        if (transform.position == nextPos)
        {
            _nextWayPointIndex++;
            _nextPos = null;
            if (_nextWayPointIndex == _path.Count)
                Destroy(gameObject);
        }
    }
    #endregion
}
