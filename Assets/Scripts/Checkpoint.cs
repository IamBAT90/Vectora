using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Visuels Néon")]
    public Material materialAllume; // Ton Mat_Neon_Checkpoint (HDR)
    public ParticleSystem particulesActivation;

    [Header("Effet de Rebond")]
    public float multiplicateurRebond = 1.35f;
    public float dureeRebond = 0.15f;

    private SpriteRenderer spriteRenderer;
    private bool estActive = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie que c'est bien le joueur et que le checkpoint n'est pas déjà activé
        if (collision.CompareTag("Player") && !estActive)
        {
            GestionMortJoueur gestionMort = collision.GetComponent<GestionMortJoueur>();
            if (gestionMort != null)
            {
                // On enregistre la position centrale du checkpoint comme point de réapparition
                gestionMort.DefinirCheckpoint(transform.position);
                ActiverVisuel();
            }
        }
    }

    private void ActiverVisuel()
    {
        estActive = true;

        // 1. Allume le néon (déclenche le Bloom sélectif)
        if (spriteRenderer != null && materialAllume != null)
        {
            spriteRenderer.material = materialAllume;
            spriteRenderer.color = Color.white;
        }

        // 2. Déclenche les particules
        if (particulesActivation != null)
        {
            particulesActivation.Play();
        }

        // 3. Lance l'effet de rebond
        StartCoroutine(EffetRebond());
    }

    private IEnumerator EffetRebond()
    {
        Vector3 tailleNormale = transform.localScale;
        Vector3 tailleMax = tailleNormale * multiplicateurRebond;

        float t = 0f;
        while (t < dureeRebond)
        {
            t += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(tailleNormale, tailleMax, t / dureeRebond);
            yield return null;
        }

        t = 0f;
        while (t < dureeRebond)
        {
            t += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(tailleMax, tailleNormale, t / dureeRebond);
            yield return null;
        }

        transform.localScale = tailleNormale;
    }
}