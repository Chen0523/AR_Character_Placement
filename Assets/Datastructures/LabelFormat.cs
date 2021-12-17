using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Label
{
    //3D space feature 2
    public Vector3[] PointsAroundNeutral;
    public Vector3 QueryCenter;

    //camera parameters 2
    public float FOV;

    //user location 3
    public Vector3 UserLocRelToOrigin;

    public Vector3 UserHeadRelToQueryCenter;
    public Vector3 UserFeetRelToQueryCenter;

    //interesting objects vertices 1
    public List<string> InterestingObjs;

    // prediction neutral 2
    public Vector3 NeutralHeadRelToQueryCenter;
    public Vector3 NeutralFeetRelToQueryCenter;

    // confirmed VC predictions 3
    public Vector3 VCLocRelToOrigin;
    public float onRight;
    public Vector3 VCToNeutral;

    //2
    public float score;

    public Label()
    {
    }

}



