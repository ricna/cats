using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBootstrap : MonoBehaviour
{
    void Start()
    {
        NetworkManager networkManager = FindAnyObjectByType<NetworkManager>();
        if (networkManager == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
