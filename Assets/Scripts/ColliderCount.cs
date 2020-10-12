using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderCount : MonoBehaviour
{
    public List<Collision> Collisions = new List<Collision>();

    public void OnCollisionEnter(Collision collision)
    {
        Collisions.Add(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        Collisions.Remove(collision);
    }
}
