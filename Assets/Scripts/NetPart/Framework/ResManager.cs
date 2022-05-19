using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
    //º”‘ÿ‘§…Ë
    public static GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
}
