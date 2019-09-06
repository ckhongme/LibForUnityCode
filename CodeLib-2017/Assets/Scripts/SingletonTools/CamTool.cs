using UnityEngine;
using System.Collections;


namespace K
{
    public class CamTool : MonoBehaviour
    {
        private static CamTool instance;
        public static CamTool Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = new GameObject("CamTool");
                    GameObject.DontDestroyOnLoad(obj);
                    obj.AddComponent<CamTool>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) DestroyImmediate(gameObject);
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public Vector2 GetScreenPoint(Vector3 pos)
        {
            return Camera.main.WorldToScreenPoint(pos);
        }

        public void SetFOV(Camera cam, float endValue, float speed = 5)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, endValue, Time.deltaTime * speed);
        }

        #region ShakeScreen

        public void ShakeScreen(Camera cam, float shakeTime, float shakeDelta = 0.05f)
        {
            StartCoroutine(_ShakeScreen(cam, shakeTime, shakeDelta));
        }

        private IEnumerator _ShakeScreen(Camera cam, float shakeTime, float shakeDelta)
        {
            while (shakeTime > 0)
            {
                shakeTime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
                cam.rect = new Rect(0.05f * (-shakeDelta + 1.0f * Random.value),
                    0.05f * (-shakeDelta + 1.0f * Random.value), 1.0f, 1.0f);
            }

            if (shakeTime <= 0)
            {
                cam.rect = new Rect(0, 0, 1, 1);
            }
        }

        #endregion

        #region ScreenShot

        /// <summary>
        /// 截屏（全屏）
        /// </summary>
        /// <param name="filePath">png文件保存路径</param>
        public void ScreenShot(string filePath)
        {
            ScreenCapture.CaptureScreenshot(filePath, 0);
        }

        /// <summary>
        /// 截屏（3D场景） 指定范围Rect
        /// </summary>
        /// <param name="camera">选定的摄像机</param>
        /// <param name="rect">选定的屏幕区域</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="mipmap">是否打开mipmap</param>
        /// <returns></returns>
        public Texture2D ScreenShot(Camera camera, Rect rect, string savePath = null, bool mipmap = false)
        {
            if (camera == null) return null;

            //设置位图深度（0，16，24）, 深度不够，会出现部分模型不显示
            int depth = 24;
            //创建RenderTexture，临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, depth);
            camera.targetTexture = rt;
            camera.Render();

            //激活rt, 并读取像素存储为纹理数据
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D((int)rt.width, (int)rt.height, TextureFormat.RGB24, mipmap);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();
            //重置相关参数，以使用camera继续在屏幕上显示  
            camera.targetTexture = null;
            RenderTexture.active = null;
            rt.Release();
            GameObject.Destroy(rt);

            if (savePath != null)
            {
                byte[] bytes = screenShot.EncodeToPNG();
                System.IO.File.WriteAllBytes(savePath, bytes);
            }
            return screenShot;
        }

        /// <summary>
        /// 截屏（3D场景） 指定宽和高
        /// </summary>
        /// <param name="camera">指定的摄像机</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>贴图</returns>
        public Texture2D ScreenShot(Camera camera, float width, float height, string savePath = null)
        {
            Rect rect = new Rect(0, 0, width, height);
            return ScreenShot(camera, rect, savePath);
        }

        public Texture2D ScreenShotByCam(Camera camera, string savePath = null, bool mipmap = false)
        {
            return ScreenShot(camera, new Rect(0, 0, camera.pixelWidth, camera.pixelHeight), savePath, mipmap);
        }

        #endregion
    }
}