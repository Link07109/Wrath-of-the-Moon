using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") SceneManager.LoadScene("Room");
    }
}
