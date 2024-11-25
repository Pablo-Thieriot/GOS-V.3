using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movementEnnemi1 : MonoBehaviour
{
    public float speed;
    public Rigidbody2D ennemiRB;
    public GameObject target;
    public Collision2D player;
    private Vector2 spawnPosition;
    private Vector2 direction;
    public Vector3 scaleChange;
    public GameObject player1;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3 (2, 2, 2f);
        GivePosition();
        this.gameObject.transform.position = spawnPosition;
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        reduceScale();
    }

    void Move()
    {
        direction = (target.transform.position - transform.position).normalized;
        ennemiRB.velocity = direction * speed;
    }
    void GivePosition()
    {
        float safeDistance = 5f; // Distance minimale entre le joueur et l'ennemi
        do
        {
            float positionX = Random.Range(57.54f, 70.68f);
            float positionY = Random.Range(23.03f, 35.51f); // Attention à l'ordre des valeurs min/max
            spawnPosition = new Vector2(positionX, positionY);
        }
        while (Vector2.Distance(spawnPosition, player1.transform.position) < safeDistance);
    }


    void reduceScale()
    {
        transform.localScale += scaleChange;
        if(transform.localScale.magnitude < 0.001)
        {
            this.gameObject.SetActive(false);
            Start();
        }
    }


}
