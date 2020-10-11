using Assets.Scripts.Utils;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static WayPointSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class CarMovement : MonoBehaviour
{
    #region Initialization
    void Awake()
    {
        _nva = GetComponent<NavMeshAgent>();
    }
    #endregion

    #region Public Fields
    public bool IsMoving;
    public IntVariable CurrentScene;
    public WaypointCollection Waypoints;
    #endregion

    #region Private Fields
    private Vector3 _destination;
    private NavMeshAgent _nva;
    #endregion


    void Start()
    {
        var point = Waypoints
            .Where(x => x.IsFinish && Vector3.Distance(transform.position, x.transform.position) > 20f)
            .GetRandomElement();
        _nva.SetDestination(point.transform.position);
        _destination = point.transform.position;
    }

    #region Public Methods
    public void ObjectDetected(GameObject go)
    {
        IsMoving = go == null;
        PauseMovement(!IsMoving);
    }
    #endregion

    #region Private Methods
    private void PauseMovement(bool isPaused)
    {
        if (isPaused)
        {
            _nva.isStopped = true;
            _nva.velocity = Vector3.zero;
        } else
        {
            _nva.isStopped = false;
            //var m = 9;
            //_nva.SetDestination(_destination);
        }
    }
    #endregion
}
