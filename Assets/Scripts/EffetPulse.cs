using UnityEngine;

public class EffetPulse : MonoBehaviour
{
    public float vitesseBattement = 5f; // Vitesse du "boom boom"
    public float tailleMin = 0.8f;      // Taille au plus petit
    public float tailleMax = 1.2f;      // Taille au plus grand

    void Update()
    {
        // Mathf.Sin crée une vague fluide. On calcule une taille qui oscille entre le min et le max.
        // Le +1 et /2 permettent de ramener la valeur de Sinus (qui va de -1 à 1) entre 0 et 1.
        float variation = (Mathf.Sin(Time.time * vitesseBattement) + 1f) / 2f;
        float tailleActuelle = Mathf.Lerp(tailleMin, tailleMax, variation);
        
        // On applique la nouvelle taille à l'obstacle
        transform.localScale = new Vector3(tailleActuelle, tailleActuelle, 1f);
    }
}