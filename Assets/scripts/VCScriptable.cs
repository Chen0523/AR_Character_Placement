using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "ScriptableObjects/VCInfo")]
public class VCScriptable : ScriptableObject
{
    public Vector3 UserFacingDir;
    public float VCTop;

    public float VCLocalScale;
}
