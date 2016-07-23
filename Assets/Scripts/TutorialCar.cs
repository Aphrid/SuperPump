using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;
using System;

public class TutorialCar : MonoBehaviour {

    private bool fueling;
    private bool animating;
    private bool userInput;

    private int lineNum;

    private GameObject speechBubble;
    private Text speechText;
    private Text nextText; // press any key to continue
    private Text mathText; // used to show equations

    private GameObject circle;
    private GameObject heart;
    private GameObject X;

    // money bubble
    private GameObject moneyBubble; // spawns the placeholder for money available
    private Text moneyText; // says MONEY above the bubble
    private Text moneyAmtText; // says $7.00 in the bubble

    // reputation bar & all text components
    private GameObject barFull;
    private GUIText pplText;
    private GUIText pplTextShadow;
    private GUIText repText;
    private GUIText repTextShadow;

    // current filling amt bubble & all text components
    private GameObject fillBubble;
    private GUIText currentText; // says CURRENT (L)
    private GUIText currentTextShadow;
    private GUIText fillAmtText; // says 1.001 (etc)
    private GUIText fillAmtTextShadow;

    private Button nextStageButton;
    private GameObject nextStageSprite;

    // stopwatch
    private Stopwatch curFuel;

    private AudioSource goodSound;
    private AudioSource badSound;
    private AudioSource talkSound;

    private static string math1 = "$6.00 ÷ $1.00/L = 6L";
    private static string math2 = "$6.00 ÷ $2.00/L = 3L";

    private string line1;
    private static string line2 = "See this 'ere bubble?                      \nI only got that much cash on me.";
    private static string line3 = "Each customer only wanna\nfill'er car up so much, \nya hear?";
    private static string line4 = "Now this sign up here?\nTells you how much each litre\nof fuel costs.";
    private static string line5 = "You gotta watch how much\nyer fillin'. Can't be givin'em \ntoo much or too little.";
    private static string line6 = "It's yer job to watch how \nmuch yer fillin'.";
    private static string line7 = "\n\nThat's how much you needa fill.";
    private static string line8 = "This thing right here shows you\nexactly how much you're \nfilling.";
    private static string line9 = "Let's wait a bit for this to\nfill up. Don'cha worry I'll\nhelp you out this one time.";
    private static string line10 = "You're pretty close now!                   \nTime to send the car off. \nIt's alright if you're a bit \nunder or over.";
    private static string line11 = "Well now, all you hafta' do\nis press the key that matches\nthe pump! Easy as that.\nTry it out!";
    private static string line12 = "Hear that? \nIt's a job well done if you're \nclose enough.";
    private static string line13 = "Let's try another one. \nLook at the price!";
    private static string line14 = "Aha! Since each litre costs \nmore, you need to fill less!";
    private static string line15 = "\n\nTry sendin' the car away.";
    private static string line16 = "Ouch! When ya over fill\na car, you also lose\nreputation!";
    private static string line17 = "You're fired if your \nreputation hits rock bottom!";
    private static string line18 = "That about covers yer training.\nGood luck and have fun!";    

    void Awake()
    {
        fueling = false;
        animating = false;
        userInput = false;
        lineNum = 1;
        curFuel = new Stopwatch();

        nextStageButton = GameObject.Find("NextStageButton").GetComponent<Button>();
        nextStageSprite = GameObject.Find("NextStage") as GameObject;

        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-8.0f, 0.0f);

        moneyBubble = GameObject.Find("MoneyBubble") as GameObject;
        speechBubble = GameObject.Find("SpeechBubble") as GameObject;
        fillBubble = GameObject.Find("FillBubble") as GameObject;
        heart = GameObject.Find("Heart") as GameObject;
        X = GameObject.Find("X") as GameObject;

        circle = GameObject.Find("CircleHighlight") as GameObject;

        GameObject goodAudioObject = GameObject.Find("GoodAudio") as GameObject;
        goodSound = goodAudioObject.GetComponent<AudioSource>();

        GameObject badAudioObject = GameObject.Find("BadAudio") as GameObject;
        badSound = badAudioObject.GetComponent<AudioSource>();

        talkSound = gameObject.GetComponent<AudioSource>();
        barFull = GameObject.Find("Bar Full") as GameObject;

        pplText = GameObject.Find("Price per Litre").GetComponent<GUIText>() as GUIText;
        pplTextShadow = GameObject.Find("Price per Litre Shadow").GetComponent<GUIText>() as GUIText;
        repText = GameObject.Find("Reputation").GetComponent<GUIText>() as GUIText;
        repTextShadow = GameObject.Find("Reputation Shadow").GetComponent<GUIText>() as GUIText;

        pplText.text = "Price: $1.00/L";
        pplTextShadow.text = "Price: $1.00/L";
        repText.text = "Reputation";
        repTextShadow.text = "Reputation";

        currentText = GameObject.Find("Car Fuel Current").GetComponent<GUIText>() as GUIText;
        currentTextShadow = GameObject.Find("Car Fuel Current Shadow").GetComponent<GUIText>() as GUIText;
        fillAmtText = GameObject.Find("Car Fuel Current AMT").GetComponent<GUIText>() as GUIText;
        fillAmtTextShadow = GameObject.Find("Car Fuel Current AMT Shadow").GetComponent<GUIText>() as GUIText;

        // gather text components from canvas
        GameObject speechTextObject = GameObject.Find("SpeechText") as GameObject;
        speechText = speechTextObject.GetComponent<Text>();

        GameObject nextTextObject = GameObject.Find("NextText") as GameObject;
        nextText = nextTextObject.GetComponent<Text>();

        GameObject moneyTextObject = GameObject.Find("MoneyText") as GameObject;
        moneyText = moneyTextObject.GetComponent<Text>();

        GameObject moneyAmtTextObject = GameObject.Find("MoneyAmt") as GameObject;
        moneyAmtText = moneyAmtTextObject.GetComponent<Text>();

        GameObject mathTextObject = GameObject.Find("MathText") as GameObject;
        mathText = mathTextObject.GetComponent<Text>();

        // user's name
        GameObject nameTextObject = GameObject.Find("NameText") as GameObject;
        if(nameTextObject == null) // check only because of scene
        {
            line1 = "First day on the job, eh?                  \nLet me show you a thing or two\nabout this beauty.";
        } else
        {
            line1 = "First day, huh " + nameTextObject.GetComponent<Text>().text + "? \nLet me show you a thing or two\nabout this beauty.";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        speechBubble.GetComponent<Transform>().localScale = new Vector3(6.3f, 6.3f, 1.0f);
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // Car has reached the station
        if (fueling && !animating)
        {
            // go to next line
            if (Input.anyKeyDown && !curFuel.IsRunning && !userInput && lineNum != 18)
            {
                lineNum++;
                if (lineNum == 2 || lineNum == 4 || lineNum == 6 || lineNum == 11 || lineNum == 13)
                {
                    StartCoroutine(FlashBubble());
                }

                if(lineNum == 7)
                {
                    StartCoroutine(ShowMath(1));
                } else if (lineNum == 8 || lineNum == 16)
                {
                    mathText.text = "";
                }

                if (lineNum == 9 || lineNum == 14)
                {
                    curFuel.Start();
                } else if (lineNum == 11)
                {
                    userInput = true;
                }

                StartCoroutine(ShowText());
            } else if (userInput && Input.GetKeyDown(KeyCode.Alpha1))
            {
                
                mathText.text = "";
                userInput = false;
                lineNum++;
                StartCoroutine(FlashBubble());
                StartCoroutine(ShowText());
                          
                if(lineNum <= 12)
                {                    
                    goodSound.Play();
                    heart.GetComponent<Transform>().localScale = new Vector3(6.5f, 6.5f, 1.0f);
                    heart.AddComponent<MoveAndDestroy>();
                } else
                {
                    badSound.Play();
                    X.GetComponent<Transform>().localScale = new Vector3(6.5f, 6.5f, 1.0f);
                    X.AddComponent<MoveAndDestroy>();

                    // reduce rep bar
                    barFull.GetComponent<Transform>().localScale = new Vector3(barFull.GetComponent<Transform>().localScale.x * 0.95f, barFull.GetComponent<Transform>().localScale.y, 0.0f);
                }
            }
        }

        if(fueling && lineNum >= 9 && curFuel.IsRunning)
        {
            fillAmtText.text = curFuel.Elapsed.Seconds.ToString() + "." + curFuel.Elapsed.Milliseconds.ToString();
            fillAmtTextShadow.text = curFuel.Elapsed.Seconds.ToString() + "." + curFuel.Elapsed.Milliseconds.ToString();
        }

        if (curFuel.ElapsedMilliseconds >= 5500L)
        {
            curFuel.Reset();
            fillAmtText.text = "5.500";
            fillAmtTextShadow.text = "5.500";
            lineNum++;
            StartCoroutine(ShowText());
            if (lineNum == 15)
            {
                StartCoroutine(ShowMath(2));
                userInput = true;
            }
        }

        // show button to be clickable
        if(lineNum == 18)
        {
            nextStageButton.interactable = true;
            nextStageSprite.GetComponent<Transform>().localScale = new Vector3(5f, 5f, 1f);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-10.0f, 0.0f);
        }

    }

    // flashes the money bubble and text
    IEnumerator FlashBubble()
    {
        // flash money
        while(lineNum == 2 || lineNum == 3)
        {           
            yield return new WaitForSeconds(0.7f);
            moneyAmtText.text = "$6.00";
            moneyText.text = "MONEY";
            moneyBubble.GetComponent<Transform>().localScale = new Vector3(14f, 9f, 1.0f);
            yield return new WaitForSeconds(0.7f);
            moneyAmtText.text = "";
            moneyText.text = "";
            moneyBubble.GetComponent<Transform>().localScale = Vector3.zero;
        }

        if(lineNum == 4)
        {
            moneyAmtText.text = "$6.00";
            moneyText.text = "MONEY";
            moneyBubble.GetComponent<Transform>().localScale = new Vector3(14f, 9f, 1.0f);
        }

        // flash price
        while(lineNum == 4 || lineNum == 5)
        {
            yield return new WaitForSeconds(0.7f);
            pplText.text = "Price: $1.00/L";
            pplTextShadow.text = "Price: $1.00/L";
            yield return new WaitForSeconds(0.7f);
            pplText.text = "";
            pplTextShadow.text = "";
        }

        if(lineNum == 6)
        {
            pplText.text = "Price: $1.00/L";
            pplTextShadow.text = "Price: $1.00/L";
        }

        // flash amount filled
        while(lineNum == 6 || lineNum == 7 || lineNum == 8)
        {
            yield return new WaitForSeconds(0.7f);
            fillBubble.GetComponent<Transform>().localScale = new Vector3(5f, 3.5f, 1.0f);
            currentText.text = "CURRENT (L)";
            currentTextShadow.text = "CURRENT (L)";
            fillAmtText.text = "0.000";
            fillAmtTextShadow.text = "0.000";

            yield return new WaitForSeconds(0.7f);
            fillBubble.GetComponent<Transform>().localScale = Vector3.zero;
            currentText.text = "";
            currentTextShadow.text = "";
            fillAmtText.text = "";
            fillAmtTextShadow.text = "";
           
        }

        if(lineNum == 9)
        {
            fillBubble.GetComponent<Transform>().localScale = new Vector3(5f, 3.5f, 1.0f);
            currentText.text = "CURRENT (L)";
            currentTextShadow.text = "CURRENT (L)";
            yield break;
        }   
    
        while(lineNum == 10 || lineNum == 11)
        {
            yield return new WaitForSeconds(0.7f);
            circle.GetComponent<Transform>().localScale = new Vector3(6f, 6f, 1.0f);
            yield return new WaitForSeconds(0.7f);
            circle.GetComponent<Transform>().localScale = Vector3.zero;
        }

        if(lineNum == 12)
        {
            circle.GetComponent<Transform>().localScale = Vector3.zero;
        }

        // flash price
        while (lineNum == 13)
        {
            yield return new WaitForSeconds(0.7f);
            pplText.text = "Price: $2.00/L";
            pplTextShadow.text = "Price: $2.00/L";
            yield return new WaitForSeconds(0.7f);
            pplText.text = "";
            pplTextShadow.text = "";
        }

        if (lineNum == 14)
        {
            pplText.text = "Price: $2.00/L";
            pplTextShadow.text = "Price: $2.00/L";
        }

        yield break;
    }


    IEnumerator ShowMath(int num)
    {
        string math = "";
        if(num == 1)
        {
            math = math1;
        } else
        {
            math = math2;
        }

        for (int i = 0; i < math.Length + 1; i++)
        {
            mathText.text = math.Substring(0, i);
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator ShowText()
    {
        string line = "Placeholder for lineNum = " + lineNum.ToString();
        animating = true;
        nextText.text = "";

        switch (lineNum) {
            case 1:
                line = line1;
                break;
            case 2:
                line = line2;
                break;
            case 3:
                line = line3;
                break;
            case 4:
                line = line4;
                break;
            case 5:
                line = line5;
                break;
            case 6:
                line = line6;
                break;
            case 7:
                line = line7;
                break;
            case 8:
                line = line8;
                break;
            case 9:
                line = line9;
                break;
            case 10:
                line = line10;
                break;
            case 11:
                line = line11;
                break;
            case 12:
                line = line12;
                break;
            case 13:
                line = line13;
                break;
            case 14:
                line = line14;
                break;
            case 15:
                line = line15;
                break;
            case 16:
                line = line16;
                break;
            case 17:
                line = line17;
                break;
            case 18:
                line = line18;
                break;
            default:
                break;
        }
        
        for (int i = 0; i < line.Length + 1; i++)
        {
            speechText.text = line.Substring(0, i);
            if((i < line.Length) && (i % 2 == 0 && !Char.IsWhiteSpace(line[i])))
            {
                talkSound.Play();
            }
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.35f);

        if(lineNum != 9 && lineNum != 11 && lineNum != 15 && lineNum != 14 && lineNum != 18)
        {
            nextText.text = "Press any key to continue.";
        }        

        fueling = true;
        animating = false;
        yield break;
    }
}
