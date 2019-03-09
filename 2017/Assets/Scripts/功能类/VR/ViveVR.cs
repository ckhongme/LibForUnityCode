using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace K
{
    public static class ViveVR
    {
        #region vrPlayer

        /// <summary>
        /// 设置位置和朝向
        /// </summary>
        /// <param name="target">指定的位置对象</param>
        public static void ToPos(Transform vrPlayer, Transform vrCam, Transform target)
        {
            Rotate(vrPlayer, vrCam, target.rotation);
            ToPos(vrPlayer, vrCam, target.position);
        }

        /// <summary>
        /// 到达指定的位置
        /// </summary>
        /// <param name="targetPos">指定的位置</param>
        public static void ToPos(Transform vrPlayer, Transform vrCam, Vector3 targetPos)
        {
            vrPlayer.position = targetPos;
            float x = vrCam.position.x - vrPlayer.position.x;
            float z = vrCam.position.z - vrPlayer.position.z;
            Vector3 pos = vrPlayer.position;
            pos = new Vector3(pos.x - x, pos.y, pos.z - z);
            vrPlayer.position = pos;
        }

        /// <summary>
        /// 设置朝向
        /// </summary>
        /// <param name="rot">朝向</param>
        private static void Rotate(Transform vrPlayer, Transform vrCam, Quaternion rot)
        {
            vrPlayer.eulerAngles = Vector3.zero;
            float angle = vrCam.eulerAngles.y;
            vrPlayer.rotation = rot;
            vrPlayer.eulerAngles = new Vector3(0, vrPlayer.eulerAngles.y - angle, 0);
        }

        #endregion

        #region Handle

        /// <summary>
        /// 返回从form到to的角度
        /// </summary>
        public static float Vector2Angle(Vector2 from, Vector2 to)
        {
            float angle;
            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            return cross.z > 0 ? -angle : angle;
        }

        /// <summary>
        /// 圆盘的触摸区域
        /// </summary>
        public static string TouchArea(Vector2 v)
        {
            float angle = Vector2Angle(new Vector2(1, 0), v);
            string str = string.Empty;

            if (angle < -45 && angle > -135)                                            //上
                str = "up";
            else if (angle > 45 && angle < 135)                                         //下
                str = "down";
            else if ((angle > 0 && angle < 45) || (angle > -45 && angle < 0))           //右
                str = "right";
            else if ((angle < 180 && angle > 135) || (angle < -135 && angle > -180))    //左
                str = "left";

            return str;
        }

        #endregion
    }
}