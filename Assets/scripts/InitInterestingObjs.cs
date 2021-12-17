using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitInterestingObjs : MonoBehaviour
{
    [SerializeField] InterestingScriptable InterestingObjs;
    public float InterestingRange = 4.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        InterestingObjs.IntObjsAll = new InterestingVertices();

        InterestingObjs.IntObjInRange = new List<GameObject>();
        InterestingObjs.InterestingRange = InterestingRange;
    }
}

//private List<float[]> CalAngelsInterestingObjs(List<Vector3[]> InterestingVertices, Transform Usertrans, Vector3 UserEye)
//{
//    List<float[]> InterestingAngles = new List<float[]>();
//    //get the edge vertices of all objects in range
//    foreach (Vector3[] vertices in InterestingVertices)
//    {
//        float MinAngle = 360;
//        float MaxAngle = -360;
//        float[] OneRange = new float[2];
//        //find the edge vertices
//        for (int i = 0; i < vertices.Length; i++)
//        {
//            Vector3 projected = Vector3.ProjectOnPlane((vertices[i] - UserEye), Vector3.up);
//            //Debug.DrawRay(UserEye, projected, Color.red,1f); ;
//            //Debug.DrawRay(Usertrans.position, Usertrans.forward, Color.green,1f); ;
//            float angle = Vector3.SignedAngle(Usertrans.forward, projected, Vector3.up);
//            if (angle > MaxAngle) MaxAngle = angle;
//            if (angle < MinAngle) MinAngle = angle;
//        }
//        if (MaxAngle < CamEye.RandomFOV * -0.5 || MinAngle > CamEye.RandomFOV * 0.5)
//        {
//            continue;
//        }
//        else
//        {
//            OneRange[0] = MinAngle;
//            OneRange[1] = MaxAngle;
//            Debug.Log("One Range Min" + OneRange[0]);
//            Debug.Log("One Range max" + OneRange[1]);
//        }

//        InterestingAngles.Add(OneRange);
//    }
//    return InterestingAngles;
//}
