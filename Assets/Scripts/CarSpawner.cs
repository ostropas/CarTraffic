using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using Assets.Scripts.Utils;
using System.Linq;

[RequireComponent(typeof(ColliderCount))]
public class CarSpawner : MonoBehaviour
{
    public List<Transform> GenerateAtPoints;
    public GameObjectCollection AvailableCarPrefabs;
    public WaypointCollection Waypoints;
    public GameObjectCollection CarSpawners;

    public float FirstCarDelay;
    public float SpawnDelay;
    public float CarsCount;

    private int _carsSpawned;
    private float _currentTime;
    private float _prevSpawnCarTime = float.MinValue;
    private ColliderCount _colliderCount;
    private bool _gameFinished = false;

    public float MinCarWaitTime = 5;
    public float MaxCarWaitTime = 10;
    public float SpeedMultily = 1;

    public void Start()
    {
        _currentTime = 0;
        _colliderCount = GetComponent<ColliderCount>();
    }

    public void OnEnable()
    {
        CarSpawners.Add(gameObject);
    }

    public void OnDisable()
    {
        CarSpawners.Remove(gameObject);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_gameFinished)
            return;

        if (FirstCarDelay > _currentTime)
            return;

        // If spawned enough cars
        if (_carsSpawned >= CarsCount)
        {
            CarSpawners.Remove(gameObject);
            return;
        }

        // If not enough space to spawn car
        if (_colliderCount.Collisions.Count >= 1)
            return;

        if (_prevSpawnCarTime < _currentTime - SpawnDelay)
        {
            GenerateCar();
            _prevSpawnCarTime = _currentTime;
            _carsSpawned++;
        }
    }

    private void GenerateCar()
    {
        var carPrefab = AvailableCarPrefabs.GetRandomElement();
        var spawnPos = GenerateAtPoints.GetRandomElement();

        // Select random far point
        var points = Waypoints
            .Where(x => x.IsFinish && Vector3.Distance(transform.position, x.transform.position) > 20f);

        if (points.Count() == 0)
            throw new System.Exception("Not found far points");

        var point = points.GetRandomElement();


        var car = Instantiate(carPrefab, spawnPos.position, spawnPos.rotation).GetComponent<CarMovement>();

        car.SetMinMaxWaitTime(MinCarWaitTime, MaxCarWaitTime);
        car.SetDestination(point.transform.position);
        car.SetSpeedMultiply(SpeedMultily);
    }

    public void OnGameFinished()
    {
        _gameFinished = true;
    }
}
