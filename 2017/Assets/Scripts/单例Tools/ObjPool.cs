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
        public static ObjPool Instance;
        private Dictionary<string, List<GameObject>> cache;

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
            cache = new Dictionary<string, List<GameObject>>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        #region create gameObject


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
            if (cache.ContainsKey(key))
            {
                cache[key].RemoveAll(p => p == null);
                return cache[key].Find(p => !p.activeSelf);
            }
            return null;
        }

        private void _Add(string key, GameObject go)
        {
            if (!cache.ContainsKey(key))
                cache.Add(key, new List<GameObject>());
            cache[key].Add(go);
        }

        #endregion

        #region collect gameObject

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
            var list = new List<string>(cache.Keys);
            while (list.Count > 0)
            {
                Clear(list[0]);
                list.RemoveAt(0);
            }
        }

        public void Clear(string key)
        {
            if (cache.ContainsKey(key))
            {
                while (cache[key].Count > 0)
                {
                    //release value first
                    Destroy(cache[key][0]);
                    cache[key].RemoveAt(0);
                }
                cache.Remove(key);
            }
        }

        #endregion
    }
}


