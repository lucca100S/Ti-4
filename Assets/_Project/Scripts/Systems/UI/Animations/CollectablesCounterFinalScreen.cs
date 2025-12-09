using System.Collections;
using UnityEngine;
using TMPro;

public class CollectablesCounterFinalScreen : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private TextMeshProUGUI textMeshPro;

    [Header("Valores")]
    [SerializeField] private int total = 100;          // Valor total
    [SerializeField] private int collected = 0;        // Valor coletado final
    [SerializeField] private float animationDuration = 1f; // Tempo total da animação
    [SerializeField] private float startDelay = 0.5f; // Delay antes de iniciar a animação (em segundos)
    public CollectableType collectableType;
    private void OnEnable()
    {
        switch (collectableType)
        {
            case CollectableType.Common:
                collected = CountCollectables.CollectedNumberByType(CollectableType.Common);
                break;
            case CollectableType.Hidden:
                collected = CountCollectables.CollectedNumberByType(CollectableType.Hidden);
                break;
        }
        // Inicializa a animação
        StartCoroutine(AnimateCollectables());
    }

    private IEnumerator AnimateCollectables()
    {
        // Aguarda o delay antes de iniciar a animação
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        float elapsed = 0f;
        int startValue = 0;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            // Interpola o valor do coletado
            int current = Mathf.RoundToInt(Mathf.Lerp(startValue, collected, elapsed / animationDuration));
            UpdateText(current, total);
            yield return null;
        }

        // Garante que o valor final esteja correto
        UpdateText(collected, total);
    }

    private void UpdateText(int currentCollected, int total)
    {
        // Formata com zeros à esquerda (ex: 005/100)
        string formattedText = $"{currentCollected:000}/{total:000}";
        textMeshPro.text = formattedText;
    }
}
