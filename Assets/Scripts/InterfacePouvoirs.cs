using UnityEngine;
// 1. On remplace l'ancien 'using UnityEngine.UI' par celui de TextMeshPro :
using TMPro; 

public class InterfacePouvoirs : MonoBehaviour
{
    [Header("Textes de l'interface")]
    // 2. On change 'Text' en 'TextMeshProUGUI'
    public TextMeshProUGUI textePrecedent; 
    public TextMeshProUGUI texteActuel;    
    public TextMeshProUGUI texteSuivant;   

    public void MettreAJourInterface(string[] inventaire, int indexActuel)
    {
        if (inventaire.Length == 0) return;

        int indexPrecedent = (indexActuel - 1 + inventaire.Length) % inventaire.Length;
        int indexSuivant = (indexActuel + 1) % inventaire.Length;

        textePrecedent.text = "A : " + inventaire[indexPrecedent];
        texteActuel.text = "> " + inventaire[indexActuel] + " <";
        texteSuivant.text = "E : " + inventaire[indexSuivant];
    }
}