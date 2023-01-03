using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance = null;

    public virtual void Awake() {
        if (Instance == null) {
            Instance = this as T;
        }
        else {
            Destroy(this);
        }
    }
}