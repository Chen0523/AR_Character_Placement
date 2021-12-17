using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "ScriptableObjects/CamEyeTransform", order = 1)]
public class CamEyeTransform : ScriptableObject
{
    //public List<Transform> Objs;

    public float UserInitHeight;
    public float RandomFOV;

    public Transform UserTransform;
    public Vector3 ViewRay;
    public Vector3 EyePos;

    public Vector3 QueryCenter;
    public Vector3[] PointsAroundNeutral;
}
