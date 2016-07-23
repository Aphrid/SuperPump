using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControllerTutorial : MonoBehaviour {

    public GameObject spawner;
    public GameObject tutorialCar;

    public GUIText pplText;
    public GUIText pplTextShadow;
    public GUIText repText;
    public GUIText repTextShadow;

    // transition screen
    public GameObject transition;
    private bool transitioning;


    // Use this for initialization
    void Start () {
        
        StartCoroutine(SpawnCar());
	}

    IEnumerator SpawnCar()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<AudioSource>().Play();
        transitioning = true;
        yield return new WaitForSeconds(1.5f);
        GameObject newCarGO = Instantiate(tutorialCar, spawner.GetComponent<Transform>().position, spawner.GetComponent<Transform>().rotation) as GameObject;
        newCarGO.GetComponent<SpriteRenderer>().sortingLayerName = spawner.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
    }
	
    void Update()
    {
        if (transitioning)
        {
            if (transition.GetComponent<Transform>().position.y < 11.7f)
            {
                transition.GetComponent<Transform>().position = new Vector3(0.0f, transition.GetComponent<Transform>().position.y + 0.09f, 0.0f);
            }
            else
            {
                pplText.text = "Price: $1.00/L";
                pplTextShadow.text = "Price: $1.00/L";
                repText.text = "Reputation";
                repTextShadow.text = "Reputation";
                transitioning = false;
                gameObject.GetComponent<AudioSource>().Stop();
                Destroy(transition);
            }
        }
    }

}
