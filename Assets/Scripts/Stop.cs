using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Stop : MonoBehaviour {

    private bool fueling;

    private GameController gameController;
    private GameObject blank;

    // THIS IS IN DOLLARS, NOT FUEL AMOUNT
    private GUIText carMoneyLabel;
    private GUIText carMoneyLabelShadow;

    private GUIText carMoneyAmt;
    private GUIText carMoneyAmtShadow;

    private GUIText curFuelAmt;
    private GUIText curFuelAmtShadow;
    private Stopwatch curFuel;

    private float carMoney;
    private Rigidbody2D rb;

    // Creats the "amount of fuel demand" 
    void GenCarMoney()
    {
        int amtCars = gameController.GetGoodServ();

        if(amtCars < 2)
        {
            // Level 1
            carMoney = Mathf.Round(Random.Range(4.0f, 8.0f));
        } else if (amtCars < 7)
        {
            // Level 2
            carMoney = Mathf.Round(Random.Range(8.0f, 22.0f)) / 2.0f;
        } else if (amtCars < 15)
        {
            // Level 3
            carMoney = Mathf.Round(Random.Range(8.0f, 35.0f)) / 4.0f;
        }
    }

    void Awake()
    {
        fueling = false;
        curFuel = new Stopwatch();

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = new Vector2(Random.Range(-10.0f, -5.0f), 0.0f);

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        GenCarMoney();
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().sortingLayerID.Equals(gameObject.GetComponent<SpriteRenderer>().sortingLayerID))
        {
            blank = gameController.spawnBlank(gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
            if (gameObject.GetComponent<SpriteRenderer>().sortingLayerName.Equals("Gas Station 2") || gameObject.GetComponent<SpriteRenderer>().sortingLayerName.Equals("Gas Station 4"))
            {
                blank.GetComponent<Transform>().localScale = new Vector2(-blank.GetComponent<Transform>().localScale.x, blank.GetComponent<Transform>().localScale.y);
            }            
        }

        UnityEngine.Debug.Log("This tag: " + gameObject.tag + " | Other tag: " + other.gameObject.tag);

        // only stop if the colliding object is on the same sorting layer as the car
        if (other.gameObject.tag.Equals("GameController") || (other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName.Equals(gameObject.GetComponent<SpriteRenderer>().sortingLayerName) && !other.gameObject.tag.Equals("Untagged")))
        {
            rb.velocity = Vector2.zero;
            fueling = true;

            switch (other.tag)
            {
                case "Gas Station 1":
                    carMoneyLabel = GameObject.Find("Car Fuel Target Text").GetComponent<GUIText>() as GUIText;
                    carMoneyLabelShadow = GameObject.Find("Car Fuel Target Text Shadow").GetComponent<GUIText>() as GUIText;
                    carMoneyAmt = GameObject.Find("Car Fuel Target").GetComponent<GUIText>() as GUIText;
                    carMoneyAmtShadow = GameObject.Find("Car Fuel Target Shadow").GetComponent<GUIText>() as GUIText;
                    curFuelAmt = GameObject.Find("Car Fuel Current AMT").GetComponent<GUIText>() as GUIText;
                    curFuelAmtShadow = GameObject.Find("Car Fuel Current AMT Shadow").GetComponent<GUIText>() as GUIText;
                    gameObject.tag = "car1";
                    break;
                case "Gas Station 2":
                    carMoneyLabel = GameObject.Find("Car 2 Fuel Target Text").GetComponent<GUIText>() as GUIText;
                    carMoneyLabelShadow = GameObject.Find("Car 2 Fuel Target Text Shadow").GetComponent<GUIText>() as GUIText;
                    carMoneyAmt = GameObject.Find("Car 2 Fuel Target").GetComponent<GUIText>() as GUIText;
                    carMoneyAmtShadow = GameObject.Find("Car 2 Fuel Target Shadow").GetComponent<GUIText>() as GUIText;
                    curFuelAmt = GameObject.Find("Car 2 Fuel Current AMT").GetComponent<GUIText>() as GUIText;
                    curFuelAmtShadow = GameObject.Find("Car 2 Fuel Current AMT Shadow").GetComponent<GUIText>() as GUIText;
                    gameObject.tag = "car2";
                    break;
                case "Gas Station 3":
                    carMoneyLabel = GameObject.Find("Car 3 Fuel Target Text").GetComponent<GUIText>() as GUIText;
                    carMoneyLabelShadow = GameObject.Find("Car 3 Fuel Target Text Shadow").GetComponent<GUIText>() as GUIText;
                    carMoneyAmt = GameObject.Find("Car 3 Fuel Target").GetComponent<GUIText>() as GUIText;
                    carMoneyAmtShadow = GameObject.Find("Car 3 Fuel Target Shadow").GetComponent<GUIText>() as GUIText;
                    curFuelAmt = GameObject.Find("Car 3 Fuel Current AMT").GetComponent<GUIText>() as GUIText;
                    curFuelAmtShadow = GameObject.Find("Car 3 Fuel Current AMT Shadow").GetComponent<GUIText>() as GUIText;
                    gameObject.tag = "car3";
                    break;
                case "Gas Station 4":
                    carMoneyLabel = GameObject.Find("Car 4 Fuel Target Text").GetComponent<GUIText>() as GUIText;
                    carMoneyLabelShadow = GameObject.Find("Car 4 Fuel Target Text Shadow").GetComponent<GUIText>() as GUIText;
                    carMoneyAmt = GameObject.Find("Car 4 Fuel Target").GetComponent<GUIText>() as GUIText;
                    carMoneyAmtShadow = GameObject.Find("Car 4 Fuel Target Shadow").GetComponent<GUIText>() as GUIText;
                    curFuelAmt = GameObject.Find("Car 4 Fuel Current AMT").GetComponent<GUIText>() as GUIText;
                    curFuelAmtShadow = GameObject.Find("Car 4 Fuel Current AMT Shadow").GetComponent<GUIText>() as GUIText;
                    gameObject.tag = "car4";
                    break;
            }            
        }
    }

    void Update()
    {
        if (fueling)
        {            
            curFuel.Start();
            carMoneyAmt.text = carMoney.ToString("C2");
            carMoneyAmtShadow.text = carMoney.ToString("C2");
            carMoneyLabel.text = "MONEY";
            carMoneyLabelShadow.text = "MONEY";
            //curFuelLabel.text = "Current:";
            //curFuelLabelShadow.text = "Current:";
            curFuelAmt.text = curFuel.Elapsed.Seconds.ToString() + "." + curFuel.Elapsed.Milliseconds.ToString();
            curFuelAmtShadow.text = curFuel.Elapsed.Seconds.ToString() + "." + curFuel.Elapsed.Milliseconds.ToString();
        }

        if ((Input.GetKeyDown(KeyCode.Alpha1) && fueling && gameObject.tag.Equals("car1")) 
            || (Input.GetKeyDown(KeyCode.Alpha2) && fueling && gameObject.tag.Equals("car2"))
            || (Input.GetKeyDown(KeyCode.Alpha3) && fueling && gameObject.tag.Equals("car3"))
            || (Input.GetKeyDown(KeyCode.Alpha4) && fueling && gameObject.tag.Equals("car4")))
        {
            curFuel.Stop();
            fueling = false;
            Destroy(blank);            
            bool good = gameController.CalculateRep(carMoney, (float) curFuel.Elapsed.TotalSeconds);

            // start the car again
            rb.velocity = new Vector2(Random.Range(-10.0f, -5.0f), 0.0f);

            // blank out the text
            carMoneyLabel.text = "";
            carMoneyLabelShadow.text = "";
            carMoneyAmt.text = "";
            carMoneyAmtShadow.text = "";

            // spawn good icon or bad icon
            gameController.spawnIcon(good, gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
        }
    }
}
