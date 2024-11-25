using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movementPlayer : MonoBehaviour
{
    public Rigidbody2D rb;  // Le Rigidbody2D du joueur
    public float speed;     // Vitesse du joueur
    private Vector2 moveDirection;  // Direction de mouvement
    public Animator animator;  // L'Animator du joueur
    private bool test = false;
    public SpriteRenderer spriteRenderer; // R�f�rence au SpriteRenderer

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();  // R�cup�re les entr�es de mouvement
        UpdateAnimation();  // Met � jour les animations
    }

    private void FixedUpdate()
    {
        Move();  // D�place le joueur
    }

    // R�cup�re les entr�es de mouvement
    void ProcessInput()
    {
        float MoveX = Input.GetAxis("Horizontal");  // Mouvement horizontal (gauche/droite)
        float MoveY = Input.GetAxis("Vertical");    // Mouvement vertical (haut/bas)

        moveDirection = new Vector2(MoveX, MoveY);

        // Mettre � jour les param�tres d'animation
        animator.SetFloat("MoveX", MoveX);  // D�finir la direction horizontale pour les animations
        animator.SetFloat("MoveY", MoveY);  // D�finir la direction verticale pour les animations
    }

    // D�place le joueur en fonction de la direction
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    // G�re les animations en fonction des entr�es
    void UpdateAnimation()
    {
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            // Si le joueur se d�place, alors lance les animations de d�placement
            if (moveDirection.x < 0)
            {
                animator.SetTrigger("walk_left_player");  // D�clenche l'animation de marche vers la gauche
            }
            else if (moveDirection.x > 0)
            {
                animator.SetTrigger("walk_right_player");  // D�clenche l'animation de marche vers la droite
            }
            else if (moveDirection.y < 0)
            {
                animator.SetTrigger("walk_down_player");  // D�clenche l'animation de marche vers le bas
            }
            else if (moveDirection.y > 0)
            {
                animator.SetTrigger("walk_up_player");  // D�clenche l'animation de marche vers le haut
            }
        }
        else
        {
            // Si le joueur ne se d�place pas, lance l'animation d'inactivit�
            animator.SetTrigger("idle_player");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "ennemi1")
        {
            if(!test)
            {
                animator.SetTrigger("die");
                rb.bodyType = RigidbodyType2D.Static;
                test = true;
                StartCoroutine(HandleDeath());
            }

        }
    }

    private IEnumerator HandleDeath()
    {
        // Dur�e de l'animation de mort (� ajuster selon ta timeline dans Unity)
        float deathAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).length + 1f;

        // Attendre que l'animation de mort soit termin�e
        yield return new WaitForSeconds(deathAnimationDuration);

        // Commencer l'effet de zoom et de transparence
        float effectDuration = 1.5f; // Dur�e de l'effet
        float elapsedTime = 0f;      // Temps �coul�
        Vector3 originalScale = transform.localScale; // Taille initiale
        Color originalColor = spriteRenderer.color;   // Couleur initiale

        while (elapsedTime < effectDuration)
        {
            // Calcul du ratio de progression
            float t = elapsedTime / effectDuration;

            // Augmenter la taille
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 20f, t);

            // Rendre de plus en plus transparent
            spriteRenderer.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                Mathf.Lerp(1f, 0f, t)
            );

            elapsedTime += Time.deltaTime; // Incr�menter le temps �coul�
            yield return null;             // Attendre la prochaine frame
        }

        // S'assurer que l'�tat final est appliqu�
        transform.localScale = originalScale * 2f;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Action suivante (par exemple, recharger une sc�ne)
        SceneManager.LoadScene("endScene");
    }

}
