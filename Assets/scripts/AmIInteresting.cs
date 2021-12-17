using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmIInteresting : MonoBehaviour
{
    [SerializeField] private CamEyeTransform CamEye;
    [SerializeField] private InterestingScriptable InterestingObjs;
    Mesh MeshThis;
    Vector3[] VerticesThis;
    Vector3[] BorderVertices;
    private void Start()
    {
        MeshThis = this.GetComponent<MeshFilter>().mesh;
        VerticesThis = new Vector3[MeshThis.vertices.Length];

        for(int i =0;  i<MeshThis.vertices.Length; i++)
        {
            VerticesThis[i] = transform.TransformPoint(MeshThis.vertices[i]);
        }

        InterestingObjFormat OneOj = new InterestingObjFormat(this.name,VerticesThis);
        InterestingObjs.IntObjsAll.Add(OneOj);

        Check();
    }
    public void Check()
    {
        float dis = Vector3.Distance(
            new Vector3(this.transform.position.x, CamEye.UserTransform.position.y,this.transform.position.z), 
            CamEye.UserTransform.position);
        Vector3 projected = new Vector3(this.transform.position.x - CamEye.UserTransform.position.x, 0, 
            this.transform.position.z-CamEye.UserTransform.position.z);
        float angle = Vector3.SignedAngle(CamEye.UserTransform.forward, projected, Vector3.up);

        if (dis < InterestingObjs.InterestingRange & angle<90 && angle > -90)
        {
            if (!InterestingObjs.IntObjInRange.Contains(this.gameObject))
            {
                InterestingObjs.IntObjInRange.Add(this.gameObject);
                BorderVertices = GetBorder(VerticesThis,this.transform.position,CamEye.UserTransform.position);
            }
        }
        else
        {
            InterestingObjs.IntObjInRange.Remove(this.gameObject);
        }
    }
    public Vector3[] GetMeshVertices()
    {
        if (MeshThis)return VerticesThis;
        else return new Vector3[0];
    }
    public float GetLowerBound()
    {
        return BorderVertices[2].y;
    }
    public Vector3[] GetBorder(Vector3[] vertices, Vector3 center, Vector3 UserPos)
    {
        float MinAngle = 360f;
        Vector3 BL = center;
        float MaxAngle = -360f;
        Vector3 BR = center;
        float miny = 4.0f;
        Vector3 BU = center;
        Vector3 StartProjected = Vector3.ProjectOnPlane((center - UserPos), Vector3.up);

        Vector3[] Borders = new Vector3[3];

        for(int i = 0;  i < vertices.Length; i++)
        {
            Vector3 projected = Vector3.ProjectOnPlane((vertices[i] - UserPos), Vector3.up);
            float angle = Vector3.SignedAngle(StartProjected, projected, Vector3.up);
            if (angle > MaxAngle)
            {
                MaxAngle = angle;
                BR = vertices[i];
            }
            if (angle < MinAngle)
            {
                MinAngle = angle;
                BL = vertices[i];
            }
            if(vertices[i].y < miny)
            {
                miny = vertices[i].y;
                BU = vertices[i];
            }
        }
        Borders[0] = BL;
        Borders[1] = BR;
        Borders[2] = BU;
        return Borders;
    }
    public float[] GetAngles()
    {
        float[] OneRange = new float[2];

        Vector3 projected1 = Vector3.ProjectOnPlane((BorderVertices[0] - CamEye.UserTransform.position), Vector3.up);
        Vector3 projected2 = Vector3.ProjectOnPlane((BorderVertices[1] - CamEye.UserTransform.position), Vector3.up);
        float angle1 = Vector3.SignedAngle(CamEye.UserTransform.forward, projected1, Vector3.up);
        float angle2 = Vector3.SignedAngle(CamEye.UserTransform.forward, projected2, Vector3.up);
        
        if (angle1 < angle2)
        {
            OneRange[0] = angle1;
            OneRange[1] = angle2;
        }
        else
        {
            OneRange[0] = angle2;
            OneRange[1] = angle1;
        }
        return OneRange;
    }
}


