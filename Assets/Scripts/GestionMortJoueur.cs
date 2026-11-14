using System.Collections;
using UnityEngine;

public class GestionMortJoueur : MonoBehaviour
{
    [Header("Paramètres de mort")]
    public float tempsAvantReapparition = 1f;
    public GameObject prefabParticulesMort; // Optionnel : un effet visuel d'explosion

    private Vector2 pointDeReapparition;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private MouvementJoueur mouvementScript;
    
    private bool estMort = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        mouvementScript = GetComponent<MouvementJoueur>();
        
        // Le premier checkpoint est simplement l'endroit où le joueur commence le niveau
        pointDeReapparition = transform.position;
    }

    public void DefinirCheckpoint(Vector2 nouveauPoint)
    {
        pointDeReapparition = nouveauPoint;
    }

    public void Mourir()
    {
        if (estMort) return; // Sécurité pour éviter de mourir 50 fois par seconde dans un laser
        StartCoroutine(SequenceMort());
    }

    IEnumerator SequenceMort()
    {
        estMort = true;
        
        // 1. Gros tremblement d'écran pour l'impact visuel !
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Secouer(0.2f, 0.6f);
        }

        // 2. Effet visuel (si tu crées un petit Prefab d'explosion plus tard)
        if (prefabParticulesMort != null)
        {
            Instantiate(prefabParticulesMort, transform.position, Quaternion.identity);
        }

        // 3. L'illusion : on désactive le joueur sans le supprimer
        spriteRenderer.enabled = false;   // Il devient invisible
        rb.simulated = false;             // Les obstacles passent au travers
        mouvementScript.enabled = false;  // Il ne peut plus bouger ni utiliser de pouvoirs

        // 4. On attend que le joueur digère son erreur
        yield return new WaitForSeconds(tempsAvantReapparition);

        // 5. On le téléporte au dernier Checkpoint
        transform.position = pointDeReapparition;
        rb.position = pointDeReapparition;
        
        // 6. On réactive tout
        spriteRenderer.enabled = true;
        rb.simulated = true;
        mouvementScript.enabled = true;
        
        estMort = false;
    }
}