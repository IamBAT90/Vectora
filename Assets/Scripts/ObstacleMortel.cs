using UnityEngine;

public class ObstacleMortel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // On cherche le nouveau gestionnaire de mort sur le joueur
            GestionMortJoueur gestionMort = collision.GetComponent<GestionMortJoueur>();
            if (gestionMort != null)
            {
                gestionMort.Mourir(); // Déclenche la séquence dramatique !
            }
        }
    }
}