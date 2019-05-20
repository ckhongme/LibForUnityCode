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
        private Dictionary<string, List<GameObject>> libs;

        private Transform container;

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
            libs = new Dictionary<string, List<GameObject>>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SetContainer(Transform container)
        {
            this.container = container;
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
                    if (container != null)
                        temp.transform.SetParent(container);
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
            if (libs.ContainsKey(key))
            {
                libs[key].RemoveAll(p => p == null);
                return libs[key].Find(p => !p.activeSelf);
            }
            return null;
        }

        private void _Add(string key, GameObject go)
        {
            if (!libs.ContainsKey(key))
                libs.Add(key, new List<GameObject>());
            libs[key].Add(go);
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
            var list = new List<string>(libs.Keys);
            while (list.Count > 0)
            {
                Clear(list[0]);
                list.RemoveAt(0);
            }
        }

        public void Clear(string key)
        {
            if (libs.ContainsKey(key))
            {
                while (libs[key].Count > 0)
                {
                    //release value first
                    Destroy(libs[key][0]);
                    libs[key].RemoveAt(0);
                }
                libs.Remove(key);
            }
        }

        #endregion

        public List<GameObject> GetObjs(string key)
        {
            if (libs.ContainsKey(key))
                return libs[key];
            return null;
        }
    }
}


