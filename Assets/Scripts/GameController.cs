using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject carSpawner;
    public GameObject carSpawner2;
    public GameObject carSpawner3;
    public GameObject carSpawner4;

    public GameObject iconSpawner1;
    public GameObject iconSpawner2;
    public GameObject iconSpawner3;
    public GameObject iconSpawner4;

    public GameObject blankSpawner1;
    public GameObject blankSpawner2;
    public GameObject blankSpawner3;
    public GameObject blankSpawner4;

    public GameObject car;
    public GameObject car_2;
    public GameObject car_3;

    public GameObject car14;
    public GameObject car14_2;
    public GameObject car14_3;

    public int reputation;

    public GUIText pplText;
    public GUIText pplTextShadow;

    public GUIText gameOverText;
    public GUIText gameOverTextShadow;
    public GUIText restartText;
    public GUIText restartTextShadow;

    public AudioSource good;
    public AudioSource bad;

    public GameObject bar;

    public GameObject goodIcon;
    public GameObject badIcon;
    public GameObject blank;

    private bool transMove;

    private Vector3 barDim;
    private bool initSpawn; // to start the spawning of cars at a new level
    private bool gameOver;
    private float ppl; // price per litre
    private int goodServ; // number of good/ok proximity cars to advance level

    // to get the level selection and difficulty
    private int level;

    // to control text during the transition screen
    public GUIText[] transTextCurrent;
    public GUIText[] transTextReputation;
    public GUIText[] transTextPPL;
    private GameObject transition;

    // Use this for initialization
    void Start() {
        gameOver = false;
        initSpawn = false;
        ppl = 1.0f;
        goodServ = 0;
        Random.seed = System.Environment.TickCount;
        barDim = bar.GetComponent<Transform>().localScale;

        StartCoroutine(SpawnCar("car1"));

        gameOverText.text = "";
        gameOverTextShadow.text = "";
        restartText.text = "";
        restartTextShadow.text = "";

        // get the level amount
        GameObject levelTextObject = GameObject.Find("LevelText") as GameObject;
        if(levelTextObject != null)
        {
            level = int.Parse(levelTextObject.GetComponent<Text>().text);
        }

        if(level == 2)
        {
            transition = GameObject.Find("Day2");
        } else if (level == 3)
        {
            transition = GameObject.Find("Day4");
        } else // 4 stations
        {
            transition = GameObject.Find("Day6");
        }

        if(level == 2 || level == 3)
        {
            transition.GetComponent<Transform>().localScale = new Vector3(2.8f, 2.8f, 0.0f);
        } else
        {
            transition.GetComponent<Transform>().localScale = new Vector3(3.34f, 3.34f, 0.0f);
        }
        

        for (int i = 0; i < transTextReputation.Length; i++)
        {
            transTextReputation[i].text = "";
        }

        for (int i = 0; i < transTextCurrent.Length; i++)
        {
            transTextCurrent[i].text = "";
        }

        for (int i = 0; i < transTextPPL.Length; i++)
        {
            transTextPPL[i].text = "";
        }

        transMove = true;
        
    }

    void Update()
    {
        if (!transMove)
        {
            pplText.text = "Price: " + ppl.ToString("C2") + "/L";
            pplTextShadow.text = "Price: " + ppl.ToString("C2") + "/L";

            if (reputation <= 0)
            {
                gameOver = true;
                gameOverText.text = "GAMEOVER";
                gameOverTextShadow.text = "GAMEOVER";
                restartText.text = "Press SPACE To Restart";
                restartTextShadow.text = "Press SPACE To Restart";
            }

            if (gameOver && Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }

            if (initSpawn)
            {
                initSpawn = false;

                if ((level == 2 && goodServ == 2) || (level == 3 && goodServ == 3) || (level == 4 && goodServ == 2))
                {
                    StartCoroutine(SpawnCar("car2"));
                }
                else if (level == 5)
                {
                    switch (goodServ)
                    {
                        case 2:
                            StartCoroutine(SpawnCar("car2"));
                            break;
                        case 5:
                            StartCoroutine(SpawnCar("car3"));
                            break;
                        case 10:
                            StartCoroutine(SpawnCar("car4"));
                            break;
                    }
                }
                else if (level == 6)
                {
                    switch (goodServ)
                    {
                        case 2:
                            StartCoroutine(SpawnCar("car2"));
                            break;
                        case 4:
                            StartCoroutine(SpawnCar("car3"));
                            break;
                        case 9:
                            StartCoroutine(SpawnCar("car4"));
                            break;
                    }
                }
            }
        } else
        {
            float max = 0.0f;
            if (level == 4)
            {
                max = 13.8f;
            } else
            {
                max = 11.7f;
            }

            if (transition.GetComponent<Transform>().position.y < max)
            {
                transition.GetComponent<Transform>().position = new Vector3(transition.GetComponent<Transform>().position.x, transition.GetComponent<Transform>().position.y + 0.09f, 0.0f);
            }
            else
            {
                transMove = false;
                for(int i = 0; i < transTextReputation.Length; i++)
                {
                    transTextReputation[i].text = "Reputation";
                }

                for (int i = 0; i < transTextCurrent.Length; i++)
                {
                    transTextCurrent[i].text = "CURRENT (L)";
                }

                for (int i = 0; i < transTextPPL.Length; i++)
                {
                    transTextPPL[i].text = "Price: $1.00/L";
                }
            }
        }

    }

    // for cars to check with the game controller which level we're on
    public int GetGoodServ()
    {
        return this.goodServ;
    }

    // Car hits gameController 
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);

        if(gameOver == false)
        {
            switch (other.gameObject.tag)
            {
                case "car1":
                    StartCoroutine(SpawnCar("car1"));
                    break;
                case "car2":
                    StartCoroutine(SpawnCar("car2"));
                    break;
                case "car3":
                    StartCoroutine(SpawnCar("car3"));
                    break;
                case "car4":
                    StartCoroutine(SpawnCar("car4"));
                    break;
            }
        }
    }


    IEnumerator SpawnCar(string tag)
    {
        GameObject carPrefab = null;
        GameObject spawner = null;
        GameObject newCarGO;

        yield return new WaitForSeconds(Random.Range(3.0F, 7.0F));        
        float x = Random.Range(0.0f, 3.0f); // Randomly choose which car to spawn

        switch (tag)
        {
            case "car1":
                spawner = carSpawner;
                if (0.0f <= x && x < 1.0f)
                {
                    carPrefab = car14;                    
                } else if (1.0f <= x && x < 2.0f)
                {
                    carPrefab = car14_2;
                } else
                {
                    carPrefab = car14_3;
                }                
                break;
            case "car2":
                spawner = carSpawner2;
                if (0.0f <= x && x < 1.0f)
                {
                    carPrefab = car;
                }
                else if (1.0f <= x && x < 2.0f)
                {
                    carPrefab = car_2;
                }
                else
                {
                    carPrefab = car_3;
                }
                break;
            case "car3":
                spawner = carSpawner3;
                if (0.0f <= x && x < 1.0f)
                {
                    carPrefab = car;
                }
                else if (1.0f <= x && x < 2.0f)
                {
                    carPrefab = car_2;
                }
                else
                {
                    carPrefab = car_3;
                }
                break;
            case "car4":
                spawner = carSpawner4;
                if (0.0f <= x && x < 1.0f)
                {
                    carPrefab = car14;
                }
                else if (1.0f <= x && x < 2.0f)
                {
                    carPrefab = car14_2;
                }
                else
                {
                    carPrefab = car14_3;
                }
                break;
        }
        newCarGO = Instantiate(carPrefab, spawner.GetComponent<Transform>().position, spawner.GetComponent<Transform>().rotation) as GameObject;
        newCarGO.GetComponent<SpriteRenderer>().sortingLayerName = spawner.gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
    }

    public bool CalculateRep(float carMoney, float amtFilled)
    {
        bool result;
        float proximity = Mathf.Abs(carMoney - (amtFilled * ppl));
      
        // calculate rep
        if (proximity < 0.5)
        {
            reputation += 10;
            goodServ++;
            good.Play();
            result = true;
        } else if (proximity < 1)
        {
            reputation += 5;
            goodServ++;
            good.Play();
            result = true;
        } else if (proximity < 3)
        {
            reputation -= 3;
            bad.Play();
            result = false;
        } else
        {
            reputation -= 5;
            bad.Play();
            result = false;
        }

        if (proximity < 1)
        {
            switch (level)
            {
                case 2:
                    if (goodServ == 2)
                    {
                        initSpawn = true;
                    }
                    else if (goodServ == 6)
                    {
                        ppl = 2.0f;
                    }
                    break;
                case 3:
                    if (goodServ == 3)
                    {
                        ppl = 1.5f;
                    }
                    else if (goodServ == 5)
                    {
                        initSpawn = true;
                    }
                    break;
                case 4:
                    if (goodServ == 2)
                    {
                        initSpawn = true;
                        ppl = 1.33f;
                    }
                    else if (goodServ == 5)
                    {
                        ppl = 1.66f;
                    }
                    break;
                case 5:
                    if (goodServ == 2)
                    {
                        initSpawn = true;
                    }
                    else if (goodServ == 3)
                    {
                        ppl = 1.5f;
                    }
                    else if (goodServ == 6)
                    {
                        ppl = 1.75f;
                        initSpawn = true;
                    }
                    else if (goodServ == 11)
                    {
                        initSpawn = true;
                        ppl = 2.0f;
                    }
                    break;
                case 6:
                    if (goodServ == 2)
                    {
                        initSpawn = true;
                        ppl = 1.5f;
                    }
                    else if (goodServ == 4)
                    {
                        ppl = 1.67f;
                        initSpawn = true;
                    }
                    else if (goodServ == 9)
                    {
                        ppl = 2.33f;
                        initSpawn = true;
                    }
                    break;
            }
        }

        // level transition
        if((level == 2 && goodServ == 12) || (level == 3 && goodServ == 12) || (level == 4 && goodServ == 12) || (level == 5 && goodServ == 16))
        {
            level++;
            // get the level amount
            GameObject levelTextObject = GameObject.Find("LevelText") as GameObject;
            if (levelTextObject != null)
            {
                levelTextObject.GetComponent<Text>().text = level.ToString();
            }
            SceneManager.LoadScene(level);
        }

        // scale health bar
        print("Rep" + reputation.ToString());
        
        bar.GetComponent<Transform>().localScale = new Vector3(barDim.x * Mathf.Clamp01((float) reputation / 100f), barDim.y, barDim.z);

        return result;
    }


    public void spawnIcon(bool good, string layerName)
    {
        GameObject spawner = null;
        switch (layerName)
        {
            case "Gas Station 1":
                spawner = iconSpawner1;
                break;
            case "Gas Station 2":
                spawner = iconSpawner2;
                break;
            case "Gas Station 3":
                spawner = iconSpawner3;
                break;
            case "Gas Station 4":
                spawner = iconSpawner4;
                break;
        }
        if (good)
        {
            Instantiate(goodIcon, spawner.GetComponent<Transform>().position, spawner.GetComponent<Transform>().rotation);
        } else
        {
            Instantiate(badIcon, spawner.GetComponent<Transform>().position, spawner.GetComponent<Transform>().rotation);
        }
        

    }


    public GameObject spawnBlank(string layerName)
    {
        GameObject spawner = null;
        GameObject spawnedBlank = null;
        switch (layerName)
        {
            case "Gas Station 1":
                spawner = blankSpawner1;
                break;
            case "Gas Station 2":
                spawner = blankSpawner2;
                break;
            case "Gas Station 3":
                spawner = blankSpawner3;
                break;
            case "Gas Station 4":
                spawner = blankSpawner4;
                break;
        }
        spawnedBlank = Instantiate(blank, spawner.GetComponent<Transform>().position, spawner.GetComponent<Transform>().rotation) as GameObject;
        return spawnedBlank;
    }

}
