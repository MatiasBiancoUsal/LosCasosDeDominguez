using UnityEngine;
using UnityEngine.UI;

public class NestInteraction : MonoBehaviour
{
    public Slider progressBar;

    public SpriteRenderer nestRenderer;
    public Sprite emptyNestSprite;


    public float holdTime = 3f;

    private float currentHold = 0f;

    private bool playerInside = false;
    private bool completed = false;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.value = 0;
            progressBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (completed)
            return;

        if (!playerInside)
            return;

        if (Input.GetKey(KeyCode.Q))
        {
            currentHold += Time.deltaTime;

            progressBar.value = currentHold / holdTime;

            if (currentHold >= holdTime)
            {
                RobEgg();
            }
        }
        else
        {
            currentHold = 0;
            progressBar.value = 0;
        }
    }

    void RobEgg()
    {
        completed = true;

        Debug.Log("¡Robaste el huevo!");

        progressBar.gameObject.SetActive(false);

        if (nestRenderer != null && emptyNestSprite != null)
        {
            nestRenderer.sprite = emptyNestSprite;
        }

        GetComponent<Collider2D>().enabled = false;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            progressBar.gameObject.SetActive(true);


        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            currentHold = 0;

            progressBar.value = 0;

            progressBar.gameObject.SetActive(false);
        }
    }
}