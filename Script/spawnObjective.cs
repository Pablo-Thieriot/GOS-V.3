using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawn : MonoBehaviour
{
    private Vector2 spawnPosition;
    public AudioClip pickupSound;
    public GameObject ennemi;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        ennemi.SetActive(false);
        GivePosition();
        this.gameObject.transform.position = spawnPosition;
        this.gameObject.SetActive(true);
        /*this.gameObject.SetActive(false);*/
    }

    // Update is called once per frame
    void Update()
    {
        if(score > 0)
        {
            ennemi.SetActive(true);
        }
        if (!this.gameObject.activeSelf)
        {
            GivePosition();
            this.gameObject.transform.position = spawnPosition;
            this.gameObject.SetActive(true);
        }
    }

    void GivePosition()
    {
        float positionX = Random.Range(57.54f, 70.68f);
        float positionY = Random.Range(35.51f, 23.03f);

        spawnPosition = new Vector2(positionX, positionY);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.name == "player")
        {
            gameObject.SetActive(false);
            score += 1;
            string scoreString = score.ToString();
            GameObject.Find("score").GetComponent<Text>().text = scoreString;
            GivePosition();
            this.gameObject.transform.position = spawnPosition;
            this.gameObject.SetActive(true);
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.clip = pickupSound;

            audioSource.priority = 0;

            audioSource.Play();

            Destroy(audioSource, pickupSound.length);
        }
    }
}

