using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AcceptName : MonoBehaviour {

    public InputField nameField;
    public Button nameFieldAccept;

    private GameObject transition;
    private bool transMove;
    public GameObject day1;
    public GameObject day2;
    public GameObject day3;
    public GameObject day4;

    public Text levelText;

    public Button diff1;
    public Button diff2;
    public Button diff3;
    public Button diff4;

    public Camera cam;
    private bool cameraMove;
    private int level;

    void Start()
    {
        cameraMove = false;
        transMove = false;
        transition = null;
        level = 0;
    }

    public void ChooseDifficulty(int diff)
    {
        levelText.text = diff.ToString();
    }

    public void SaveName()
    {
        if (nameField.text.Equals(""))
        {
            nameField.text = "Andrew";
        }

        nameField.interactable = false;
        nameFieldAccept.interactable = false;
        cameraMove = true;
    }


    void Update()
    {
        if (cameraMove)
        {
            cam.GetComponent<Transform>().position = new Vector3(0f, cam.GetComponent<Transform>().position.y - 0.075f, -10.0f);
            nameField.GetComponent<RectTransform>().position = new Vector3(nameField.GetComponent<RectTransform>().position.x, nameField.GetComponent<RectTransform>().position.y + 11.5f, 0f);

            if (cam.GetComponent<Transform>().position.y <= -1.5f)
            {
                cameraMove = false;
                diff1.interactable = true;
                diff2.interactable = true;
                diff3.interactable = true;
                diff4.interactable = true;
            }
        }

        if (transMove)
        {
            if(transition.GetComponent<Transform>().position.y > -1.18f)
            {
                transition.GetComponent<Transform>().position = new Vector3(0.0f, transition.GetComponent<Transform>().position.y - 0.08f, 0.0f);
            } else
            {
                transMove = false;
                gameObject.GetComponent<AudioSource>().Stop();
                SceneManager.LoadScene(level);
            }
        }
    }

    // level is index in build settings, transition and then load level
    public void LoadScene(int levelEntry)
    {
        level = levelEntry;

        diff1.interactable = false;
        diff2.interactable = false;
        diff3.interactable = false;
        diff4.interactable = false;

        switch (levelText.text)
        {
            case "1":
                transition = day1;
                break;
            case "2":
                transition = day2;
                break;
            case "3":
                transition = day3;
                break;
            case "4":
                transition = day4;
                break;
        }
        transition.GetComponent<Transform>().localScale = new Vector3(1.4f, 1.4f, 0.0f);
        transMove = true;
        gameObject.GetComponent<AudioSource>().Play();   
    }

}
