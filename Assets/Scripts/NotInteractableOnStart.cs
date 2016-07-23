using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotInteractableOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().interactable = false;
	}
}
