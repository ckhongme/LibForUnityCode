using UnityEngine;

namespace K
{
    public class SpaceTool
    {
        #region Angle

        /// <summary>
        /// Get the angle between from and to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="isClockwise"></param>
        /// <returns></returns>
        public static float GetAngle(Vector2 from, Vector2 to, bool isClockwise = false)
        {
            Vector3 cross = Vector3.Cross(from, to);
            float angle = Vector2.Angle(from, to);
            //大于0时表示顺时针，小于0时表示逆时针
            angle = cross.z > 0 ? -angle : angle;
            return isClockwise ? angle : -angle;
        }

        /// <summary>
        /// Get the angle between from and to, and limit the value between 0 to 360
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="isClockwise"></param>
        /// <returns></returns>
        public static float GetPositiveAngle(Vector2 from, Vector3 to, bool isClockwise = false)
        {
            float angle = GetAngle(from, to, isClockwise);
            return angle < 0 ? 360 + angle : angle;
        }

        /// <summary>
        /// get the angle between from and to (left-hand role)
        /// </summary>
        /// <param name="normal">the normal of plane</param>
        /// <returns></returns>
        public static float GetAngle(Vector3 from, Vector3 to, Vector3 normal, bool isClockwise = false)
        {
            var angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(from, to), Vector3.ProjectOnPlane(to, normal), normal);
            return isClockwise ? angle : -angle;

            //Vector3 cross = Vector3.Cross(from, to);
            //float value = Vector3.Dot(cross, normal);
            //float angle = Vector3.Angle(from, to);
            //angle = value > 0 ? -angle : angle;
            //return isClockwise ? -angle : angle;
        }

        public static float GetPositiveAngle(Vector3 from, Vector3 to, Vector3 normal, bool isClockwise = false)
        {
            float angle = GetAngle(from, to, normal, isClockwise);
            return angle < 0 ? 360 + angle : angle;
        }

        /// <summary>
        /// get the angle between zeroDirection and the plane project of target (0~360)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="zeroDirection"></param>
        /// <param name="normal">the normal of the plane (base on the left-half corrdinate system) </param>
        /// <returns></returns>
        public static float GetProjectAngle(Vector3 target, Vector3 zeroDirection, Vector3 normal,
            bool isClockwise = false)
        {
            var project = Vector3.ProjectOnPlane(target, normal);
            return GetAngle(zeroDirection, project, normal, isClockwise);
        }

        #endregion

        /// <summary>
        /// 获取对象指定方向某个距离的点坐标
        /// </summary>
        public static Vector3 GetDirectedPoint(Transform origin, Vector3 direction, float distance)
        {
            return origin.position + direction * distance;
        }

        /// <summary>
        /// 获取对象旋转指定角度后，某个距离的点坐标
        /// </summary>
        public static Vector3 GetDirectedPoint(Transform origin, float angle, float distance)
        {
            Vector3 v = Quaternion.Euler(new Vector3(0, angle, 0)) * origin.forward;
            return origin.position + v * distance;
        }
    }
}