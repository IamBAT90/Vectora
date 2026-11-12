using UnityEngine;

public class MouvementProjectile : MonoBehaviour
{
    [Header("Paramètres")]
    public float vitesse = 8f;
    public float dureeVie = 5f; // Détruit le projectile après 5s (optimisation)

    private Vector2 direction;

    void Start()
    {
        // Enclenche un compte à rebours de destruction dès son apparition
        Destroy(gameObject, dureeVie);
    }

    // Fonction appelée par la tourelle pour donner la direction cible
    public void DefinirDirection(Vector2 nouvelleDirection)
    {
        direction = nouvelleDirection.normalized;
    }

    void Update()
    {
        // Déplace le projectile à chaque frame
        transform.Translate(direction * vitesse * Time.deltaTime);
    }
}