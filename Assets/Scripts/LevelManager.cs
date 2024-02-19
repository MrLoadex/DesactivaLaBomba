using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TextMeshProUGUI tareasCompleatadasTMP;

    [Header("Timer en Segundos")]
    private int maxTime;
    private int actualTime;

    [Header("Sonidos")]
    public AudioClip clipVictoria;
    public AudioClip clipDerrota;
    public AudioClip clipLogro;
    public AudioClip clipAllLogro;

    private AudioSource audioSource;

    //Eventos
    public static Action< Cable> EventoCortarCable;
    public static Action<TipoObjetivo> EventoProgreso;
    public static Action EventoPerder;

    //Tareas
    private int tareasTotales = 1;
    private int tareasCmpletadas = 0;

    //Estado de nivel
    private bool cableCortado;
    private bool electricidadCortada;
    private bool codigoDesifrado;
    private bool listoParaGanar;

    private void Start() 
    {
        //Obtener el audio Sourse
        audioSource = gameObject.AddComponent<AudioSource>();
        
        //Dar por completados los logros que no estan habilitados en ese nivel
        InicializarTareasNecesarias();

        //Inicializar tareas y mostrarlas wen el GUI
        InicializarTareasTotales();
        ActualizarTareasCompletadasGUI();

        //Actualizar el tiempo maximo e inicializar el contador
        maxTime = levelSettings.tiempoMaximo;
        actualTime = maxTime;
        StartCoroutine(DescontarTiempo());
    }

    private void InicializarTareasNecesarias()
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

        //Reproducir sonido
        audioSource.clip = clipAllLogro;
        audioSource.Play();
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

        tareasCmpletadas = tareasTotales;
        ActualizarTareasCompletadasGUI();
        
        levelSettings.nextLvlSettings.tiempoMaximo = actualTime;

        StartCoroutine(ReproducirVictoria());
    }

    private IEnumerator ReproducirVictoria()
    {
        //Reproducir sonido
        audioSource.clip = clipVictoria;
        audioSource.Play();

        yield return new WaitForSeconds(clipVictoria.length);

        //Cambiar de escena
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

        //Reproducir sonido
        audioSource.clip = clipDerrota;
        audioSource.Play();     
    }

    public void ResetearJuego()
    {


        panelDerrota.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        return;
    }

    private void CortarCableCorrecto()
    {
        cableCortado = true;
        ActualizarProgreso();

        //Reproducir sonido
        audioSource.clip = clipLogro;
        audioSource.Play();

        //Tareas
        tareasCmpletadas ++;
        ActualizarTareasCompletadasGUI();
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
                if (electricidadCortada)
                {
                    return;
                }
                electricidadCortada = true;
                ActualizarProgreso();

                //tareas
                tareasCmpletadas ++;

                //Sonido
                audioSource.clip = clipLogro;
                audioSource.Play();
                break;
                
            case TipoObjetivo.DesifrarCodigo:
                if (codigoDesifrado)
                {
                    return;
                }

                codigoDesifrado = true;
                ActualizarProgreso();

                //tareas
                tareasCmpletadas ++;

                //Sonido
                audioSource.clip = clipLogro;
                audioSource.Play();
                break;

            default:
                break;
        }
        ActualizarTareasCompletadasGUI();
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

    #region Tareas

    private void InicializarTareasTotales()
    {
        if(levelSettings.CortarCable)
        {
            tareasTotales ++;
        }
        if(levelSettings.DesifrarCodigo)
        {
            tareasTotales ++;
        }
        if(levelSettings.CortarElectricidad)
        {
            tareasTotales ++;
        }
    }

    private void ActualizarTareasCompletadasGUI()
    {
        tareasCompleatadasTMP.text = $"{tareasCmpletadas}/{tareasTotales}";
    }

    #endregion
}
