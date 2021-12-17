using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeMove : MonoBehaviour
{
    [SerializeField] LabelScriptable LabelInfos;

    public CharacterController controller;
    public float speed = 1.0f;
    public Vector3 startLoc;
    // Start is called before the first frame update
    void Start()
    {
        startLoc = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LabelInfos.Adjusting)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }

    }

}
