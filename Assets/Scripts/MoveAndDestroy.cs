using UnityEngine;
using System.Collections;

public class MoveAndDestroy : MonoBehaviour {

    private Rigidbody2D rb;

	void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(slowDown()); 
        Destroy(gameObject, 1.4f);
    }

    IEnumerator slowDown()
    {
        rb.velocity = new Vector2(0.0f, 1.7f);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(0.0f, 1.4f);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(0.0f, 1.1f);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(0.0f, 0.7f);
    }
}
