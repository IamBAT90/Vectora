using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MouvementJoueur : MonoBehaviour
{
    // 1. On a ajouté "Ralenti" et "Miniature" à la liste
    public enum TypePouvoir { Aucun, Dash, Teleportation, Ralenti, Miniature }

    [Header("Déplacement")]
    public float vitesse = 5f;

    [Header("Touches de Déplacement")]
    public KeyCode toucheHaut = KeyCode.W; // Z
    public KeyCode toucheBas = KeyCode.S;
    public KeyCode toucheGauche = KeyCode.A; // Q
    public KeyCode toucheDroite = KeyCode.D;

    [Header("Touches d'Action")]
    public KeyCode toucheAction = KeyCode.Space;
    public KeyCode toucheInventaireGauche = KeyCode.Q; // A
    public KeyCode toucheInventaireDroite = KeyCode.E;

    [Header("Inventaire des Pouvoirs")]
    public List<TypePouvoir> pouvoirsDebloques = new List<TypePouvoir>();
    private int indexPouvoirActuel = 0; 

    [Header("Interface")]
    public TextMeshProUGUI affichagePouvoirActuel; 

    [Header("Paramètres Dash")]
    public float multiplicateurDash = 3f;
    public float dureeDash = 0.2f;

    [Header("Paramètres Téléportation")]
    public float distanceTeleportation = 4f;
    public float tempsRechargeTeleportation = 1f;

    [Header("Paramètres Ralenti")]
    public float facteurRalenti = 0.5f; // Le jeu tournera à 50% de sa vitesse
    public float dureeRalenti = 3f;

    [Header("Paramètres Miniature")]
    public float facteurTaille = 0.5f; // Le joueur sera 2 fois plus petit
    public float dureeMiniature = 3f;

    private Rigidbody2D rb;
    private Vector2 directionMouvement;
    
    // Sécurités pour ne pas lancer le pouvoir 10 fois en même temps
    private bool enTrainDeDash = false;
    private bool teleportationPrete = true;
    private bool enTrainDeRalentir = false;
    private bool enTrainDeRetrecir = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (affichagePouvoirActuel != null) affichagePouvoirActuel.text = "Pouvoir : Aucun";

        // Chargement de toutes les touches
        toucheHaut = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheHaut", "W"));
        toucheBas = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheBas", "S"));
        toucheGauche = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheGauche", "A"));
        toucheDroite = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheDroite", "D"));

        toucheAction = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheAction", "Space"));
        toucheInventaireGauche = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheInvGauche", "Q"));
        toucheInventaireDroite = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheInvDroite", "E"));
    }

    void Update()
    {
        // Nouveau système de déplacement manuel
        float moveX = 0f;
        float moveY = 0f;

        // Input.GetKey vérifie si la touche est maintenue enfoncée (contrairement à GetKeyDown)
        if (Input.GetKey(toucheGauche)) moveX -= 1f;
        if (Input.GetKey(toucheDroite)) moveX += 1f;
        if (Input.GetKey(toucheHaut)) moveY += 1f;
        if (Input.GetKey(toucheBas)) moveY -= 1f;

        directionMouvement = new Vector2(moveX, moveY).normalized;

        if (pouvoirsDebloques.Count > 0)
        {
            if (Input.GetKeyDown(toucheInventaireGauche)) ChangerPouvoir(-1);
            else if (Input.GetKeyDown(toucheInventaireDroite)) ChangerPouvoir(1);

            if (Input.GetKeyDown(toucheAction)) UtiliserPouvoirActuel();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + directionMouvement.normalized * vitesse * Time.fixedDeltaTime);
    }

    public void DebloquerPouvoir(TypePouvoir nouveauPouvoir)
    {
        if (!pouvoirsDebloques.Contains(nouveauPouvoir))
        {
            pouvoirsDebloques.Add(nouveauPouvoir);
            
            if (pouvoirsDebloques.Count == 1)
            {
                indexPouvoirActuel = 0;
            }
            
            MettreAJourAffichage(); 
        }
    }

    void ChangerPouvoir(int direction)
    {
        indexPouvoirActuel += direction;

        if (indexPouvoirActuel >= pouvoirsDebloques.Count)
        {
            indexPouvoirActuel = 0;
        }
        else if (indexPouvoirActuel < 0)
        {
            indexPouvoirActuel = pouvoirsDebloques.Count - 1;
        }

        MettreAJourAffichage();
    }

    void MettreAJourAffichage()
    {
        if (affichagePouvoirActuel != null && pouvoirsDebloques.Count > 0)
        {
            affichagePouvoirActuel.text = "Pouvoir : " + pouvoirsDebloques[indexPouvoirActuel].ToString();
        }
    }

    // 2. On a ajouté les nouvelles conditions ici
    void UtiliserPouvoirActuel()
    {
        TypePouvoir pouvoirActif = pouvoirsDebloques[indexPouvoirActuel];

        if (pouvoirActif == TypePouvoir.Dash && !enTrainDeDash)
        {
            StartCoroutine(FaireUnDash());
        }
        else if (pouvoirActif == TypePouvoir.Teleportation && teleportationPrete)
        {
            StartCoroutine(FaireUneTeleportation());
        }
        else if (pouvoirActif == TypePouvoir.Ralenti && !enTrainDeRalentir)
        {
            StartCoroutine(FaireUnRalenti());
        }
        else if (pouvoirActif == TypePouvoir.Miniature && !enTrainDeRetrecir)
        {
            StartCoroutine(FaireUneMiniature());
        }
    }

    IEnumerator FaireUnDash()
    {
        enTrainDeDash = true;
        float vitesseNormale = vitesse;
        vitesse = vitesse * multiplicateurDash; 
        yield return new WaitForSeconds(dureeDash); 
        vitesse = vitesseNormale; 
        enTrainDeDash = false;
    }

    IEnumerator FaireUneTeleportation()
    {
        teleportationPrete = false;
        Vector2 directionSaut = directionMouvement.normalized;
        if (directionSaut == Vector2.zero) directionSaut = Vector2.up; 
        
        // On tire un "laser" depuis le joueur, dans la direction du saut, sur la distance du saut.
        // Le LayerMask.GetMask("wall") permet au laser de traverser tout le reste et de ne détecter que les murs.
        RaycastHit2D hit = Physics2D.Raycast(rb.position, directionSaut, distanceTeleportation, LayerMask.GetMask("wall"));

        // Si le laser a percuté un mur
        if (hit.collider != null)
        {
            // On téléporte le joueur au point d'impact du mur. 
            // On recule d'une toute petite marge (0.1f) pour éviter qu'il ne se coince dans le mur.
            rb.position = hit.point - (directionSaut * 0.1f);
        }
        else
        {
            // Si le laser n'a rien touché, la voie est libre, on fait un saut complet
            rb.position = rb.position + (directionSaut * distanceTeleportation);
        }
        
        yield return new WaitForSeconds(tempsRechargeTeleportation);
        teleportationPrete = true;
    }

    // --- NOUVEAUX POUVOIRS ---

    IEnumerator FaireUnRalenti()
    {
        enTrainDeRalentir = true;
        
        // Time.timeScale gère la vitesse globale du jeu (1 = normal, 0.5 = 2x plus lent)
        Time.timeScale = facteurRalenti; 

        // IMPORTANT: Comme le temps est ralenti, on utilise WaitForSecondsRealtime
        // Sinon, le pouvoir durerait 2x plus longtemps dans la vraie vie !
        yield return new WaitForSecondsRealtime(dureeRalenti); 

        // On remet la vitesse normale
        Time.timeScale = 1f; 
        enTrainDeRalentir = false;
    }

    IEnumerator FaireUneMiniature()
    {
        enTrainDeRetrecir = true;
        
        // On sauvegarde la taille d'origine du joueur
        Vector3 tailleNormale = transform.localScale;
        
        // On le rend plus petit
        transform.localScale = tailleNormale * facteurTaille;

        yield return new WaitForSeconds(dureeMiniature); 

        // On lui redonne sa taille normale
        transform.localScale = tailleNormale;
        enTrainDeRetrecir = false;
    }
}