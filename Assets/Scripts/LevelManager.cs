using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelManager : MonoBehaviour
{
    [Header("Configuracion de Nivel")]
    [SerializeField] private LevelSettings levelSettings;
    public LevelSettings LevelSettings  => levelSettings;
    
    [Header("GUI")] 
    [SerializeField] private GameObject panelDerrota;
    [SerializeField] private TextMeshProUGUI timerTMP;

    [Header("Timer en Segundos")]
    [SerializeField] private int maxTime;
    private int actualTime;

    public static Action< Cable> EventoCortarCable;

    //Estado de nivel
    private bool cableCortado;
    private bool electricidadCortada;
    private bool codigoDesifrado;
    private bool listoParaGanar;

    private void Start() 
    {
        if (!levelSettings.DesifrarCodigo)
        {
            codigoDesifrado = true;
        }

        if (!levelSettings.CortarElectricidad)
        {
            electricidadCortada = true;
        }

        if (!levelSettings.CortarCable)
        {
            cableCortado = true;
        }
        if (cableCortado && codigoDesifrado && electricidadCortada)
        {
            listoParaGanar = true;
        }

        actualTime = maxTime;
        StartCoroutine(DescontarTiempo());
    }

    private void CortarCableCorrecto()
    {
        cableCortado = true;
        ActualizarProgreso();
    }

    private void ActualizarProgreso()
    {
        if (!codigoDesifrado)
        {
            return;
        }
        if (!electricidadCortada)
        {
            return;
        }
        if (!cableCortado)
        {
            return;
        }
        listoParaGanar = true;
    }

    public void IntentarDefusar()
    {
        if (!listoParaGanar)
        {
            PerderNivel();
            return;
        }
        GanarNivel();
    }

    private void GanarNivel()
    {
        Debug.Log("Ganaste!");
        SceneManager.LoadScene(levelSettings.nextSceneName);
    }

    private void PerderNivel()
    {
        Debug.Log("Perdiste ");
        Time.timeScale = 0f;
        panelDerrota.SetActive(true);
        
    }

    public void ResetearJuego()
    {

        panelDerrota.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    private void ResponerEventoCortarCable(Cable cableCortado)
    {
        if(cableCortado.ColorCable == LevelSettings.colorGanador && levelSettings.CortarCable)
        {
            CortarCableCorrecto();
        }
        else
        {
            PerderNivel();
        }
    }

    private void OnEnable() 
    {
        EventoCortarCable += ResponerEventoCortarCable;
    }

    private void OnDisable() 
    {
        EventoCortarCable += ResponerEventoCortarCable;
    }
    
    private IEnumerator DescontarTiempo()
    {
        if (actualTime <= 0)
        {
            
            PerderNivel();
            StopCoroutine(DescontarTiempo());
        }
        TimeSpan tiempo = TimeSpan.FromSeconds(actualTime);
        timerTMP.text = $"{tiempo.ToString(@"mm\:ss")}";
        actualTime --;
        yield return new WaitForSeconds(1);
        StartCoroutine(DescontarTiempo());

    }

}
