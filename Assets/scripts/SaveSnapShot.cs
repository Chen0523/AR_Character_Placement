using System.IO;
using UnityEngine;

public class SaveSnapShot : MonoBehaviour
{
    public int fileCounter;
    public KeyCode screenshotKey;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            //Capture();
            string filepath = Application.dataPath + "/GeneratedImg/" + fileCounter + ".png";
            ScreenCapture.CaptureScreenshot(filepath);
            fileCounter++;

        }
    }
}