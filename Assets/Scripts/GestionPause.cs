using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionPause : MonoBehaviour
{
    public GameObject menuPauseUI;
    public GameObject panneauParametres; // Si tu as copié ton panneau de paramètres ici
    private bool jeuEnPause = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jeuEnPause) Reprendre();
            else MettreEnPause();
        }
    }

    public void MettreEnPause()
    {
        menuPauseUI.SetActive(true);
        Time.timeScale = 0f; // Fige le temps du jeu
        jeuEnPause = true;
    }

    public void Reprendre()
    {
        menuPauseUI.SetActive(false);
        if (panneauParametres != null) panneauParametres.SetActive(false);
        Time.timeScale = 1f; // Relance le temps du jeu
        jeuEnPause = false;
    }

    public void RetourMenuPrincipal()
    {
        Time.timeScale = 1f; // IMPORTANT : Remettre le temps à la normale avant de changer de scène
        SceneManager.LoadScene(0); // 0 = Menu Principal
    }
}