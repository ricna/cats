using Unity.Netcode;
using UnityEngine;
using Unrez;

public class Barrier : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Cat cat))
        {
            if (cat.IsDashing())
            {
                Destroy(gameObject);
            }
        }
    }
}
