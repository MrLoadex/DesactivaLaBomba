using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public enum ColorCable
{
    Rojo,
    Verde,
    Gris
}

[CreateAssetMenu(menuName ="LeveSettings")]
public class LevelSettings : ScriptableObject
{
    public ColorCable colorGanador;

    [Header("Objetivos Para Ganar")]
    
    public bool CortarCable;
    public bool CortarElectricidad;
    public bool DesifrarCodigo;

    [Header("Siguiente Escena")]
    public string nextSceneName;
}
