using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class OffMeshMover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private bool _moveAcrossNavMeshesStarted = false;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.isOnOffMeshLink && !_moveAcrossNavMeshesStarted)
        {
            StartCoroutine(MoveAcrossNavMeshLink());
            _moveAcrossNavMeshesStarted = true;
        }
    }


    IEnumerator MoveAcrossNavMeshLink()
    {
        OffMeshLinkData data = _navMeshAgent.currentOffMeshLinkData;
        _navMeshAgent.updateRotation = false;


        var tmpPos = _navMeshAgent.destination;

        Vector3 startPos = _navMeshAgent.transform.position;
        Vector3 endPos = data.endPos;
        endPos.y = 0;
        float duration = (endPos - startPos).magnitude / _navMeshAgent.velocity.magnitude;
        float t = 0.0f;
        float tStep = 1.0f / duration;        
        while (t < 1.0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.LookAt(endPos);
            _navMeshAgent.destination = transform.position;
            t += tStep * Time.deltaTime;
            yield return null;
        }        
        transform.position = endPos;
        _navMeshAgent.destination = tmpPos;
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.CompleteOffMeshLink();
        _moveAcrossNavMeshesStarted = false;

    }
}
