using UnityEngine;
using System.Collections;

public class NoDestroy : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        // Switch to 640 x 480 fullscreen
        Screen.SetResolution(1366, 768, true);
    }
}
