
using TMPro;
using UnityEngine;

public class CajaFuerte : MonoBehaviour
{
    [Header("Panel:")]
    [SerializeField] private TextMeshProUGUI pantallaTMP;

    [Header("Luz Testigo")]
    [SerializeField] private Luz luzTestigo;
    
    [Header("Nivel Settings")]
    [SerializeField] private LevelSettings levelSettings;

    
    private string codigoCorrecto;
    private string codigoActual = "";

    private void Start() 
    {
        codigoCorrecto = levelSettings.codigoGanador.ToString();
        ActualizarPantalla();
    }

    public void ResponderBotonApretado(int num)
    {
        codigoActual += num;
        ActualizarPantalla();
    }

    public void Borrar()
    {
        codigoActual = "";
        ActualizarPantalla();
    }

    public void PorbarCodigo()
    {
        if (codigoActual == codigoCorrecto)
        {
            luzTestigo.Habilitar();
            LevelManager.EventoProgreso?.Invoke(TipoObjetivo.DesifrarCodigo);
        }
        else
        {
            LevelManager.EventoPerder?.Invoke();
        }
    }

    private void ActualizarPantalla()
    {
        pantallaTMP.text = codigoActual;
    }

}
