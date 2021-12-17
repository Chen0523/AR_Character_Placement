using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVCHeight : MonoBehaviour
{

    [SerializeField] VCScriptable VCInfos;
    // Start is called before the first frame update
    void Start()
    {
        VCInfos.VCTop = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("VC Height: " + this.transform.position.y);
        VCInfos.VCTop = this.transform.position.y;
    }
}
