using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneArrivee : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // On récupère le numéro du niveau qu'on est en train de terminer (ex: 2 pour le Tuto)
            int indexNiveauTermine = SceneManager.GetActiveScene().buildIndex;
            
            // Le niveau qu'on veut débloquer est logiquement le suivant (ex: 3 pour le Niveau 1)
            int niveauADebloquer = indexNiveauTermine + 1;

            // On vérifie la sauvegarde. Le "2" ici signifie que le Tuto est le niveau de base garanti
            int niveauMaxDebloque = PlayerPrefs.GetInt("NiveauDebloque", 2);

            // Si on vient de finir un niveau qui débloque un nouvel accès, on sauvegarde
            if (niveauADebloquer > niveauMaxDebloque)
            {
                PlayerPrefs.SetInt("NiveauDebloque", niveauADebloquer);
                PlayerPrefs.Save();
                Debug.Log("Nouveau niveau débloqué : Index " + niveauADebloquer);
            }

            // On renvoie le joueur sur la carte de sélection (Index 1)
            SceneManager.LoadScene(1);
        }
    }
}