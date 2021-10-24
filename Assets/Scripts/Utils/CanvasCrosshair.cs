using UnityEngine;

namespace Canvas.UI
{
    public class CanvasCrosshair : Singleton<CanvasCrosshair> 
    {
        private void OnApplicationPause(bool pause)
        {
            Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Confined;
            Cursor.visible = pause ? true : false;
        }
    }
}