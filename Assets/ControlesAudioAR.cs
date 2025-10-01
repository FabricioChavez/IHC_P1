using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ControlSimpleVisible : MonoBehaviour
{
    [Header("Botones UI (arrastrar aquí)")]
    public Button botonPlay;
    public Button botonPausa;
    public Button botonContinue;

    private AudioSource ultimoAudio;

    void Awake()
    {
        Debug.Log("[ControlSimpleVisible] Awake");
    }

    void Start()
    {
        // Comprobar asignaciones
        if (botonPlay == null || botonPausa == null || botonContinue == null)
        {
            Debug.LogWarning("[ControlSimpleVisible] Uno o más botones NO están asignados en el Inspector.");
        }

        if (botonPlay != null) botonPlay.onClick.AddListener(Play);
        if (botonPausa != null) botonPausa.onClick.AddListener(Pausar);
        if (botonContinue != null) botonContinue.onClick.AddListener(Continuar);
    }

    void Update()
    {
        // Buscar cualquier AudioSource que esté sonando o tenga progreso
        AudioSource[] todos = FindObjectsOfType<AudioSource>();
        AudioSource activo = todos.FirstOrDefault(a => a.isPlaying || a.time > 0f);

        if (activo != null)
        {
            ultimoAudio = activo;

            if (activo.isPlaying)
            {
                botonPlay?.gameObject.SetActive(false);
                botonPausa?.gameObject.SetActive(true);
                botonContinue?.gameObject.SetActive(false);
            }
            else if (activo.time > 0f)
            {
                botonPlay?.gameObject.SetActive(false);
                botonPausa?.gameObject.SetActive(false);
                botonContinue?.gameObject.SetActive(true);
            }
            else
            {
                botonPlay?.gameObject.SetActive(true);
                botonPausa?.gameObject.SetActive(false);
                botonContinue?.gameObject.SetActive(false);
            }
        }
        else
        {
            botonPlay?.gameObject.SetActive(true);
            botonPausa?.gameObject.SetActive(false);
            botonContinue?.gameObject.SetActive(false);
        }
    }

    void Play()
    {
        if (ultimoAudio != null)
        {
            ultimoAudio.time = 0f;
            ultimoAudio.Play();
            Debug.Log("[ControlSimpleVisible] Play -> " + ultimoAudio.gameObject.name);
        }
        else Debug.LogWarning("[ControlSimpleVisible] Play pulsado, pero ultimoAudio == null");
    }

    void Pausar()
    {
        if (ultimoAudio != null && ultimoAudio.isPlaying)
        {
            ultimoAudio.Pause();
            Debug.Log("[ControlSimpleVisible] Pausa -> " + ultimoAudio.gameObject.name);
        }
        else Debug.LogWarning("[ControlSimpleVisible] Pausa pulsado, pero no hay audio reproduciéndose");
    }

    void Continuar()
    {
        if (ultimoAudio != null && !ultimoAudio.isPlaying && ultimoAudio.time > 0f)
        {
            ultimoAudio.UnPause();
            Debug.Log("[ControlSimpleVisible] Continuar -> " + ultimoAudio.gameObject.name);
        }
        else Debug.LogWarning("[ControlSimpleVisible] Continuar pulsado, pero no hay audio pausado");
    }
}
