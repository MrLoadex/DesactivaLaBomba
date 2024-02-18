using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luz : MonoBehaviour
{
    [SerializeField] private Material materialDeshabilitado;
    [SerializeField] private Material materialHabilitado;

    private Renderer _renderer;

    private void Start() {
        _renderer = GetComponent<Renderer>();
        _renderer.material = materialDeshabilitado;
    }

    public void Habilitar()
    {
        _renderer.material = materialHabilitado;
    }

    public void Desabilitar()
    {
        _renderer.material = materialDeshabilitado;
    }
}
