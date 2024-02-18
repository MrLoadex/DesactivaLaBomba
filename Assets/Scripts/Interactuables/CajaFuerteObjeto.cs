using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaFuerteObjeto : InteractuableObject
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject safeCamara;
    [SerializeField] private GameObject panelCajaFuerte;

    private bool interactuando;

    private bool safePanelActivo;


    protected override void Update()
    {
        if (!safePanelActivo)
        {
            base.Update();
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            DesactivarPanelSafe();
        }

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(this.tag == "Safe")
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

    protected override void Intaractuar()
    {
        if (!interactuando)
        {
            return;
        }
        player.SetActive(false);
        safeCamara.SetActive(true);
        botonInteractuar.SetActive(false);
        panelCajaFuerte.SetActive(true);

        safePanelActivo = true;
    }

    private void DesactivarPanelSafe()
    {
        player.SetActive(true);
        safeCamara.SetActive(false);
        botonInteractuar.SetActive(true);
        panelCajaFuerte.SetActive(false);

        safePanelActivo = false;
    }

    private void ResponderProgresoCajaFuerte(TipoObjetivo tipo)
    {
        if(tipo != TipoObjetivo.DesifrarCodigo)
        {
            return;
        }
        
        DesactivarPanelSafe();
    }

    private void OnEnable() 
    {
        LevelManager.EventoProgreso += ResponderProgresoCajaFuerte;
    }

    private void OnDisable() 
    {
        LevelManager.EventoProgreso -= ResponderProgresoCajaFuerte;
        
    }
}
