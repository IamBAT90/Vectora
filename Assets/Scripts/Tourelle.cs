using UnityEngine;

public class Tourelle : MonoBehaviour
{
    [Header("Paramètres de Tir")]
    public GameObject prefabProjectile; // Le modèle de la balle à tirer
    public Transform pointDeTir; // L'endroit d'où part le tir
    public float cadenceDeTir = 2f; // Temps en secondes entre chaque tir

    private Transform joueur;
    private float timerTir;

    void Start()
    {
        // Le script cherche automatiquement le joueur grâce à son Tag "Player"
        GameObject objetJoueur = GameObject.FindGameObjectWithTag("Player");
        if (objetJoueur != null)
        {
            joueur = objetJoueur.transform;
        }
        
        timerTir = cadenceDeTir;
    }

    void Update()
    {
        if (joueur == null) return; // Sécurité si le joueur est mort/absent

        // On diminue le chronomètre
        timerTir -= Time.deltaTime;
        
        if (timerTir <= 0f)
        {
            Tirer();
            timerTir = cadenceDeTir; // On réinitialise le chrono
        }
    }

    void Tirer()
    {
        // Calcul de la trajectoire du point de tir jusqu'au joueur
        Vector2 directionVersJoueur = (joueur.position - pointDeTir.position).normalized;

        // Création du projectile dans la scène
        GameObject nouveauProjectile = Instantiate(prefabProjectile, pointDeTir.position, Quaternion.identity);

        // On récupère le script du projectile qu'on vient de créer pour lui donner la direction
        MouvementProjectile scriptProjectile = nouveauProjectile.GetComponent<MouvementProjectile>();
        if (scriptProjectile != null)
        {
            scriptProjectile.DefinirDirection(directionVersJoueur);
        }
    }
}