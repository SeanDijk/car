using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class AbstractSensor<T> : MonoBehaviour
{
    protected T myListener;

}