using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRoomTransform : MonoBehaviour
{
    [SerializeField] InterestingScriptable interestingInfos;
    // Start is called before the first frame update
    Vector3 RoomTransform;
    Vector3 RoomRotation;
    void Start()
    {
        RoomTransform = this.transform.localPosition;
        RoomRotation = new Vector3(this.transform.localRotation.eulerAngles.x, 
            this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z);
        Debug.Log("[INFO] Room Rotation :" + RoomRotation);
        interestingInfos.IntObjsAll.SetRoomRotation(RoomRotation);
        interestingInfos.IntObjsAll.SetRoomTransform(RoomTransform);
    }
}
