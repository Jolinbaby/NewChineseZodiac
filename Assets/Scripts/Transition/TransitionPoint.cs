using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene
    }

    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag desitionTag;
}
