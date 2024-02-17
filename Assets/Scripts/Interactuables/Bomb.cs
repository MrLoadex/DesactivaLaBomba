using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : InteractuableObject
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bombCamera;
    [SerializeField] private GameObject botonDefuse;

    private bool bombaPanelActivo;

    protected override void Update()
    {
        if (!bombaPanelActivo)
        {
            base.Update();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            DesactivarPanelBomba();
        }

    }
    
    protected override void Intaractuar()
    {
        player.SetActive(false);
        bombCamera.SetActive(true);
        botonDefuse.SetActive(true);
        botonInteractuar.SetActive(false);

        bombaPanelActivo = true;
    }

    private void DesactivarPanelBomba()
    {
        player.SetActive(true);
        bombCamera.SetActive(false);
        botonDefuse.SetActive(false);
        botonInteractuar.SetActive(true);

        bombaPanelActivo = false;
    }
}
