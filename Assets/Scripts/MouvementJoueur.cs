using UnityEngine;
using UnityEngine.InputSystem; // Indispensable pour le New Input System
using System.Collections;
using System.Collections.Generic;

public class MouvementJoueur : MonoBehaviour
{
    public enum TypePouvoir { Aucun, Dash, Teleportation, Ralenti, Miniature }

    [Header("Déplacement & Rotation")]
    public float vitesse = 5f;
    public float vitesseRotation = 20f;

    [Header("Inventaire des Pouvoirs")]
    public List<TypePouvoir> pouvoirsDebloques = new List<TypePouvoir>();
    private int indexPouvoirActuel = 0;

    [Header("Interface")]
    public InterfacePouvoirs interfacePouvoirs;
    public GameObject fondHUD;

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
    private float angleCible = 0f;

    private bool enTrainDeDash = false;
    private bool teleportationPrete = true;
    private bool enTrainDeRalentir = false;
    private bool enTrainDeRetrecir = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MettreAJourAffichage();
    }

    // --- NOUVELLES FONCTIONS APPELÉES PAR LE PLAYER INPUT ---

    void OnMove(InputValue valeur)
    {
        // Récupère directement le Vector2 (clavier, stick manette 360°, ou joystick mobile)
        directionMouvement = valeur.Get<Vector2>();
    }

    void OnAction()
    {
        if (pouvoirsDebloques.Count > 0)
        {
            UtiliserPouvoirActuel();
        }
    }

    void OnInventaireGauche()
    {
        if (pouvoirsDebloques.Count > 0)
        {
            ChangerPouvoir(-1);
        }
    }

    void OnInventaireDroite()
    {
        if (pouvoirsDebloques.Count > 0)
        {
            ChangerPouvoir(1);
        }
    }

    // --- PHYSIQUE ET ROTATION 360° ---

    void FixedUpdate()
    {
        // ClampMagnitude(1f) évite d'aller plus vite en diagonale au clavier tout en gardant la sensibilité du stick
        Vector2 deplacement = Vector2.ClampMagnitude(directionMouvement, 1f);
        rb.MovePosition(rb.position + deplacement * vitesse * Time.fixedUnscaledDeltaTime);

        // On met à jour l'angle si le stick/clavier dépasse une petite zone morte (0.1f)
        if (directionMouvement.sqrMagnitude > 0.01f)
        {
            angleCible = Mathf.Atan2(directionMouvement.y, directionMouvement.x) * Mathf.Rad2Deg - 90f;
        }

        float angleLisse = Mathf.LerpAngle(rb.rotation, angleCible, vitesseRotation * Time.fixedUnscaledDeltaTime);
        rb.MoveRotation(angleLisse);
    }

    // --- GESTION DE L'INVENTAIRE ---

    public void DebloquerPouvoir(TypePouvoir nouveauPouvoir)
    {
        if (!pouvoirsDebloques.Contains(nouveauPouvoir))
        {
            pouvoirsDebloques.Add(nouveauPouvoir);
            if (pouvoirsDebloques.Count == 1) indexPouvoirActuel = 0;
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
        bool aDesPouvoirs = pouvoirsDebloques.Count > 0;

        if (fondHUD != null) fondHUD.SetActive(aDesPouvoirs);

        if (interfacePouvoirs != null && aDesPouvoirs)
        {
            string[] nomsPouvoirs = new string[pouvoirsDebloques.Count];
            for (int i = 0; i < pouvoirsDebloques.Count; i++)
            {
                nomsPouvoirs[i] = pouvoirsDebloques[i].ToString();
            }
            interfacePouvoirs.MettreAJourInterface(nomsPouvoirs, indexPouvoirActuel);
        }
    }

    // --- POUVOIRS ---

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
        vitesse *= multiplicateurDash;
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
        Vector2 destination = (hit.collider != null) ? hit.point - (directionSaut * 0.1f) : rb.position + (directionSaut * distanceTeleportation);

        rb.interpolation = RigidbodyInterpolation2D.None;
        rb.position = destination;
        transform.position = destination;

        yield return new WaitForFixedUpdate();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        yield return new WaitForSecondsRealtime(tempsRechargeTeleportation);
        teleportationPrete = true;
    }

    IEnumerator FaireUnRalenti()
    {
        enTrainDeRalentir = true;
        Time.timeScale = facteurRalenti;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(dureeRalenti);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        enTrainDeRalentir = false;
    }

    IEnumerator FaireUneMiniature()
    {
        enTrainDeRetrecir = true;
        Vector3 tailleNormale = transform.localScale;
        transform.localScale = tailleNormale * facteurTaille;
        yield return new WaitForSecondsRealtime(dureeMiniature);
        transform.localScale = tailleNormale;
        enTrainDeRetrecir = false;
    }
}