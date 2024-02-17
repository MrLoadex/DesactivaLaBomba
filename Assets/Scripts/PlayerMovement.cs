using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
    //[SerializeField] private Camera playerCamera;
    public float velocidadMovimiento = 5f;
    public float velocidadRotacion = 3f;

    void Update()
    {
        // Obtener entrada de teclado para el movimiento
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Calcular la dirección del movimiento
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0f, movimientoVertical).normalized;

        // Aplicar movimiento al jugador
        transform.Translate(movimiento * velocidadMovimiento * Time.deltaTime);

        // Obtener la posición del mouse para la rotación
        float rotacionHorizontal = Input.GetAxis("Mouse X") * velocidadRotacion;

        // Aplicar rotación al jugador
        transform.Rotate(Vector3.up * rotacionHorizontal);

        // Obtener la posición del mouse para la rotación vertical de la cámara
        float rotacionVertical = -Input.GetAxis("Mouse Y") * velocidadRotacion;

        // Obtener el ángulo actual de rotación vertical de la cámara
        float anguloRotacionVertical = transform.GetChild(0).transform.eulerAngles.x + rotacionVertical;

        // Limitar la rotación vertical entre -90 y 90 grados
        anguloRotacionVertical = Mathf.Clamp(anguloRotacionVertical, -90f, 90f);

        // Aplicar rotación vertical a la cámara
        //transform.GetChild(0).transform.eulerAngles = new Vector3(anguloRotacionVertical, transform.eulerAngles.y, 0f);

        // Asegurarse de que la cámara esté siempre mirando hacia el jugador
        //playerCamera.transform.LookAt(transform);
    }

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        
    }
}