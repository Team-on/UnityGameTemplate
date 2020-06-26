using UnityEngine;

namespace UnityForge
{
    public static class TransformExtentions
    {
        /// <summary>
        /// Rotates the transform so the forward vector points from target's current position.
        /// </summary>
        /// <param name="target">Object to point away from.</param>
        public static void LookFrom(this Transform transform, Transform target)
        {
            transform.LookFrom(target.position);
        }

        /// <summary>
        /// Rotates the transform so the forward vector points from target's current position.
        /// </summary>
        /// <param name="target">Object to point away from.</param>
        /// <param name="worldUp">Vector specifying the upward direction.</param>
        public static void LookFrom(this Transform transform, Transform target, Vector3 worldUp)
        {
            transform.LookFrom(target.position, worldUp);
        }

        /// <summary>
        /// Rotates the transform so the forward vector points from worldPosition.
        /// </summary>
        /// <param name="worldPosition">Point to look at.</param>
        public static void LookFrom(this Transform transform, Vector3 worldPosition)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - worldPosition);
        }

        /// <summary>
        /// Rotates the transform so the forward vector points from worldPosition.
        /// </summary>
        /// <param name="worldPosition">Point to look at.</param>
        /// <param name="worldUp">Vector specifying the upward direction.</param>
        public static void LookFrom(this Transform transform, Vector3 worldPosition, Vector3 worldUp)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - worldPosition, worldUp);
        }

        /// <summary>
        /// Sets the transform world space position x component.
        /// </summary>
        /// <param name="value">Value of x component.</param>
        public static void SetPositionX(this Transform transform, float value)
        {
            transform.position = Vector3Utils.SetX(transform.position, value);
        }

        /// <summary>
        /// Sets the transform world space position y component.
        /// </summary>
        /// <param name="value">Value of y component.</param>
        public static void SetPositionY(this Transform transform, float value)
        {
            transform.position = Vector3Utils.SetY(transform.position, value);
        }

        /// <summary>
        /// Sets the transform world space position z component.
        /// </summary>
        /// <param name="value">Value of z component.</param>
        public static void SetPositionZ(this Transform transform, float value)
        {
            transform.position = Vector3Utils.SetZ(transform.position, value);
        }

        /// <summary>
        /// Sets the transform local space position x component.
        /// </summary>
        /// <param name="value">Value of x component.</param>
        public static void SetLocalPositionX(this Transform transform, float value)
        {
            transform.localPosition = Vector3Utils.SetX(transform.localPosition, value);
        }

        /// <summary>
        /// Sets the transform local space position y component.
        /// </summary>
        /// <param name="value">Value of y component.</param>
        public static void SetLocalPositionY(this Transform transform, float value)
        {
            transform.localPosition = Vector3Utils.SetY(transform.localPosition, value);
        }

        /// <summary>
        /// Sets the transform world space position z component.
        /// </summary>
        /// <param name="value">Value of z component.</param>
        public static void SetLocalPositionZ(this Transform transform, float value)
        {
            transform.localPosition = Vector3Utils.SetZ(transform.localPosition, value);
        }

        public static void AddChild(this Transform transform, GameObject childGameObject)
        {
            childGameObject.transform.SetParent(transform, false);
        }

        public static void AddChild(this Transform transform, GameObject childGameObject, bool worldPositionStays)
        {
            childGameObject.transform.SetParent(transform, worldPositionStays);
        }

        public static void AddChild(this Transform transform, Transform childTransform)
        {
            childTransform.SetParent(transform, false);
        }

        public static void AddChild(this Transform transform, Transform childTransform, bool worldPositionStays)
        {
            childTransform.SetParent(transform, worldPositionStays);
        }

        public static void DestroyChildren(this Transform transform)
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
