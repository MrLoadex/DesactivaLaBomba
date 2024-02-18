using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public enum TipoObjetivo
{
    DesenchufarElectricidad,
    DesifrarCodigo
}

public class LevelManager : MonoBehaviour
{
    [Header("Configuracion de Nivel")]
    [SerializeField] private LevelSettings levelSettings;
    public LevelSettings LevelSettings  => levelSettings;
    
    [Header("GUI")] 
    [SerializeField] private GameObject panelDerrota;
    [SerializeField] private TextMeshProUGUI timerTMP;

    [Header("Timer en Segundos")]
    private int maxTime;
    private int actualTime;

    public static Action< Cable> EventoCortarCable;
    public static Action<TipoObjetivo> EventoProgreso;
    public static Action EventoPerder;

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

        maxTime = levelSettings.tiempoMaximo;
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
        
        levelSettings.nextLvlSettings.tiempoMaximo = actualTime;

        SceneManager.LoadScene(levelSettings.nextSceneName);
    }

    private void PerderNivel()
    {
        Debug.Log("Perdiste ");
        Time.timeScale = 0f;
        if(panelDerrota == null)
        {
            return;
        }
        panelDerrota.SetActive(true);
        
    }

    public void ResetearJuego()
    {

        panelDerrota.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        return;
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

    private void ResponerEventoProgreso(TipoObjetivo tipo)
    {
        switch (tipo)
        {
            case TipoObjetivo.DesenchufarElectricidad:
                electricidadCortada = true;
                ActualizarProgreso();
                break;
                
            case TipoObjetivo.DesifrarCodigo:
                codigoDesifrado = true;
                ActualizarProgreso();
                break;

            default:
                break;
        }
    }

    private void OnEnable() 
    {
        EventoCortarCable += ResponerEventoCortarCable;
        EventoProgreso += ResponerEventoProgreso;
        EventoPerder += PerderNivel;
    }

    private void OnDisable() 
    {
        EventoCortarCable -= ResponerEventoCortarCable;
        EventoProgreso -= ResponerEventoProgreso;
        EventoPerder -= PerderNivel;
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
