using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeight : MonoBehaviour
{
    [SerializeField] VCScriptable VCInfos;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        VCInfos.VCLocalScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(VCInfos.VCLocalScale, VCInfos.VCLocalScale, 1);
        VCInfos.VCTop = this.transform.position.y;
    }
}
