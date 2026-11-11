using UnityEngine;

public class MouvementCarte : MonoBehaviour
{
    public float vitesse = 5f;
    private Rigidbody2D rb;
    private Vector2 directionMouvement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        directionMouvement.x = Input.GetAxisRaw("Horizontal");
        directionMouvement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + directionMouvement.normalized * vitesse * Time.fixedDeltaTime);
    }
}