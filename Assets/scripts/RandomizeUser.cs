using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeUser : MonoBehaviour
{
    [SerializeField] CamEyeTransform CamEye;
    [SerializeField] LabelScriptable LabelInfo;
    Camera View;
    private void Start()
    {
        View = GetComponentInChildren<Camera>();
        CamEye.RandomFOV = 56.42f;
    }
    void Update()
    {
        //random fov
        View.fieldOfView = CamEye.RandomFOV;
    }

}
