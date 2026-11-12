using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouvementJoueur : MonoBehaviour
{
    public enum TypePouvoir { Aucun, Dash, Teleportation, Ralenti, Miniature }

    [Header("Déplacement")]
    public float vitesse = 5f;

    [Header("Touches de Déplacement")]
    public KeyCode toucheHaut = KeyCode.W;
    public KeyCode toucheBas = KeyCode.S;
    public KeyCode toucheGauche = KeyCode.A;
    public KeyCode toucheDroite = KeyCode.D;

    [Header("Touches d'Action")]
    public KeyCode toucheAction = KeyCode.Space;
    public KeyCode toucheInventaireGauche = KeyCode.Q;
    public KeyCode toucheInventaireDroite = KeyCode.E;

    [Header("Inventaire des Pouvoirs")]
    public List<TypePouvoir> pouvoirsDebloques = new List<TypePouvoir>();
    private int indexPouvoirActuel = 0; 

    [Header("Interface")]
    public InterfacePouvoirs interfacePouvoirs;

    [Header("Paramètres Dash")]
    public float multiplicateurDash = 3f;
    public float dureeDash = 0.2f;

    [Header("Paramètres Téléportation")]
    public float distanceTeleportation = 4f;
    public float tempsRechargeTeleportation = 1f;

    [Header("Paramètres Ralenti")]
    public float facteurRalenti = 0.5f; 
    public float dureeRalenti = 3f;

    [Header("Paramètres Miniature")]
    public float facteurTaille = 0.5f; 
    public float dureeMiniature = 3f;

    private Rigidbody2D rb;
    private Vector2 directionMouvement;
    
    private bool enTrainDeDash = false;
    private bool teleportationPrete = true;
    private bool enTrainDeRalentir = false;
    private bool enTrainDeRetrecir = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        toucheHaut = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheHaut", "W"));
        toucheBas = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheBas", "S"));
        toucheGauche = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheGauche", "A"));
        toucheDroite = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheDroite", "D"));

        toucheAction = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheAction", "Space"));
        toucheInventaireGauche = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheInvGauche", "Q"));
        toucheInventaireDroite = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ToucheInvDroite", "E"));

        MettreAJourAffichage();
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

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
        // NOUVEAUTÉ : On utilise "fixedUnscaledDeltaTime". 
        // Ainsi, le joueur se déplace en fonction du temps réel de la vraie vie, et ignore le ralenti du jeu !
        rb.MovePosition(rb.position + directionMouvement.normalized * vitesse * Time.fixedUnscaledDeltaTime);
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
        if (indexPouvoirActuel >= pouvoirsDebloques.Count) indexPouvoirActuel = 0;
        else if (indexPouvoirActuel < 0) indexPouvoirActuel = pouvoirsDebloques.Count - 1;

        MettreAJourAffichage();
    }

    void MettreAJourAffichage()
    {
        if (interfacePouvoirs != null && pouvoirsDebloques.Count > 0)
        {
            string[] nomsPouvoirs = new string[pouvoirsDebloques.Count];
            for (int i = 0; i < pouvoirsDebloques.Count; i++)
            {
                nomsPouvoirs[i] = pouvoirsDebloques[i].ToString();
            }
            interfacePouvoirs.MettreAJourInterface(nomsPouvoirs, indexPouvoirActuel);
        }
    }

    void UtiliserPouvoirActuel()
    {
        TypePouvoir pouvoirActif = pouvoirsDebloques[indexPouvoirActuel];

        if (pouvoirActif == TypePouvoir.Dash && !enTrainDeDash) StartCoroutine(FaireUnDash());
        else if (pouvoirActif == TypePouvoir.Teleportation && teleportationPrete) StartCoroutine(FaireUneTeleportation());
        else if (pouvoirActif == TypePouvoir.Ralenti && !enTrainDeRalentir) StartCoroutine(FaireUnRalenti());
        else if (pouvoirActif == TypePouvoir.Miniature && !enTrainDeRetrecir) StartCoroutine(FaireUneMiniature());
    }

    IEnumerator FaireUnDash()
    {
        if (CameraShake.Instance != null) CameraShake.Instance.Secouer(0.1f, 0.2f);
        enTrainDeDash = true;
        float vitesseNormale = vitesse;
        vitesse = vitesse * multiplicateurDash; 
        
        // NOUVEAUTÉ : WaitForSecondsRealtime garantit que le Dash dure bien 0.2s en vrai, 
        // même si le reste du jeu tourne au ralenti.
        yield return new WaitForSecondsRealtime(dureeDash); 
        
        vitesse = vitesseNormale; 
        enTrainDeDash = false;
    }

    IEnumerator FaireUneTeleportation()
    {
        teleportationPrete = false;
        Vector2 directionSaut = directionMouvement.normalized;
        if (directionSaut == Vector2.zero) directionSaut = Vector2.up; 
        
        RaycastHit2D hit = Physics2D.Raycast(rb.position, directionSaut, distanceTeleportation, LayerMask.GetMask("wall"));

        if (hit.collider != null) rb.position = hit.point - (directionSaut * 0.1f);
        else rb.position = rb.position + (directionSaut * distanceTeleportation);
        
        // NOUVEAUTÉ : Temps de recharge basé sur le temps réel
        yield return new WaitForSecondsRealtime(tempsRechargeTeleportation);
        teleportationPrete = true;
    }

    IEnumerator FaireUnRalenti()
    {
        enTrainDeRalentir = true;
        
        // 1. On ralentit le jeu
        Time.timeScale = facteurRalenti; 
        
        // 2. CORRECTION : On accélère l'horloge de la physique pour compenser le ralenti global
        // 0.02f est la valeur par défaut de base dans Unity
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        yield return new WaitForSecondsRealtime(dureeRalenti); 

        // 3. On remet tout à la normale à la fin du pouvoir
        Time.timeScale = 1f; 
        Time.fixedDeltaTime = 0.02f; 
        
        enTrainDeRalentir = false;
    }

    IEnumerator FaireUneMiniature()
    {
        enTrainDeRetrecir = true;
        Vector3 tailleNormale = transform.localScale;
        transform.localScale = tailleNormale * facteurTaille;
        
        // NOUVEAUTÉ : Durée de la miniature basée sur le temps réel
        yield return new WaitForSecondsRealtime(dureeMiniature); 
        
        transform.localScale = tailleNormale;
        enTrainDeRetrecir = false;
    }
}