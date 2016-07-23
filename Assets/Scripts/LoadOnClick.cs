using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

    // level is index in build settings
	public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }
}
