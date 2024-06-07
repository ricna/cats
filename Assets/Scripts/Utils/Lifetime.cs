using System.Collections;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField]
    private float _lifetime = 5;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_lifetime);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
