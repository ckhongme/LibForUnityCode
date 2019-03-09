using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace K
{
    public class SceneTool : MonoBehaviour
    {
        public static SceneTool Instance;
        AsyncOperation async;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    Destroy(gameObject);
                }
            }
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