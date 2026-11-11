using UnityEngine;
using TMPro;

public class PersonnalisationTouches : MonoBehaviour
{
    [Header("Boutons de Déplacement")]
    public TextMeshProUGUI texteBoutonHaut;
    public TextMeshProUGUI texteBoutonBas;
    public TextMeshProUGUI texteBoutonGauche;
    public TextMeshProUGUI texteBoutonDroite;

    [Header("Boutons d'Action")]
    public TextMeshProUGUI texteBoutonAction;
    public TextMeshProUGUI texteBoutonInvGauche;
    public TextMeshProUGUI texteBoutonInvDroite;

    private bool enAttenteDeTouche = false;
    private string actionAModifier = "";

    void Start()
    {
        texteBoutonHaut.text = "Haut : " + PlayerPrefs.GetString("ToucheHaut", "W");
        texteBoutonBas.text = "Bas : " + PlayerPrefs.GetString("ToucheBas", "S");
        texteBoutonGauche.text = "Gauche : " + PlayerPrefs.GetString("ToucheGauche", "A");
        texteBoutonDroite.text = "Droite : " + PlayerPrefs.GetString("ToucheDroite", "D");

        texteBoutonAction.text = "Action : " + PlayerPrefs.GetString("ToucheAction", "Space");
        texteBoutonInvGauche.text = "Pouvoir Préc. : " + PlayerPrefs.GetString("ToucheInvGauche", "Q");
        texteBoutonInvDroite.text = "Pouvoir Suiv. : " + PlayerPrefs.GetString("ToucheInvDroite", "E");
    }

    public void DemanderNouvelleTouche(string nomAction)
    {
        enAttenteDeTouche = true;
        actionAModifier = nomAction;
        
        if (nomAction == "ToucheHaut") texteBoutonHaut.text = "...";
        else if (nomAction == "ToucheBas") texteBoutonBas.text = "...";
        else if (nomAction == "ToucheGauche") texteBoutonGauche.text = "...";
        else if (nomAction == "ToucheDroite") texteBoutonDroite.text = "...";
        else if (nomAction == "ToucheAction") texteBoutonAction.text = "...";
        else if (nomAction == "ToucheInvGauche") texteBoutonInvGauche.text = "...";
        else if (nomAction == "ToucheInvDroite") texteBoutonInvDroite.text = "...";
    }

    void OnGUI()
    {
        if (enAttenteDeTouche)
        {
            Event e = Event.current;
            if (e.isKey && e.keyCode != KeyCode.None)
            {
                PlayerPrefs.SetString(actionAModifier, e.keyCode.ToString());
                PlayerPrefs.Save();

                if (actionAModifier == "ToucheHaut") texteBoutonHaut.text = "Haut : " + e.keyCode.ToString();
                else if (actionAModifier == "ToucheBas") texteBoutonBas.text = "Bas : " + e.keyCode.ToString();
                else if (actionAModifier == "ToucheGauche") texteBoutonGauche.text = "Gauche : " + e.keyCode.ToString();
                else if (actionAModifier == "ToucheDroite") texteBoutonDroite.text = "Droite : " + e.keyCode.ToString();
                else if (actionAModifier == "ToucheAction") texteBoutonAction.text = "Action : " + e.keyCode.ToString();
                else if (actionAModifier == "ToucheInvGauche") texteBoutonInvGauche.text = "Pouvoir Préc. : " + e.keyCode.ToString();
                else if (actionAModifier == "ToucheInvDroite") texteBoutonInvDroite.text = "Pouvoir Suiv. : " + e.keyCode.ToString();

                enAttenteDeTouche = false;
            }
        }
    }
}