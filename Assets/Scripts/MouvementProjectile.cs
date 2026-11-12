using UnityEngine;

public class MouvementProjectile : MonoBehaviour
{
    [Header("Paramètres")]
    public float vitesse = 8f;
    public float dureeVie = 5f;

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, dureeVie);
    }

    public void DefinirDirection(Vector2 nouvelleDirection)
    {
        direction = nouvelleDirection.normalized;

        // --- NOUVEAUTÉ : LA ROTATION ---
        // 1. On calcule l'angle mathématique vers le joueur en degrés
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 2. On applique cette rotation sur l'axe Z (celui de la 2D).
        // L'astuce : On fait "angle - 90f" car l'image de la Capsule 2D dans Unity 
        // pointe naturellement vers le HAUT (axe Y). La formule mathématique, elle, 
        // considère que l'avant est vers la DROITE (axe X).
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void Update()
    {
        // --- NOUVEAUTÉ : LE DÉPLACEMENT ---
        // On modifie directement la position globale pour que la rotation
        // n'interfère pas avec la trajectoire droite de la balle !
        transform.position += (Vector3)(direction * vitesse * Time.deltaTime);
    }
}