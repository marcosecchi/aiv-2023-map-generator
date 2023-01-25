using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyAllChildrenImmediate(this Transform tr)
    {
        while (tr.childCount > 0)
        {
            Object.DestroyImmediate(tr.GetChild(0).gameObject);
        }
    }

    public static void DestroyAllChildrenImmediate(this GameObject go)
    {
        go.transform.DestroyAllChildrenImmediate();
    }
}
