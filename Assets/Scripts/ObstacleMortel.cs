using UnityEngine;
using UnityEngine.SceneManagement; // Permet de gérer le rechargement du niveau

public class ObstacleMortel : MonoBehaviour
{
    // Cette fonction se déclenche automatiquement quand un objet entre dans le Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie si l'objet qui nous touche a bien le tag "Player"
        if (collision.CompareTag("Player"))
        {
            // On recharge la scène actuelle depuis le début (Game Over)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}