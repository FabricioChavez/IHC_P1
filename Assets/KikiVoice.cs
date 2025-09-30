using UnityEngine;

public class KikiLipSync : MonoBehaviour // ← Cambiado a MonoBehaviour
{
    [Header("Audio")]
    public AudioClip audioExplicacion;
    
    [Header("BlendShapes para la boca")]
    public SkinnedMeshRenderer faceRenderer;
    public string blendShapeName = "Ahh";
    public float sensibilidad = 500f;
    public float suavizado = 10f;
    
    private AudioSource audioSource;
    private int blendShapeIndex = -1;
    private float valorActual = 0f;
    private static KikiLipSync personajeActivo = null; // Para evitar múltiples audios
    
    void Start()
    {
        // Configurar audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        
        // BUSCAR Face.003 automáticamente
        if (faceRenderer == null)
        {
            Transform face = transform.Find("Face.003");
            if (face != null)
            {
                faceRenderer = face.GetComponent<SkinnedMeshRenderer>();
                Debug.Log("✅ Encontrado Face.003");
            }   
            else
            {
                SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var r in renderers)
                {
                    if (r.name.Contains("Face"))
                    {
                        faceRenderer = r;
                        Debug.Log($"✅ Usando: {r.name}");
                        break;
                    }
                }
            }
        }
        
        // Encontrar el índice del blendshape
        if (faceRenderer != null)
        {
            for (int i = 0; i < faceRenderer.sharedMesh.blendShapeCount; i++)
            {
                string name = faceRenderer.sharedMesh.GetBlendShapeName(i);
                
                if (name.ToLower().Contains("ahh") || name.ToLower().Contains("ah"))
                {
                    blendShapeIndex = i;
                    Debug.Log($"✅ Usando blendshape: {name} en índice {i}");
                    break;
                }
            }
        }
    }
    
    // MÉTODO PÚBLICO para que Vuforia lo llame
    public void IniciarAudio()
    {
        // Detener cualquier otro personaje que esté hablando
        if (personajeActivo != null && personajeActivo != this)
        {
            personajeActivo.DetenerAudio();
        }
        personajeActivo = this;
        
        if (audioExplicacion != null && audioSource != null)
        {
            audioSource.clip = audioExplicacion;
            audioSource.Play();
            Debug.Log("🎵 Audio iniciado con lip sync");
        }
        
        

    }
    
    // MÉTODO PÚBLICO para que Vuforia lo llame
    public void DetenerAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        // Cerrar boca
        if (faceRenderer != null && blendShapeIndex >= 0)
        {
            faceRenderer.SetBlendShapeWeight(blendShapeIndex, 0);
            valorActual = 0f;
        }
        
        if (personajeActivo == this)
        {
            personajeActivo = null;
        }
    }
    
    void Update()
    {
        // Solo animar si está reproduciendo audio
        if (audioSource != null && audioSource.isPlaying && faceRenderer != null && blendShapeIndex >= 0)
        {
            // Obtener volumen actual
            float[] samples = new float[64];
            audioSource.GetOutputData(samples, 0);
            
            float sum = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                sum += Mathf.Abs(samples[i]);
            }
            
            float promedio = sum / samples.Length;
            
            // Convertir a valor de blendshape (0-100)
            float valorObjetivo = Mathf.Clamp(promedio * sensibilidad, 0, 100);
            
            // Suavizar el movimiento
            valorActual = Mathf.Lerp(valorActual, valorObjetivo, Time.deltaTime * suavizado);
            
            // Aplicar al blendshape
            faceRenderer.SetBlendShapeWeight(blendShapeIndex, valorActual);
        }
    }
    
 
}