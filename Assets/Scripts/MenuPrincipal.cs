using UnityEngine;
using UnityEngine.SceneManagement; // Requis pour charger les niveaux

public class MenuPrincipal : MonoBehaviour
{
    // Cette fonction doit être "public" pour que le bouton puisse l'utiliser
    public void LancerJeu()
    {
        // On charge la scène numéro 1 (qui sera ton niveau 1)
        SceneManager.LoadScene(1);
    }

    public void QuitterJeu()
    {
        // Ferme le jeu (ne fonctionne que lorsque le jeu est exporté, pas dans l'éditeur)
        Debug.Log("Le joueur a quitté le jeu");
        Application.Quit();
    }
}