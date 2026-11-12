using UnityEngine;

public class Tourelle : MonoBehaviour
{
    [Header("Paramètres de Tir")]
    public GameObject prefabProjectile; 
    public Transform pointDeTir; 
    public float cadenceDeTir = 2f; 

    [Header("Paramètres de Détection")]
    public float portee = 10f; // La distance maximale à laquelle la tourelle repère le joueur

    private Transform joueur;
    private float timerTir;

    void Start()
    {
        GameObject objetJoueur = GameObject.FindGameObjectWithTag("Player");
        if (objetJoueur != null)
        {
            joueur = objetJoueur.transform;
        }
        
        timerTir = cadenceDeTir;
    }

    void Update()
    {
        if (joueur == null) return; 

        // On calcule la distance exacte entre la tourelle et le joueur
        float distanceJoueur = Vector2.Distance(transform.position, joueur.position);

        // Si le joueur est assez proche, la tourelle s'active
        if (distanceJoueur <= portee)
        {
            timerTir -= Time.deltaTime;
            
            if (timerTir <= 0f)
            {
                Tirer();
                timerTir = cadenceDeTir; 
            }
        }
        else
        {
            // Optionnel : si le joueur sort de la zone, on réinitialise le chrono 
            // pour éviter que la tourelle ne tire à la microseconde où il remet un pied dedans.
            timerTir = cadenceDeTir; 
        }
    }

    void Tirer()
    {
        Vector2 directionVersJoueur = (joueur.position - pointDeTir.position).normalized;
        GameObject nouveauProjectile = Instantiate(prefabProjectile, pointDeTir.position, Quaternion.identity);

        MouvementProjectile scriptProjectile = nouveauProjectile.GetComponent<MouvementProjectile>();
        if (scriptProjectile != null)
        {
            scriptProjectile.DefinirDirection(directionVersJoueur);
        }
    }

    // --- LE BONUS LEVEL DESIGN ---
    // Cette fonction magique dessine la portée de la tourelle dans l'éditeur (fenêtre Scene).
    // Elle ne sera visible que par toi pendant la création, pas par le joueur pendant le jeu !
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, portee);
    }
}