using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Indispensable pour l'interface

public class EntreeNiveau : MonoBehaviour
{
    [Header("Paramètres")]
    // L'index du niveau dans le Build Settings (Ex: 2 pour le Tuto, 3 pour le Niveau 1)
    public int indexNiveauACharger; 

    [Header("Interface UI")]
    // Le texte qu'on a créé sur le Canvas
    public TextMeshProUGUI texteIndicationUI; 

    private bool joueurEstSurLaCase = false;
    private bool niveauEstDebloque = false;

    void Start()
    {
        // On récupère la sauvegarde. Par défaut (pour un nouveau joueur), c'est 2 (le Tuto).
        int niveauMaxDebloque = PlayerPrefs.GetInt("NiveauDebloque", 2);

        // Si la sauvegarde est supérieure ou égale à l'index de CE niveau, alors on y a accès
        if (niveauMaxDebloque >= indexNiveauACharger)
        {
            niveauEstDebloque = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            joueurEstSurLaCase = true;

            // On change le message selon l'état du niveau
            if (niveauEstDebloque)
            {
                texteIndicationUI.text = "Appuyez sur ESPACE pour entrer";
                texteIndicationUI.color = Color.white; // Texte en blanc
            }
            else
            {
                texteIndicationUI.text = "Niveau pas encore débloqué";
                texteIndicationUI.color = Color.red; // Texte en rouge pour montrer que c'est bloqué
            }

            // On affiche le texte à l'écran
            texteIndicationUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            joueurEstSurLaCase = false;
            
            // On vérifie que le texte existe toujours avant d'essayer de le cacher
            if (texteIndicationUI != null) 
            {
                texteIndicationUI.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Le joueur doit être sur la case, avoir le niveau débloqué ET appuyer sur espace
        if (joueurEstSurLaCase && niveauEstDebloque && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(indexNiveauACharger);
        }
    }
}