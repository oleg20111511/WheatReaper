using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Transform FindClosestTransform(List<Transform> transforms, Transform targetTransform)
    {
        Transform closestTransform = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform transform in transforms)
        {
            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if (distance < closestDistance)
            {
                closestTransform = transform;
                closestDistance = distance;
            }
        }
        

        return closestTransform;
    }


    public static T FindClosestObject<T>(List<T> objects, Transform targetTransform, Func<T, Transform> getTransform)
    {
        T closestObject = default(T);
        float closestDistance = Mathf.Infinity;

        foreach (T obj in objects)
        {
            Transform transform = getTransform(obj);
            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if (distance < closestDistance)
            {
                closestObject = obj;
                closestDistance = distance;
            }
        }

        return closestObject;
    }


    public static bool IsClose(Transform t1, Transform t2)
    {
        float distance = (t1.position - t2.position).magnitude;
        return distance < 0.1f;
    }
}
