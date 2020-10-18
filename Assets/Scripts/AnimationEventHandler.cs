using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent<int> OnAnimation;

    public void RaiseAnimEvent(int id)
    {
        OnAnimation.Invoke(id);
    }
}
