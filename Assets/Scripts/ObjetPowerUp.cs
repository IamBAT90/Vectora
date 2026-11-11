using UnityEngine;
using TMPro;
using System.Collections;

public class ObjetPowerUp : MonoBehaviour
{
    [Header("Quel pouvoir donne cet objet ?")]
    // On fait apparaître la liste déroulante de nos pouvoirs dans Unity !
    public MouvementJoueur.TypePouvoir pouvoirADebloquer;

    [Header("Interface")]
    [TextArea]
    public string messagePouvoir = "NOUVEAU POUVOIR RAMASSÉ !";
    public GameObject texteUI; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // On envoie le pouvoir choisi dans Unity au script du joueur
            collision.GetComponent<MouvementJoueur>().DebloquerPouvoir(pouvoirADebloquer);

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(AfficherExplication());
        }
    }

    IEnumerator AfficherExplication()
    {
        texteUI.GetComponent<TextMeshProUGUI>().text = messagePouvoir;
        texteUI.SetActive(true); 
        yield return new WaitForSeconds(3f); 
        texteUI.SetActive(false); 
        Destroy(gameObject);
    }
}