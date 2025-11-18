using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SequentialTextDisplay : MonoBehaviour
{
    [Tooltip("Componente TextMeshProUGUI que exibirá o texto.")]
    public TextMeshProUGUI textField;

    [Tooltip("Lista de textos a serem exibidos em sequência.")]
    public List<string> textSequence = new();

    [Tooltip("Tempo em segundos entre cada troca de texto.")]
    public float changeInterval = 2f;

    private Coroutine displayRoutine;

    private void OnEnable()
    {
        if (textField == null || textSequence.Count == 0)
        {
            Debug.LogWarning("SequentialTextDisplay: faltando TextMeshProUGUI ou lista de textos vazia.");
            return;
        }

        displayRoutine = StartCoroutine(PlaySequenceLoop());
    }

    private void OnDisable()
    {
        if (displayRoutine != null)
            StopCoroutine(displayRoutine);

        // Opcional: limpa ou reseta o texto
        textField.text = string.Empty;
    }

    private IEnumerator PlaySequenceLoop()
    {
        int index = 0;

        while (true) // repete até o objeto ser desativado
        {
            textField.text = textSequence[index];

            yield return new WaitForSeconds(changeInterval);

            index++;
            if (index >= textSequence.Count)
                index = 0; // reinicia o ciclo
        }
    }
}
