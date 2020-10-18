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

    public void Start()
    {
        _currentTime = 0;
        _colliderCount = GetComponent<ColliderCount>();
    }

    public void OnEnable()
    {
        CarSpawners.Add(gameObject);
    }

    void Update()
    {
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

        _currentTime += Time.deltaTime;
    }

    private void GenerateCar()
    {
        var carPrefab = AvailableCarPrefabs.GetRandomElement();
        var spawnPos = GenerateAtPoints.GetRandomElement();

        var car = Instantiate(carPrefab, spawnPos.position, spawnPos.rotation).GetComponent<CarMovement>();

        // Select random far point
        var point = Waypoints
            .Where(x => x.IsFinish && Vector3.Distance(transform.position, x.transform.position) > 20f)
            .GetRandomElement();

        car.SetDestination(point.transform.position);
    }
}
