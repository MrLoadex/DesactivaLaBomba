using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractuableObject : MonoBehaviour
{
    [Header("Elementos Interaccion")]
    [SerializeField] protected GameObject botonInteractuar;

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && botonInteractuar.activeSelf)
        {
            Intaractuar();
        }
    }

    protected virtual void Intaractuar()
    {
        Debug.Log("Interactuando");
    }

    private void ActivarPanelInteraccion()
    {
        botonInteractuar.SetActive(true);
    }

    private void DesactivarPanelInteraccion()
    {
        botonInteractuar.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if( other.tag != "Player")
        {
            return;
        }
        ActivarPanelInteraccion();
    }

    private void OnTriggerExit(Collider other) 
    {
        if( other.tag != "Player")
        {
            return;
        }
        DesactivarPanelInteraccion();
    }

}
