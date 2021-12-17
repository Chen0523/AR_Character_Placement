using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{
    [SerializeField] LabelScriptable LabelInfos;
    public float mouseSensitivity = 70f;
    public Transform playerBody;
    float yRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -45, 45);
            transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        if (!LabelInfos.Adjusting)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }

    }
}
