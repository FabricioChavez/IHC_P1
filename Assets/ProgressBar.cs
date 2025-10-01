using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ProgresoAudio : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider barraProgreso;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI nombrePersonaje;

    private AudioSource audioActual;
    private bool arrastrandoBarra = false;

    void Start()
    {
        if (barraProgreso != null)
        {
            barraProgreso.minValue = 0;
            barraProgreso.maxValue = 1;
            barraProgreso.interactable = true; // CAMBIAR A TRUE

            // Agregar listeners para detectar cuando se arrastra
            barraProgreso.onValueChanged.AddListener(OnBarraMovida);

            // Detectar cuando empieza/termina el arrastre
            var eventTrigger = barraProgreso.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

            // Cuando empieza a arrastrar
            var pointerDown = new UnityEngine.EventSystems.EventTrigger.Entry();
            pointerDown.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => { arrastrandoBarra = true; });
            eventTrigger.triggers.Add(pointerDown);

            // Cuando suelta
            var pointerUp = new UnityEngine.EventSystems.EventTrigger.Entry();
            pointerUp.eventID = UnityEngine.EventSystems.EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => { arrastrandoBarra = false; });
            eventTrigger.triggers.Add(pointerUp);
        }

        MostrarProgreso(false);
    }

    void Update()
    {
        AudioSource[] todos = FindObjectsOfType<AudioSource>();
        AudioSource activo = todos.FirstOrDefault(a => a.isPlaying || a.time > 0f);

        if (activo != null && activo.clip != null)
        {
            audioActual = activo;
            MostrarProgreso(true);

            // Solo actualizar la barra si NO se está arrastrando
            if (!arrastrandoBarra)
            {
                float progreso = activo.time / activo.clip.length;
                if (barraProgreso != null)
                    barraProgreso.value = progreso;
            }

            // Actualizar texto siempre
            if (textoTiempo != null)
            {
                int segundos = (int)activo.time;
                int total = (int)activo.clip.length;
                textoTiempo.text = $"{segundos}/{total}s";
            }

            if (nombrePersonaje != null)
            {
                nombrePersonaje.text = activo.gameObject.name;
            }
        }
        else
        {
            MostrarProgreso(false);
            audioActual = null;
        }
    }

    void OnBarraMovida(float valor)
    {
        // Solo cambiar posición del audio si se está arrastrando
        if (arrastrandoBarra && audioActual != null && audioActual.clip != null)
        {
            float nuevaPosicion = valor * audioActual.clip.length;
            audioActual.time = nuevaPosicion;

            // Actualizar texto inmediatamente
            if (textoTiempo != null)
            {
                int segundos = (int)nuevaPosicion;
                int total = (int)audioActual.clip.length;
                textoTiempo.text = $"{segundos}/{total}s";
            }
        }
    }

    void MostrarProgreso(bool mostrar)
    {
        if (barraProgreso != null)
            barraProgreso.gameObject.SetActive(mostrar);
        if (textoTiempo != null)
            textoTiempo.gameObject.SetActive(mostrar);
        if (nombrePersonaje != null)
            nombrePersonaje.gameObject.SetActive(mostrar);
    }

    void OnDestroy()
    {
        // Limpiar listener
        if (barraProgreso != null)
            barraProgreso.onValueChanged.RemoveListener(OnBarraMovida);
    }
}