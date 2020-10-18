using Assets.Scripts.Utils;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarMovement : MonoBehaviour
{
    #region Initialization
    #endregion

    public IEnumerator Start()
    {
        _nva = GetComponent<NavMeshAgent>();
        _nvaAngularSpeed = _nva.angularSpeed;
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            t += Time.deltaTime * 5;
            yield return null;
        }

        transform.localScale = Vector3.one;

        if (_destination.HasValue)
            _nva.SetDestination(_destination.Value);
    }

    #region Public Fields
    public bool IsMoving;
    public IntVariable CurrentScene;
    public WaypointCollection Waypoints;
    public GameObjectCollection InstantiatedCars;
    public GameEvent CrashEvent;
    #endregion

    #region Private Fields
    private NavMeshAgent _nva;
    private float _nvaAngularSpeed;
    private Vector3? _destination;
    private bool _isPathComplete;
    #endregion

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Car"))
            CrashEvent.Raise();
    }

    #region Public Methods
    public void SetDestination(Vector3 destination)
    {
        _destination = destination;
    }
    public void ObjectDetected(bool isDetected)
    {
        IsMoving = !isDetected;
        PauseMovement(!IsMoving);
    }
    #endregion

    #region Private Methods
    private void OnEnable()
    {
        InstantiatedCars.Add(gameObject);
    }

    private void OnDisable()
    {
        InstantiatedCars.Remove(gameObject);
    }

    private void Update()
    {
        if (PathComplete())
        {
            PauseMovement(true);
            _isPathComplete = true;

            StartCoroutine(DestroyCoroutine());
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            t += Time.deltaTime * 5;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void PauseMovement(bool isPaused)
    {
        if (_isPathComplete)
            return;

        if (isPaused)
        {
            _nva.isStopped = true;
            _nva.velocity = Vector3.zero;
            _nva.angularSpeed = 0;
        }
        else
        {
            _nva.angularSpeed = _nvaAngularSpeed;
            _nva.isStopped = false;
        }
    }

    private bool PathComplete()
    {
        if (Vector3.Distance(_destination.Value, _nva.transform.position) <= 1f)
        {
            return true;
        }

        return false;
    }
    #endregion
}
