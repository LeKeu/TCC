using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaSpawner : MonoBehaviour
{
    enum SpawnerTipo { Leque, Espiral, Circulo, Onda }
    List<SpawnerTipo> listaTipos = new List<SpawnerTipo>() 
    { SpawnerTipo.Leque, SpawnerTipo.Espiral, SpawnerTipo.Circulo, SpawnerTipo.Onda };

    [Header("Bala Attributos")]
    public GameObject bala;
    public float balaVida = 1f;
    public float vel = 1f;

    [Header("Spawner Atributos")]
    [SerializeField] private SpawnerTipo spawnerTipo;
    [SerializeField] private float atirandoRate = 1f;
    [SerializeField] private int numBalas = 1;

    [Header("Espiral")]
    [SerializeField] float espiralAngulo = 0f;
    [SerializeField] float espiralRotVel = 5f;

    [Header("Onda")]
    public int numWaveBullets = 10;    // N�mero de balas na onda
    public float waveAmplitude = 1.0f; // Amplitude da onda
    public float waveFrequency = 2.0f; // Frequ�ncia da onda
    public float waveSpeed = 2.0f;     // Velocidade da onda
    private float startTime;

    private GameObject spawnedBala;
    private float timer = 0f;

    bool iaraEstaAtirando;

    void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (iaraEstaAtirando)
        {
            timer += Time.deltaTime;
            if (timer >= atirandoRate)
            {
                Atirar();
                timer = 0f;
            }
        }
    }

    public void IniciarTiros() => iaraEstaAtirando = true;
    public void PararTiros() { iaraEstaAtirando = false; DestruirBalas(); MudarTipoBala(); }
    void MudarTipoBala() => spawnerTipo = listaTipos[Random.Range(0, listaTipos.Count)];
    public void DestruirBalas()
    {
        GameObject[] balas = GameObject.FindGameObjectsWithTag("AtqBalaIaraDist");
        if(balas.Length != 0)
        {
            foreach (GameObject bala in balas)
                Destroy(bala);
        }
    }

    private void Atirar()
    {
        if (bala)
        {
            if (spawnerTipo == SpawnerTipo.Leque)
            {
                float angleStep = 360f / numBalas;
                for (int i = 0; i < numBalas; i++)
                {
                    float angle = i * angleStep + espiralAngulo;
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    spawnedBala = Instantiate(bala, transform.position, rotation);
                    spawnedBala.GetComponent<BalaIara>().vel = vel;
                    spawnedBala.GetComponent<BalaIara>().vidaBala = balaVida;
                }
                espiralAngulo += espiralRotVel;
            }

            if (spawnerTipo == SpawnerTipo.Espiral)
            {

                float angleStep = 360f / numBalas;
                for (int i = 0; i < numBalas; i++)
                {
                    float angle = i * angleStep + espiralAngulo; // isso faz girar
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    spawnedBala = Instantiate(bala, transform.position, rotation);
                    spawnedBala.GetComponent<BalaIara>().vel = vel;
                    spawnedBala.GetComponent<BalaIara>().vidaBala = balaVida;
                }
                espiralAngulo += espiralRotVel;
            }

            if (spawnerTipo == SpawnerTipo.Circulo)
            {
                for (int i = 0; i < numBalas; i++)
                {
                    float angle = (i / (float)numBalas) * 360f + espiralAngulo;
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    spawnedBala = Instantiate(bala, transform.position, rotation);
                    spawnedBala.GetComponent<BalaIara>().vel = vel;
                    spawnedBala.GetComponent<BalaIara>().vidaBala = balaVida;
                }
                espiralAngulo += espiralRotVel;
            }

            if (spawnerTipo == SpawnerTipo.Onda)
            {
                float bulletSpacing = 360f / numWaveBullets;
                float currentTime = Time.time - startTime;

                for (int i = 0; i < numWaveBullets; i++)
                {
                    float angle = i * bulletSpacing + espiralAngulo;
                    float yOffset = waveAmplitude * Mathf.Sin(waveFrequency * currentTime + (i * bulletSpacing));
                    Vector3 spawnPosition = transform.position + new Vector3(0, yOffset, 0);
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    spawnedBala = Instantiate(bala, spawnPosition, rotation);
                    spawnedBala.GetComponent<BalaIara>().vel = vel;
                    spawnedBala.GetComponent<BalaIara>().vidaBala = balaVida;
                }
                espiralAngulo += espiralRotVel;
            }
        }

    }
}
