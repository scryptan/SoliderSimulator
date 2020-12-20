using UnityEngine;

namespace Root
{
    public static class Utils
    {
        public static Vector3 GetVectorFromAngle(float angle)
        {
            var rad = angle * (Mathf.PI / 180);
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    }
}