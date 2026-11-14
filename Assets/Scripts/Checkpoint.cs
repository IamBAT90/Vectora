using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie si c'est bien le joueur qui traverse la zone
        if (collision.CompareTag("Player"))
        {
            GestionMortJoueur gestionMort = collision.GetComponent<GestionMortJoueur>();
            if (gestionMort != null)
            {
                // On enregistre la position centrale du checkpoint comme point de réapparition
                gestionMort.DefinirCheckpoint(transform.position);
            }
        }
    }
}
