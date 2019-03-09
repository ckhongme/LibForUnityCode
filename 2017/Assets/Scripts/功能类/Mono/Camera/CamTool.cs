using UnityEngine;
using System.Collections;

public class CamTool : MonoBehaviour
 {
    public static CamTool Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public Vector2 GetScreenPoint(Vector3 pos)
    {
        return Camera.main.WorldToScreenPoint(pos);
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

    public void SetFOV(Camera cam, float endValue, float speed = 5)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, endValue, Time.deltaTime * speed);
    }
}
