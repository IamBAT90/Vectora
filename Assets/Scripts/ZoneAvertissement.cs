using System.Collections;
using UnityEngine;

public class ZoneAvertissement : MonoBehaviour
{
    [Header("Temps (en secondes)")]
    public float tempsAvertissement = 1.5f; // Durée de la prévention
    public float dureeAttaque = 1f; // Durée pendant laquelle la zone blesse

    [Header("Visuels")]
    public Color couleurAvertissement = new Color(1f, 0f, 0f, 0.3f); // Rouge transparent
    public Color couleurMortelle = new Color(1f, 0f, 0f, 1f); // Rouge vif

    private SpriteRenderer spriteRenderer;
    private Collider2D zoneMortelle;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        zoneMortelle = GetComponent<Collider2D>();

        // On lance la séquence dès que l'objet est activé
        StartCoroutine(SequenceAttaque());
    }

    IEnumerator SequenceAttaque()
    {
        // 1. PHASE D'AVERTISSEMENT
        zoneMortelle.enabled = false; // Le collider est désactivé, on peut passer au travers
        spriteRenderer.color = couleurAvertissement;

        // Le code fait une pause ici pendant "tempsAvertissement" secondes
        yield return new WaitForSeconds(tempsAvertissement);

        // 2. PHASE D'ATTAQUE
        zoneMortelle.enabled = true; // Le collider s'active (et tuera le joueur s'il a ton script ObstacleMortel)
        spriteRenderer.color = couleurMortelle;

        // Le code fait une pause pendant l'attaque
        yield return new WaitForSeconds(dureeAttaque);

        // 3. FIN DE L'ATTAQUE
        // L'objet se détruit de lui-même une fois l'attaque terminée
        Destroy(gameObject); 
    }
}