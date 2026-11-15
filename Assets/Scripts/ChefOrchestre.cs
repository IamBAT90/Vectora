using System.Collections.Generic;
using UnityEngine;

// Cette ligne est cruciale : elle permet à Unity d'afficher notre classe personnalisée dans l'Inspector !
[System.Serializable]
public class EvenementNiveau
{
    public float tempsApparition; // À quelle seconde l'ennemi doit apparaître
    public GameObject prefabAInvoquer; // L'ennemi à faire apparaître (Laser, Tourelle, Zone...)
    public Vector2 positionApparition; // Ses coordonnées d'apparition
}

public class ChefOrchestre : MonoBehaviour
{
    [Header("Timeline du Niveau")]
    // La liste de tous les ennemis qui vont apparaître, modifiable dans l'Inspector
    public List<EvenementNiveau> evenements = new List<EvenementNiveau>();

    private float chronoNiveau = 0f;
    private int indexEvenement = 0;

    void Start()
    {
        // Sécurité : On trie automatiquement la liste par ordre de temps.
        // Comme ça, si tu ajoutes un événement à 2s après en avoir créé un à 10s dans l'Inspector, le code ne se perdra pas !
        evenements.Sort((a, b) => a.tempsApparition.CompareTo(b.tempsApparition));
    }

    void Update()
    {
        // On avance le chrono du niveau. 
        // En utilisant Time.deltaTime, le chrono ralentira aussi si le joueur utilise son pouvoir de Ralenti !
        chronoNiveau += Time.deltaTime;

        // On vérifie si on a encore des événements à jouer dans notre liste
        if (indexEvenement < evenements.Count)
        {
            // Si le chrono du jeu dépasse le temps de l'événement prévu
            if (chronoNiveau >= evenements[indexEvenement].tempsApparition)
            {
                InvoquerEvenement(evenements[indexEvenement]);
                indexEvenement++; // On passe à l'événement suivant
            }
        }
    }

    void InvoquerEvenement(EvenementNiveau evenement)
    {
        if (evenement.prefabAInvoquer != null)
        {
            // On crée (Instancie) le prefab à la position demandée
            Instantiate(evenement.prefabAInvoquer, evenement.positionApparition, Quaternion.identity);
        }
    }

    // --- LE BONUS LEVEL DESIGN ---
    // Dessine des petits carrés cyan dans la scène pour voir où les ennemis vont spawner
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (var evt in evenements)
        {
            if (evt.prefabAInvoquer != null)
            {
                Gizmos.DrawWireCube(evt.positionApparition, new Vector3(0.5f, 0.5f, 0));
            }
        }
    }
}