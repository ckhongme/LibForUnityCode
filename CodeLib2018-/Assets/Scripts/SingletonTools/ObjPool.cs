using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace K
{
    /// <summary>
    /// load file from Resources folder
    /// </summary>
    public class ObjPool : MonoBehaviour
    {
        private static ObjPool instance;
        public static ObjPool Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = new GameObject("ObjPool");
                    GameObject.DontDestroyOnLoad(obj);
                    obj.AddComponent<ObjPool>();
                }
                return instance;
            }
        }

        private Dictionary<string, List<GameObject>> _libs;
        private Transform _container;

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) DestroyImmediate(gameObject);
            _libs = new Dictionary<string, List<GameObject>>();
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void SetContainer(Transform container)
        {
            this._container = container;
        }
        
        #region Create GameObject

        public GameObject CreateObj(string key, GameObject prefab)
        {
            GameObject temp = _FindUsable(key);
            if (temp != null)
            {
                temp.SetActive(true);
            }
            else
            {
                if (prefab != null)
                {
                    temp = Instantiate(prefab) as GameObject;
                    if (_container != null)
                        temp.transform.SetParent(_container);
                    _Add(key, temp);
                }
            }
            return temp;
        }

        public GameObject CreateObj(string key, GameObject prefab, Vector3 postion, Quaternion quaternion)
        {
            GameObject temp = CreateObj(key,prefab);
            if (temp != null)
            {
                temp.transform.position = postion;
                temp.transform.rotation = quaternion;
            }
            return temp;
        }

        private GameObject _FindUsable(string key)
        {
            if (_libs.ContainsKey(key))
            {
                _libs[key].RemoveAll(p => p == null);
                return _libs[key].Find(p => !p.activeSelf);
            }
            return null;
        }

        private void _Add(string key, GameObject go)
        {
            if (!_libs.ContainsKey(key))
                _libs.Add(key, new List<GameObject>());
            _libs[key].Add(go);
        }

        #endregion

        #region Collect GameObject

        public void CollectObj(GameObject go)
        {
            go.SetActive(false);
        }

        public void CollectObj(GameObject go, float delay)
        {
            StartCoroutine(_DelayCollect(go, delay));
        }

        private IEnumerator _DelayCollect(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            CollectObj(go);
        }

        #endregion

        #region Clear

        public void ClearAll()
        {
            var list = new List<string>(_libs.Keys);
            while (list.Count > 0)
            {
                Clear(list[0]);
                list.RemoveAt(0);
            }
        }

        public void Clear(string key)
        {
            if (_libs.ContainsKey(key))
            {
                while (_libs[key].Count > 0)
                {
                    //release value first
                    Destroy(_libs[key][0]);
                    _libs[key].RemoveAt(0);
                }
                _libs.Remove(key);
            }
        }

        #endregion

        public List<GameObject> GetObjs(string key)
        {
            if (_libs.ContainsKey(key))
                return _libs[key];
            return null;
        }
    }
}


