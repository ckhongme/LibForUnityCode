using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace K
{
    public static class Extension
    {
        /// <summary>
        /// 挂在指定的父物体上
        /// </summary>
        /// <param name="parent">指定的父物体</param>
        /// <param name="isStayLoacl">是否保留自身Transform的数值不变</param>
        public static void AddParent(this Transform tfm, Transform parent, bool isStayLocal = true)
        {
            if (tfm == null || parent == null) return;

            if (isStayLocal) //位置方向和体积 都根据父物体进行改变
            {
                Vector3 pos = tfm.localPosition;
                Vector3 rot = tfm.localEulerAngles;
                Vector3 scale = tfm.localScale;

                tfm.SetParent(parent);
                tfm.localPosition = pos;
                tfm.localEulerAngles = rot;
                tfm.localScale = scale;
            }
            else  //位置方向和体积 相对于场景都不变
            {
                tfm.SetParent(parent);
            }
        }

        public static void AddParent(this GameObject obj, Transform parent, bool isStayLocal = true)
        {
            obj.transform.AddParent(parent, isStayLocal);
        }

        /// <summary>
        /// 根据子物体路径获取对应的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tfm"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T GetComponentByPath<T>(this Transform tfm, string path = "") where T : Component
        {
            Transform t;
            if (string.IsNullOrEmpty(path))
                t = tfm;
            else
                t = tfm.Find(path);

            if (t == null)
            {
                Debug.LogError("没有找到 " + path + " 对应的Transform");
                return null;
            }

            T ret = t.GetComponent<T>();
            if (ret == null)
            {
                Debug.LogError("没有查到 " + typeof(T).ToString() + " 对应的Component");
                return null;
            }

            return ret;
        }

        /// <summary>
        /// 复制目标的Tfm（位置和方向）
        /// </summary>
        /// <param name="tfm"></param>
        /// <param name="target"></param>
        public static void CopyTfm(this Transform tfm, Transform target)
        {
            tfm.position = target.position;
            tfm.localEulerAngles = target.localEulerAngles;
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        public static void SetActiveSafe(this GameObject go, bool value)
        {
            if (go != null && go.activeSelf != value)
            {
                go.SetActive(value);
            }
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        public static void SetActive(this Transform tfm, bool value)
        {
            if (tfm != null)
                tfm.gameObject.SetActiveSafe(value);
        }

        /// <summary>
        /// ui对象的面向目标
        /// </summary>
        public static void UiLookAt(this Transform tfm, Transform target)
        {
            tfm.LookAt(target);
            Vector3 angle = tfm.localEulerAngles;
            tfm.localEulerAngles = new Vector3(-angle.x, angle.y + 180, angle.z);
        }

        public static Vector3 Local2World(this Transform tfm, Vector3 point, bool ignoreRotateXZ = false)
        {
            if (ignoreRotateXZ)
            {
                Transform t = tfm;
                t.eulerAngles = new Vector3(0, tfm.eulerAngles.y, 0);
                return t.TransformPoint(point);
            }
            return tfm.TransformPoint(point);
        }

        public static Vector3 World2Local(this Transform tfm, Vector3 point, bool ignoreRotateXZ = false)
        {
            if (ignoreRotateXZ)
            {
                Transform t = tfm;
                t.eulerAngles = new Vector3(0, tfm.eulerAngles.y, 0);
                return t.InverseTransformPoint(point);
            }
            return tfm.InverseTransformPoint(point);
        }
    }
}