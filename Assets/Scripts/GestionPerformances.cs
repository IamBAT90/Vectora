using UnityEngine;
using TMPro;

public class GestionPerformances : MonoBehaviour
{
    [Header("Affichage")]
    public TextMeshProUGUI texteFPS;
    
    [Header("Paramètres")]
    public bool afficherFPS = true;
    public float frequenceMiseAJour = 0.5f; // Met à jour le texte toutes les 0.5s pour rester lisible

    private float chronometre;

    void Start()
    {
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = -1; 
        
        // On charge la sauvegarde (1 = affiché, 0 = caché). Par défaut c'est 1.
        afficherFPS = PlayerPrefs.GetInt("AfficherFPS", 1) == 1;

        if (texteFPS != null)
        {
            texteFPS.gameObject.SetActive(afficherFPS);
        }
    }

    void Update()
    {
        if (!afficherFPS || texteFPS == null) return;

        // On utilise unscaledTime pour que le compteur ne s'effondre pas quand le joueur utilise son pouvoir de Ralenti !
        if (Time.unscaledTime > chronometre)
        {
            // Calcul mathématique des FPS
            float fps = 1f / Time.unscaledDeltaTime;
            
            texteFPS.text = Mathf.Round(fps) + " FPS";
            chronometre = Time.unscaledTime + frequenceMiseAJour;
        }
    }

    // Fonction à relier à une case à cocher (Toggle) dans ton menu Paramètres
    public void BasculerAffichageFPS(bool estCoche)
    {
        afficherFPS = estCoche;
        if (texteFPS != null)
        {
            texteFPS.gameObject.SetActive(afficherFPS);
        }
        
        // On sauvegarde le choix pour la prochaine fois
        PlayerPrefs.SetInt("AfficherFPS", estCoche ? 1 : 0);
        PlayerPrefs.Save();
    }
}