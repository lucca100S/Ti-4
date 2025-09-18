using UnityEngine;

[System.Serializable]
public class InputInfo
{
    public enum Type
    {
        KeyCode,
        Button,
        NewInput
    }

    [SerializeField] private Type m_type;
    [SerializeField] private string m_inputName;
    [SerializeField] private KeyCode m_inputKeyCode;

    [SerializeField] private float m_bufferTime;
    [SerializeField] private float m_delayTime;

    private float m_lastInput;

    //Returns
    private bool m_isEnabled;

    private bool m_isPressed;
    private bool m_isUp;
    private bool m_isDown;

    #region Properties

    public Type type
    {
        get { return m_type; }
    }

    public float lastInput
    {
        get { return m_lastInput; }
    }

    public float bufferTime
    {
        get { return m_bufferTime; }
    }

    public float delayTime
    {
        get { return m_delayTime; }
    }

    public bool isEnabled
    {
        get { return m_isEnabled; }
    }

    public bool isPressed
    {
        get { return m_isPressed; }
    }

    public bool isUp
    {
        get { return m_isUp; }
    }

    public bool isDown
    {
        get { return m_isDown; }
    }

    #endregion

    public InputInfo(Type type, float bufferTime = 0.2f, float delayTime = 0.2f, string inputName = "Horizontal", KeyCode keycode = KeyCode.None)
    {
        m_type = type;
        m_bufferTime = bufferTime;
        m_delayTime = delayTime;
        m_inputName = inputName;
        m_inputKeyCode = keycode;

        m_lastInput = -1;
    }

    public void GetInput()
    {
        m_isPressed = false;
        m_isUp = false;
        m_isDown = false;

        switch (m_type)
        {
            case Type.Button:
                m_isPressed = Input.GetButton(m_inputName);
                m_isUp = Input.GetButtonUp(m_inputName);
                m_isDown = Input.GetButtonDown(m_inputName);
                break;
            case Type.NewInput:
                
                break;
            case Type.KeyCode:
                m_isPressed = Input.GetKey(m_inputKeyCode);
                m_isUp = Input.GetKeyUp(m_inputKeyCode);
                m_isDown = Input.GetKeyDown(m_inputKeyCode);
                break;
        }

        if(m_isDown)
        {
            m_lastInput = Time.time;
        }

        m_isEnabled = Time.time < m_lastInput + m_bufferTime;

    }

    public bool GetDelayInput(float time)
    {
        GetInput();
        return Time.time < time + m_delayTime && m_isDown;
    }
}
