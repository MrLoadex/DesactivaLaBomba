using UnityEngine;

public class Cable : MonoBehaviour
{
    [Header("Estados del Cable")]
    [SerializeField] private GameObject estadoEntero;
    [SerializeField] private GameObject estadoCortado;

    [SerializeField] private ColorCable color;
    public ColorCable ColorCable => color;
    
    private bool cutCable;

    public void Cut()
    {
        if(cutCable == true)
        {
            return;
        }

        estadoEntero.SetActive(false);
        estadoCortado.SetActive(true);
        cutCable = true;
    }
}
