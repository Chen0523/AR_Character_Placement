using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCamTransform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CamEyeTransform CamEye;
    private Camera Eye;
    private void Awake()
    {
        CamEye.UserTransform = gameObject.transform;
        CamEye.UserInitHeight = 1.7f;
        Eye = GetComponentInChildren<Camera>();
        CamEye.EyePos = Eye.transform.TransformPoint(Eye.transform.position);
    }
    private void Update()
    {
        CamEye.UserTransform = this.transform;
        CamEye.ViewRay = Eye.transform.forward;
        CamEye.EyePos = Eye.transform.TransformPoint(Eye.transform.position);
    }
}
