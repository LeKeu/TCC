using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawnar : MonoBehaviour
{
    public void RespawnarJogador()
    {
        JogadorController.Instance.transform.position = gameObject.transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EsconderUI()
    {
        GameObject.Find("Canvas").SetActive(false);
    }
}
