using UnityEngine;
using Vuforia;
using System.Collections;

public class ControlCanvasEstricto : MonoBehaviour
{
    public Canvas mainCanvas;
    public Canvas imgCanvas;
    
    private Camera arCamera;
    private float tiempoSinDeteccion = 0;
    
    void Start()
    {
        arCamera = VuforiaBehaviour.Instance.GetComponent<Camera>();
        MostrarMainCanvas();
    }
    
    void Update()
    {
        // Verificar si hay algún target visible en la cámara
        bool hayTargetVisible = false;
        
        ImageTargetBehaviour[] targets = FindObjectsOfType<ImageTargetBehaviour>();
        foreach (var target in targets)
        {
            if (target.TargetStatus.Status == Status.TRACKED)
            {
                hayTargetVisible = true;
                break;
            }
        }
        
        if (hayTargetVisible)
        {
            tiempoSinDeteccion = 0;
            
            if (mainCanvas.enabled)
            {
                mainCanvas.enabled = false;
                imgCanvas.enabled = true;
            }
        }
        else
        {
            tiempoSinDeteccion += Time.deltaTime;
            
            // Después de 0.5 segundos sin target, volver al menú
            if (tiempoSinDeteccion > 0.5f && imgCanvas.enabled)
            {
                mainCanvas.enabled = true;
                imgCanvas.enabled = false;
            }
        }
    }
    
    void MostrarMainCanvas()
    {
        mainCanvas.enabled = true;
        imgCanvas.enabled = false;
    }
}