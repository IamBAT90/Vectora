using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Le "Singleton" : permet d'accéder à ce script de n'importe où sans faire de liens dans l'éditeur
    public static CameraShake Instance;

    private Vector3 positionOriginale;

    void Awake()
    {
        // On s'assure qu'il n'y a qu'une seule instance de ce script
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Fonction publique que tu pourras appeler depuis n'importe quel autre script
    public void Secouer(float duree, float magnitude)
    {
        // On arrête les anciens tremblements s'il y en a en cours pour ne pas les superposer
        StopAllCoroutines();
        StartCoroutine(GenererTremblement(duree, magnitude));
    }

    IEnumerator GenererTremblement(float duree, float magnitude)
    {
        // On sauvegarde la position de base de la caméra
        positionOriginale = transform.localPosition;
        float tempsEcoule = 0f;

        while (tempsEcoule < duree)
        {
            // On génère des micro-décalages aléatoires
            float decalageX = Random.Range(-1f, 1f) * magnitude;
            float decalageY = Random.Range(-1f, 1f) * magnitude;

            // On applique le décalage
            transform.localPosition = new Vector3(
                positionOriginale.x + decalageX, 
                positionOriginale.y + decalageY, 
                positionOriginale.z
            );

            tempsEcoule += Time.deltaTime;

            // On attend la frame suivante avant de recommencer la boucle
            yield return null;
        }

        // Le temps est écoulé, on remet la caméra exactement à sa place
        transform.localPosition = positionOriginale;
    }
}