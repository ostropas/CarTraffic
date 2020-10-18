using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviorUtils 
{
    public static Coroutine Delay(this MonoBehaviour mb, float time, System.Action callback)
    {
        return mb.StartCoroutine(DelayCoroutine(time, callback));
    }

    private static IEnumerator DelayCoroutine(float time, System.Action callback)
    {
        yield return new WaitForSeconds(time);
        callback.Invoke();
    }
}
