using System;
using UnityEngine;
[Serializable]
public class InterestingObjFormat
{
    public string Id;
    public Vector3[] verticesThis;
    public Vector3[] verticesRotated;

    public InterestingObjFormat(string IdInput, Vector3[] vs)
    {
        this.Id = IdInput;
        this.verticesThis = vs;
    }
    public void logOne()
    {
        Debug.Log(this.Id);
    }
    public void rotateVertices(float Xangle, float Yangle, float Zangle)
    {
        this.verticesRotated = new Vector3[this.verticesThis.Length];
        Debug.Log("Rotating vertices in Interesting Object " + Id); 
        for(int i = 0; i< verticesThis.Length; i++)
            this.verticesRotated[i] = Quaternion.Euler(Xangle, Yangle, Zangle) * verticesThis[i];
    }
}
