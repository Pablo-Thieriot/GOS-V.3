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
    public SpriteRenderer spriteRenderer; // Référence au SpriteRenderer

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();  // Récupère les entrées de mouvement
        UpdateAnimation();  // Met à jour les animations
    }

    private void FixedUpdate()
    {
        Move();  // Déplace le joueur
    }

    // Récupère les entrées de mouvement
    void ProcessInput()
    {
        float MoveX = Input.GetAxis("Horizontal");  // Mouvement horizontal (gauche/droite)
        float MoveY = Input.GetAxis("Vertical");    // Mouvement vertical (haut/bas)

        moveDirection = new Vector2(MoveX, MoveY);

        // Mettre à jour les paramètres d'animation
        animator.SetFloat("MoveX", MoveX);  // Définir la direction horizontale pour les animations
        animator.SetFloat("MoveY", MoveY);  // Définir la direction verticale pour les animations
    }

    // Déplace le joueur en fonction de la direction
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    // Gère les animations en fonction des entrées
    void UpdateAnimation()
    {
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            // Si le joueur se déplace, alors lance les animations de déplacement
            if (moveDirection.x < 0)
            {
                animator.SetTrigger("walk_left_player");  // Déclenche l'animation de marche vers la gauche
            }
            else if (moveDirection.x > 0)
            {
                animator.SetTrigger("walk_right_player");  // Déclenche l'animation de marche vers la droite
            }
            else if (moveDirection.y < 0)
            {
                animator.SetTrigger("walk_down_player");  // Déclenche l'animation de marche vers le bas
            }
            else if (moveDirection.y > 0)
            {
                animator.SetTrigger("walk_up_player");  // Déclenche l'animation de marche vers le haut
            }
        }
        else
        {
            // Si le joueur ne se déplace pas, lance l'animation d'inactivité
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
        // Durée de l'animation de mort (à ajuster selon ta timeline dans Unity)
        float deathAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).length + 1f;

        // Attendre que l'animation de mort soit terminée
        yield return new WaitForSeconds(deathAnimationDuration);

        // Commencer l'effet de zoom et de transparence
        float effectDuration = 1.5f; // Durée de l'effet
        float elapsedTime = 0f;      // Temps écoulé
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

            elapsedTime += Time.deltaTime; // Incrémenter le temps écoulé
            yield return null;             // Attendre la prochaine frame
        }

        // S'assurer que l'état final est appliqué
        transform.localScale = originalScale * 2f;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Action suivante (par exemple, recharger une scène)
        SceneManager.LoadScene("endScene");
    }

}
