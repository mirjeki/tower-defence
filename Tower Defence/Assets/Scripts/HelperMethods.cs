using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    public static GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
