using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RollingTextMeshProElement : UITextMeshProElement
{
    [Tooltip("O RectTransform do conteúdo que deve rolar (geralmente o Text ou um container).")]
    public RectTransform content;

    [Tooltip("Velocidade de rolagem em unidades por segundo.")]
    public float scrollSpeed = 50f;

    [Tooltip("Altura máxima que o conteúdo deve rolar antes de parar.")]
    public float maxScrollHeight = 2000f;

    private Vector2 startPosition;
    private bool isScrolling = false;

    private void OnEnable()
    {
        if (content == null)
        {
            Debug.LogWarning("CreditsScroller: Nenhum RectTransform atribuído!");
            return;
        }

        // Salva a posição inicial do conteúdo
        startPosition = content.anchoredPosition;

        // Começa a rolagem
        isScrolling = true;
    }

    private void OnDisable()
    {
        // Para a rolagem e reseta a posição
        isScrolling = false;
        if (content != null)
            content.anchoredPosition = startPosition;
    }

    private void Update()
    {
        if (!isScrolling || content == null)
            return;

        // Move o conteúdo para cima (eixo Y negativo em UI é para baixo)
        content.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // Para ao atingir o limite
        if (content.anchoredPosition.y >= maxScrollHeight)
            isScrolling = false;
    }
}
