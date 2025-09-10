using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Classe que define os dados carregados de maneira genérica prlos coletáveis
/// </summary>
[CreateAssetMenu(fileName = "CollectableData", menuName = "Scriptable Objects/CollectableData")]
public class CollectableData : ScriptableObject
{
    public string collectableName;
    public string loreText;
    public Image collectableImage;
}
