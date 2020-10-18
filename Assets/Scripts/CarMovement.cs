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
        _nva.speed *= _speedMul;
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
    private Coroutine _waitCoroutine;
    private System.Tuple<float, float> _minMaxWaitTime;
    private float _speedMul;
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

    public void SetSpeedMultiply(float speed)
    {
        _speedMul = speed;
    }

    public void ObjectDetected(bool isDetected)
    {
        IsMoving = !isDetected;
        PauseMovement(!IsMoving);
    }

    public void SetMinMaxWaitTime(float min, float max)
    {
        _minMaxWaitTime = new System.Tuple<float, float>(min, max);
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

    /// <summary>
    /// Pause nav mesh agent movement
    /// </summary>
    /// <param name="isPaused"></param>
    private void PauseMovement(bool isPaused)
    {
        if (_isPathComplete)
            return;

        if (isPaused)
        {
            StartWaitTimer();
            _nva.isStopped = true;
            _nva.velocity = Vector3.zero;
            _nva.angularSpeed = 0;
        }
        else
        {
            StopWaitTimer();
            this.Delay(Random.Range(0f, 1f), () =>
            {
                _nva.angularSpeed = _nvaAngularSpeed;
                _nva.isStopped = false;
            });
        }
    }

    private void StartWaitTimer()
    {
        StopWaitTimer();

        float time = Random.Range(_minMaxWaitTime.Item1, _minMaxWaitTime.Item2);

        _waitCoroutine = this.Delay(time, () =>
        {
            PauseMovement(false);
        });
    }

    private void StopWaitTimer()
    {
        if (_waitCoroutine != null)
        {
            StopCoroutine(_waitCoroutine);
            _waitCoroutine = null;
        }
    }

    /// <summary>
    /// Check is destination reached
    /// </summary>
    /// <returns>reached or not</returns>
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
