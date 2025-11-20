using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class UIComponent : MonoBehaviour
{
    public override string ToString()
    {
        return $"UIComponent {this.gameObject.name} ";
    }
}