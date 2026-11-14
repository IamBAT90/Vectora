using UnityEngine;

public class LaserRotatif : MonoBehaviour
{
    [Header("Paramètres")]
    public float vitesseRotation = 90f; // Vitesse en degrés par seconde. 
    // (Astuce : Mets une valeur négative pour le faire tourner dans l'autre sens)

    void Update()
    {
        // La rotation s'applique sur l'axe Z (le 3ème paramètre) en 2D.
        transform.Rotate(0f, 0f, vitesseRotation * Time.deltaTime);
    }
}