using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class InterestingVertices
{
    public Vector3 RoomTransform;
    public Vector3 RoomRotation;

    public List<InterestingObjFormat> InterestingObjs;
    public InterestingVertices()
    {
        InterestingObjs = new List<InterestingObjFormat>();
    }
    public void Add(InterestingObjFormat IntObj)
    {
        this.InterestingObjs.Add(IntObj);
    }
    public void SetRoomTransform(Vector3 roomT)
    {
        this.RoomTransform = roomT;
    }
    public void SetRoomRotation(Vector3 roomR)
    {
        this.RoomRotation = roomR;
    }
    public int GetCount()
    {
        return this.InterestingObjs.Count;
    }
    public void LogAll()
    {
        foreach(InterestingObjFormat i in this.InterestingObjs)
        {
            i.logOne();
        }
    }
    public void RotateAll(float Xangle, float Yangle, float Zangle)
    {
        foreach (InterestingObjFormat i in this.InterestingObjs)
        {
            i.rotateVertices(Xangle, Yangle, Zangle);
        }

    }
}
