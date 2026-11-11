using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Requis pour les éléments d'interface TMP

public class MenuParametres : MonoBehaviour
{
    [Header("Menu principal")]
    public GameObject menuAccueil; 
    
    [Header("Éléments de l'interface")]
    public TMP_Dropdown dropdownResolution;
    public Toggle togglePleinEcran;
    
    // Le panneau lui-même pour pouvoir l'afficher/le cacher
    public GameObject panneauParametres; 

    private Resolution[] resolutions;

    void Start()
    {
        // On récupère toutes les résolutions compatibles avec l'écran du joueur
        resolutions = Screen.resolutions;
        dropdownResolution.ClearOptions();

        List<string> options = new List<string>();
        int indexResolutionActuelle = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            // On formate le texte (Ex: 1920 x 1080 @ 60Hz)
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + Mathf.RoundToInt((float)resolutions[i].refreshRateRatio.value) + "Hz";
            options.Add(option);

            // On cherche quelle est la résolution actuelle pour la sélectionner par défaut
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                indexResolutionActuelle = i;
            }
        }

        dropdownResolution.AddOptions(options);
        dropdownResolution.value = indexResolutionActuelle;
        dropdownResolution.RefreshShownValue();

        // On coche ou décoche la case selon si on est déjà en plein écran
        togglePleinEcran.isOn = Screen.fullScreen;
        
        // On cache le menu au démarrage
        panneauParametres.SetActive(false);
    }

    // --- FONCTIONS À RELIER AUX BOUTONS DE L'INTERFACE ---

    public void AppliquerResolution(int indexResolution)
    {
        Resolution res = resolutions[indexResolution];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void AppliquerPleinEcran(bool estPleinEcran)
    {
        Screen.fullScreen = estPleinEcran;
    }
    public void OuvrirParametres()
    {
        panneauParametres.SetActive(true);
        menuAccueil.SetActive(false); // On cache le menu principal
    }

    public void FermerParametres()
    {
        panneauParametres.SetActive(false);
        menuAccueil.SetActive(true); // On réaffiche le menu principal
    }
}