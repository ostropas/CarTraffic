using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviorUtils 
{
    /// <summary>
    /// Timeout callback
    /// </summary>
    /// <param name="mb">Target mono behavior</param>
    /// <param name="time">Delay time</param>
    /// <param name="callback">Callback action</param>
    /// <returns>Delay coroutine</returns>
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
