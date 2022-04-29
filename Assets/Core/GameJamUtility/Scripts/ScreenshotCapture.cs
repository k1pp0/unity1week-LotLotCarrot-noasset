using UnityEngine;

namespace GameJamUtility
{
    public class ScreenshotCapture : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                ScreenCapture.CaptureScreenshot($"screenshot_{Time.frameCount}.png");
                Debug.Log($"screenshot_{Time.frameCount}");
            }
        }
    }
}
