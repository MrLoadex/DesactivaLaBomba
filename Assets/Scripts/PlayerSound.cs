using System.Collections;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioClip[] sonidosPasos; // Asigna los sonidos de pasos en el Inspector
    private AudioSource audioSource;

    private bool reproduciendoSonido = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Verificar si no se está reproduciendo ningún sonido y si se está moviendo
        if (!reproduciendoSonido && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            // Reproducir sonido aleatorio
            ReproducirSonidoAleatorio();
        }
    }

    void ReproducirSonidoAleatorio()
    {
        if (sonidosPasos.Length > 0)
        {
            // Seleccionar un sonido aleatorio
            AudioClip sonidoSeleccionado = sonidosPasos[Random.Range(0, sonidosPasos.Length)];

            // Asignar el sonido al AudioSource
            audioSource.clip = sonidoSeleccionado;

            //Regular Volumen
            audioSource.volume = .1f;

            // Reproducir el sonido
            audioSource.Play();

            // Establecer el indicador de reproducción a true
            reproduciendoSonido = true;

            // Iniciar una corrutina para restablecer el indicador después de la duración del sonido
            StartCoroutine(ReiniciarIndicadorSonido(sonidoSeleccionado.length));
        }
    }

    IEnumerator ReiniciarIndicadorSonido(float duracionSonido)
    {
        // Esperar la duración del sonido
        yield return new WaitForSeconds(duracionSonido);

        // Restablecer el indicador de reproducción a false
        reproduciendoSonido = false;
    }
}