using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsCanvas : MonoBehaviour
{
    public void IniciarJogo() => SceneManager.LoadScene("02_comunidade"); //
    public void MenuInicial() => SceneManager.LoadScene("Inicio");
    public void TelaDemo() => SceneManager.LoadScene("FinalDemo");
    public void Template() => SceneManager.LoadScene("Template");
    public void BossRush() => SceneManager.LoadScene("BOSSRUSH");
    public void Sair() => Application.Quit();
}
