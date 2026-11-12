using UnityEngine;

public class ObstaclePatrouille : MonoBehaviour
{
    [Header("Points de passage")]
    public Transform pointA;
    public Transform pointB;
    
    [Header("Paramètres")]
    public float vitesse = 5f;

    private Vector2 cibleActuelle;

    void Start()
    {
        // Au démarrage, l'obstacle se détache de ses points (pour éviter des bugs si tu les as mis en enfants)
        // Et on définit le point B comme première destination
        cibleActuelle = pointB.position;
    }

    void Update()
    {
        // On déplace l'obstacle vers la cible actuelle à vitesse constante
        transform.position = Vector2.MoveTowards(transform.position, cibleActuelle, vitesse * Time.deltaTime);

        // Si l'obstacle est arrivé à destination (avec une infime marge d'erreur de 0.1f)
        if (Vector2.Distance(transform.position, cibleActuelle) < 0.1f)
        {
            // On inverse la cible ! S'il était sur A, il va vers B, et inversement.
            if (cibleActuelle == (Vector2)pointA.position)
            {
                cibleActuelle = pointB.position;
            }
            else
            {
                cibleActuelle = pointA.position;
            }
        }
    }
}