using UnityEngine;

public class SuiviCamera : MonoBehaviour
{
    [Header("Paramètres")]
    public Transform cible; // Le joueur à suivre
    public float vitesseLissage = 5f; // La vitesse à laquelle la caméra rattrape le joueur
    public Vector3 decalage = new Vector3(0f, 0f, -10f); // Le recul de la caméra

    // On utilise LateUpdate pour la caméra, car elle s'exécute APRES le déplacement du joueur
    void LateUpdate()
    {
        // On vérifie que le joueur existe bien (au cas où il serait détruit)
        if (cible != null)
        {
            // La position où la caméra aimerait aller
            Vector3 positionDesiree = cible.position + decalage;

            // Vector3.Lerp crée un mouvement fluide de la position actuelle vers la position désirée
            transform.position = Vector3.Lerp(transform.position, positionDesiree, vitesseLissage * Time.deltaTime);
        }
    }
}