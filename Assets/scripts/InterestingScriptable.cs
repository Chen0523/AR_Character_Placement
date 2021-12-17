using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "ScriptableObjects/InterestingObj")]
public class InterestingScriptable : ScriptableObject
{
    public float InterestingRange;
    public string CurModelId;

    public List<GameObject> IntObjInRange;
    public InterestingVertices IntObjsAll;
}
