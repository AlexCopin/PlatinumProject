using UnityEngine;
abstract class Unique<T> : MonoBehaviour where T : class
{
    public static T _instance;
    void Awake() { if (_instance == null) { _instance = GameObject.FindObjectOfType(typeof(T)) as T; DontDestroyOnLoad(gameObject); } else Destroy(gameObject); }
}