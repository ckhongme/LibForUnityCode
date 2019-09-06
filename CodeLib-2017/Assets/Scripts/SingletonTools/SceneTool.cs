using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace K
{
    public class SceneTool : MonoBehaviour
    {
        private static SceneTool instance;
        public static SceneTool Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = new GameObject("SceneTool");
                    GameObject.DontDestroyOnLoad(obj);
                    obj.AddComponent<SceneTool>();
                }
                return instance;
            }
        }
        AsyncOperation async;

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) DestroyImmediate(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        #region Async

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(_LoadSceneAsync(sceneName));
        }

        private IEnumerator _LoadSceneAsync(string sceneName)
        {
            async = SceneManager.LoadSceneAsync(sceneName);
            yield return async;
        }

        public int GetProgress()
        {
            return (int)(async.progress * 100);
        }

        #endregion
    }
}