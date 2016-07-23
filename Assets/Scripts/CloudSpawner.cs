using UnityEngine;
using System.Collections;
using System.Globalization;

/*
 * Moves the title screen camera down and spawns clouds
 * 
 */
public class CloudSpawner : MonoBehaviour {

    public GameObject cloud1;
    public GameObject cloud2;
    public GameObject cloud3;

    public Camera cam;
    private bool cameraMove;

    public Vector2 spawnPositions;

    // Use this for initialization
    void Start () {
        cameraMove = true;
        Random.seed = System.Environment.TickCount;
        StartCoroutine(SpawnClouds()); 
	}

    // Move the camera initially
    void Update()
    {
        if (cameraMove)
        {
            cam.GetComponent<Transform>().position = new Vector3(0f, cam.GetComponent<Transform>().position.y - 0.075f, -10.0f);
            if (cam.GetComponent<Transform>().position.y <= 1.0f)
            {
                cameraMove = false;
            }
        }
    }


    IEnumerator SpawnClouds()
    {
        while (true)
        {
            Vector2 spawnSpot = new Vector2(spawnPositions.x, Random.Range(spawnPositions.y, spawnPositions.y + 3.0f));

            float randVal = Random.Range(0.0f, 3.0f);
            GameObject cloud;
            GameObject spawnedCloud;

            // Choose random cloud
            if(randVal <= 1.0f)
            {
                cloud = cloud1;
            } else if (randVal <= 2.0f)
            {
                cloud = cloud2;
            } else
            {
                cloud = cloud3;
            }

            spawnedCloud = Instantiate(cloud, spawnSpot, gameObject.GetComponent<Transform>().rotation) as GameObject;
            Rigidbody2D rb = spawnedCloud.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = new Vector2(Random.Range(-1.5f, -0.5f), 0.0f);

            yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));
        }
    }

}
