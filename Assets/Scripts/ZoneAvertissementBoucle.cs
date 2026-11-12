using System.Collections;
using UnityEngine;

public class ZoneAvertissementBoucle : MonoBehaviour
{
    [Header("Temps (en secondes)")]
    public float tempsRepos = 2f; // Le temps où la zone est désactivée et traversable
    public float tempsAvertissement = 1f;
    public float dureeAttaque = 1f;

    [Header("Visuels")]
    public Color couleurRepos = new Color(1f, 1f, 1f, 0f); // Invisible par défaut
    public Color couleurAvertissement = new Color(1f, 0f, 0f, 0.3f);
    public Color couleurMortelle = new Color(1f, 0f, 0f, 1f);

    private SpriteRenderer spriteRenderer;
    private Collider2D zoneMortelle;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        zoneMortelle = GetComponent<Collider2D>();

        // On lance la séquence en boucle
        StartCoroutine(SequenceAttaqueBoucle());
    }

    IEnumerator SequenceAttaqueBoucle()
    {
        // Le "while (true)" permet de répéter ce bloc à l'infini
        while (true)
        {
            // 1. PHASE DE REPOS (Désactivée)
            zoneMortelle.enabled = false;
            spriteRenderer.color = couleurRepos;
            yield return new WaitForSeconds(tempsRepos);

            // 2. PHASE D'AVERTISSEMENT
            spriteRenderer.color = couleurAvertissement;
            yield return new WaitForSeconds(tempsAvertissement);

            // 3. PHASE D'ATTAQUE
            zoneMortelle.enabled = true;
            spriteRenderer.color = couleurMortelle;

            // NOUVEAUTÉ : On vérifie si l'objet est visible à l'écran avant de secouer !
            if (spriteRenderer.isVisible && CameraShake.Instance != null)
            {
                CameraShake.Instance.Secouer(0.1f, 0.2f);
            }

            yield return new WaitForSeconds(dureeAttaque);
            
            // Une fois l'attaque finie, la boucle recommence à l'étape 1 !
        }
    }
}