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
}
