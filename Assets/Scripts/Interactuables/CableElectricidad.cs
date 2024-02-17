using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableElectricidad : InteractuableObject
{
    [SerializeField] private GameObject cableDesconectadoGO;
    [SerializeField] private GameObject cableConectadoGO;

    private bool interactuando;

    protected override void Intaractuar()
    {
        if (!interactuando)
        {
            return;
        }
        //Notificar del evento de progreso
        LevelManager.EventoProgreso(TipoObjetivo.DesenchufarElectricidad);

        //Cambiar los sprites
        cableConectadoGO.SetActive(false);
        cableDesconectadoGO.SetActive(true);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(this.tag == "CableElectricidad")
        {
            base.OnTriggerEnter(other);
            interactuando = true;
            return;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        interactuando = false;
        return;
    }

}
