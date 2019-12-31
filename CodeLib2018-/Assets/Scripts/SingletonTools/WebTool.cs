using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace K
{
    /// <summary>
    /// HTTP回调
    /// </summary>
    /// <returns>是否释放资源(已经成功获取数据)</returns>
    public delegate bool HttpCallBack(WWW www);

    public class WebTool : MonoBehaviour
    {
        private static WebTool instance;
        public static WebTool Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = new GameObject("WebTool");
                    GameObject.DontDestroyOnLoad(obj);
                    obj.AddComponent<WebTool>();
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

        #region Http

        public WWWForm CreateWWWForm(string key, string data)
        {
            WWWForm form = new WWWForm();
            form.AddField(key, data);
            return form;
        }

        public void PostData(string url, WWWForm form, HttpCallBack callBack = null)
        {
            WWW www = new WWW(url, form);
            StartCoroutine(_Send(www, callBack));
        }

        public void GetData(string url, HttpCallBack callBack = null)
        {
            WWW www = new WWW(url);
            StartCoroutine(_Send(www, callBack));
        }

        private IEnumerator _Send(WWW www, HttpCallBack callBack = null)
        {
            //挂起程序段，等资源下载完成后，继续执行下去  
            yield return www;
            if (callBack != null)
            {
                if (callBack(www))
                {
                    //判断是否有错误产生  
                    if (!string.IsNullOrEmpty(www.error))
                    {
                        //释放资源  
                        www.Dispose();
                    }
                }
            }
            else
            {
                //判断是否有错误产生  
                if (!string.IsNullOrEmpty(www.error))
                {
                    //释放资源  
                    www.Dispose();
                }
            }
        }

        public WWWForm SetLoginData(string idKey, string idData, string pwdKey, string pwdData)
        {
            WWWForm form = new WWWForm();
            form.AddField(idKey, idData);
            form.AddField(pwdKey, pwdData);
            return form;
        }

        #endregion
    }
}