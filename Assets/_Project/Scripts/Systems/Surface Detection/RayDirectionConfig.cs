using UnityEngine;

/// <summary>
/// Estrutura que define uma direção de raycast e sua distância.
/// </summary>
[System.Serializable]
public struct RayDirectionConfig
{
    public enum Type
    {
        Local,
        Global
    }

    public enum Direction
    {
        Forward,
        Back,
        Right,
        Left,
        Up,
        Down
    }

    public Type type;
    public Direction directionVector;
    public Transform target;
    
    [HideInInspector]
    public Vector3 direction
    {
        get
        {
            switch(type)
            {
                case Type.Local:
                    switch(directionVector)
                    {
                        case Direction.Forward:
                            return target.forward;
                        case Direction.Back:
                            return -target.forward;
                        case Direction.Right:
                            return target.right;
                        case Direction.Left:
                            return -target.right;
                        case Direction.Up:
                            return target.up;
                        case Direction.Down:
                            return -target.up;
                    }
                    break;
                case Type.Global:
                    switch (directionVector)
                    {
                        case Direction.Forward:
                            return Vector3.forward;
                        case Direction.Back:
                            return Vector3.back;
                        case Direction.Right:
                            return Vector3.right;
                        case Direction.Left:
                            return Vector3.left;
                        case Direction.Up:
                            return Vector3.up;
                        case Direction.Down:
                            return Vector3.down;
                    }
                    break;
            }

            return Vector3.zero;
        }
    }
    public float distance;
}
