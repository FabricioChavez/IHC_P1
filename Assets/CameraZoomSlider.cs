using UnityEngine;
using Vuforia;

public class ZoomCamaraAR : MonoBehaviour
{
    [Header("Configuración de Zoom")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 1f;
    public float maxZoom = 5f;
    
    private float currentZoom = 1f;
    private Camera arCamera;
    
    void Start()
    {
        // Buscar la ARCamera de Vuforia
        arCamera = VuforiaBehaviour.Instance.GetComponent<Camera>();
        if (arCamera == null)
        {
            arCamera = Camera.main;
        }
    }
    
    void Update()
    {
        // ZOOM CON PINCH (pellizco) en móvil
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            
            // Calcular distancia anterior
            Vector2 prevPos1 = touch1.position - touch1.deltaPosition;
            Vector2 prevPos2 = touch2.position - touch2.deltaPosition;
            float prevDistance = Vector2.Distance(prevPos1, prevPos2);
            
            // Calcular distancia actual
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            
            // Diferencia = zoom
            float deltaDistance = currentDistance - prevDistance;
            
            // Aplicar zoom
            currentZoom += deltaDistance * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            
            // Cambiar FOV de la cámara
            arCamera.fieldOfView = 60f / currentZoom;
        }
        
        // ZOOM CON BOTONES (para testing en Editor)
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Q))
        {
            currentZoom += zoomSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            currentZoom -= zoomSpeed;
        }
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        arCamera.fieldOfView = 60f / currentZoom;
#endif
    }
}