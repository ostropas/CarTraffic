using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum AvailablePath
    {
		ZAvailable,
		XAvailable
    }

    public AvailablePath CurrentAvailablePath;
}
