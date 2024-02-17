using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBomb : MonoBehaviour
{
    [Header("Capa Cables")]
    public LayerMask capaCable;

    private bool hayCableSeleccionado;

    [Header("Cables")]
    [SerializeField] private Cable cableRojo;
    [SerializeField] private Cable cableVerde;
    [SerializeField] private Cable cableGris;

    private Cable cableSeleccionado;

    void Update()
    {
        hayCableSeleccionado = ComprobarColisionConCable();
        ComprobarInteraccion();
        return;
    }

    public void SelectCable(Cable cablePorSeleccionar)
    {
        cableSeleccionado = cablePorSeleccionar;
        return;
    }

    public void CutCable() 
    {
        if(cableSeleccionado == null)
        {
            return;
        }

        cableSeleccionado.Cut();
        LevelManager.EventoCortarCable?.Invoke(cableSeleccionado);
        return;
    }

    private bool ComprobarColisionConCable()
    {
        // Lanzar un rayo desde la posición del mouse hacia abajo en el eje Y
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayo, out hitInfo, Mathf.Infinity, capaCable))
        {
            // Verificar si el rayo ha colisionado con un objeto de la capa de cables
            GameObject objetoColisionado = hitInfo.collider.gameObject;

            if (objetoColisionado.CompareTag("Cable"))
            {
                //Se selecciona dicho cable
                Cable cablePorSeleccionar = objetoColisionado.GetComponent<Cable>();
                SelectCable(cablePorSeleccionar);
                return true;
            }
        }
        return false;
    }

    private void ComprobarInteraccion()
    {
        if (!hayCableSeleccionado)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            CutCable();
            return;
        }
        return;
    }
}
