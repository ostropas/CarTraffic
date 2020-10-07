using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class WayPointSystem : MonoBehaviour
{
	[Serializable]
	public class Path
    {
		public WayPoint Start;
		public WayPoint Finish;
		public int LinesCount;
    }

	#region Initialization
	void Awake()
	{
		//_waypoints = WayPoints.ToDictionary(x => x.Id);
	}
	#endregion

	#region Public Fields
	public List<Path> RoadPart;
	#endregion
	
	#region Private Fields
	#endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
		//var wayPointsDict = WayPoints.ToDictionary(x => x.Id);
		//var allPaths = Paths.Select(path => path.WaypointIndex.Select(index => wayPointsDict[index]).ToList()).ToList();

		//Gizmos.color = Color.red;
  //      foreach (var path in allPaths)
  //      {
  //          for (int i = 0; i < path.Count - 1; i++)
  //          {
		//		Gizmos.DrawLine(path[i].transform.position, path[i + 1].transform.position);
  //          }
  //      }
	}

    #region Public Methods
    [ContextMenu("Index It!")]
	public void IndexedWaypoint()
    {
		//var waypoints = GetComponentsInChildren<WayPoint>().ToList();

		//for (int i = 0; i < _waypoints.Count; i++)
		//{
		//	waypoints[i].Id = i;
		//}
	}

	public List<Path> GetRandomPath(WayPoint wayPoint)
    {
        var finalPath = new List<Path>
        {
            RoadPart[0],
            RoadPart[1]
        };

        return finalPath;
    }
	#endregion
	
	#region Private Methods
	#endregion
}
