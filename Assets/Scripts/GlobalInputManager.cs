using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[System.Serializable]
public enum Axis
{
    HORIZONTAL,
    VERTICAL
}

[System.Serializable]
public enum KeyBind
{
    LEFT_END_UP,
    LEFT_END_DOWN,
    RIGHT_END_UP,
    RIGHT_END_DOWN
}

[System.Serializable]
public enum InputType
{
    KEYBOARD,
    GAMEPAD
}

[System.Serializable]
public struct KeyBindDef
{
    public KeyBind bind;
    public GamepadButton gamepadBind;
    public Axis axis;
    public Key key;
}

[System.Serializable]
public struct PlayerInputKeybind
{
    public InputType inputType;
    public KeyBindDef leftEndUp;
    public KeyBindDef leftEndDown;
    public KeyBindDef rightEndUp;
    public KeyBindDef rightEndDown;
}

public class GlobalInputManager : MonoBehaviour
{

    public static GlobalInputManager instance;

    public static PlayerInputKeybind playerInputKeybind = new PlayerInputKeybind();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        setDefaultKeyBind();
    }

    public void setDefaultKeyBind()
    {

        playerInputKeybind.inputType = InputType.KEYBOARD;

        playerInputKeybind.leftEndUp.bind = KeyBind.LEFT_END_UP;
        playerInputKeybind.leftEndDown.bind = KeyBind.LEFT_END_DOWN;
        playerInputKeybind.rightEndUp.bind = KeyBind.RIGHT_END_UP;
        playerInputKeybind.rightEndDown.bind = KeyBind.RIGHT_END_DOWN;

        playerInputKeybind.leftEndUp.key = Key.W;
        playerInputKeybind.leftEndDown.key = Key.S;
        playerInputKeybind.rightEndUp.key = Key.I;
        playerInputKeybind.rightEndDown.key = Key.K;

        playerInputKeybind.leftEndUp.gamepadBind = GamepadButton.DpadUp;
        playerInputKeybind.leftEndDown.gamepadBind = GamepadButton.DpadDown;
        playerInputKeybind.rightEndUp.gamepadBind = GamepadButton.North;
        playerInputKeybind.rightEndDown.gamepadBind = GamepadButton.South;

    }

    public bool getBind(KeyBind key)
    {
        switch (playerInputKeybind.inputType)
        {
            case InputType.KEYBOARD:
                return getKeyBind(key);
            case InputType.GAMEPAD:
                return getGamepadBind(key);
            default:
                Debug.LogAssertion("Unknown Input Type!");
                break;
        }

        return false;
    }

    public bool getKeyBind(KeyBind key)
    {
        switch (key)
        {
            case KeyBind.LEFT_END_UP:
                return Keyboard.current[playerInputKeybind.leftEndUp.key].isPressed;
            case KeyBind.LEFT_END_DOWN:
                return Keyboard.current[playerInputKeybind.leftEndDown.key].isPressed;
            case KeyBind.RIGHT_END_UP:
                return Keyboard.current[playerInputKeybind.rightEndUp.key].isPressed;
            case KeyBind.RIGHT_END_DOWN:
                return Keyboard.current[playerInputKeybind.rightEndDown.key].isPressed;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }

        return false;
    }

    public bool getGamepadBind(KeyBind key)
    {
        switch (key)
        {
            case KeyBind.LEFT_END_UP:
                return Gamepad.current[playerInputKeybind.leftEndUp.gamepadBind].isPressed;
            case KeyBind.LEFT_END_DOWN:
                return Gamepad.current[playerInputKeybind.leftEndDown.gamepadBind].isPressed;
            case KeyBind.RIGHT_END_UP:
                return Gamepad.current[playerInputKeybind.rightEndUp.gamepadBind].isPressed;
            case KeyBind.RIGHT_END_DOWN:
                return Gamepad.current[playerInputKeybind.rightEndDown.gamepadBind].isPressed;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }

        return false;
    }

    public bool getBindUp(KeyBind key)
    {
        switch (playerInputKeybind.inputType)
        {
            case InputType.KEYBOARD:
                return getKeyBindUp(key);
            case InputType.GAMEPAD:
                return getGamepadBindUp(key);
            default:
                Debug.LogAssertion("Unknown Input Type!");
                break;
        }

        return false;
    }

    public bool getKeyBindUp(KeyBind key)
    {
        switch (key)
        {
            case KeyBind.LEFT_END_UP:
                return Keyboard.current[playerInputKeybind.leftEndUp.key].wasReleasedThisFrame;
            case KeyBind.LEFT_END_DOWN:
                return Keyboard.current[playerInputKeybind.leftEndDown.key].wasReleasedThisFrame;
            case KeyBind.RIGHT_END_UP:
                return Keyboard.current[playerInputKeybind.rightEndUp.key].wasReleasedThisFrame;
            case KeyBind.RIGHT_END_DOWN:
                return Keyboard.current[playerInputKeybind.rightEndDown.key].wasReleasedThisFrame;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }

        return false;
    }

    public bool getGamepadBindUp(KeyBind key)
    {
        switch (key)
        {
            case KeyBind.LEFT_END_UP:
                return Gamepad.current[playerInputKeybind.leftEndUp.gamepadBind].wasReleasedThisFrame;
            case KeyBind.LEFT_END_DOWN:
                return Gamepad.current[playerInputKeybind.leftEndDown.gamepadBind].wasReleasedThisFrame;
            case KeyBind.RIGHT_END_UP:
                return Gamepad.current[playerInputKeybind.rightEndUp.gamepadBind].wasReleasedThisFrame;
            case KeyBind.RIGHT_END_DOWN:
                return Gamepad.current[playerInputKeybind.rightEndDown.gamepadBind].wasReleasedThisFrame;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }

        return false;
    }

    public bool getBindDown(KeyBind key)
    {
        switch (playerInputKeybind.inputType)
        {
            case InputType.KEYBOARD:
                return getKeyBindDown(key);
            case InputType.GAMEPAD:
                return getGamepadBindDown(key);
            default:
                Debug.LogAssertion("Unknown Input Type!");
                break;
        }

        return false;
    }

    public bool getKeyBindDown(KeyBind key)
    {
        switch (key)
        {
            case KeyBind.LEFT_END_UP:
                return Keyboard.current[playerInputKeybind.leftEndUp.key].wasPressedThisFrame;
            case KeyBind.LEFT_END_DOWN:
                return Keyboard.current[playerInputKeybind.leftEndDown.key].wasPressedThisFrame;
            case KeyBind.RIGHT_END_UP:
                return Keyboard.current[playerInputKeybind.rightEndUp.key].wasPressedThisFrame;
            case KeyBind.RIGHT_END_DOWN:
                return Keyboard.current[playerInputKeybind.rightEndDown.key].wasPressedThisFrame;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }

        return false;
    }

    public bool getGamepadBindDown(KeyBind key)
    {
        switch (key)
        {
            case KeyBind.LEFT_END_UP:
                return Gamepad.current[playerInputKeybind.leftEndUp.gamepadBind].wasPressedThisFrame;
            case KeyBind.LEFT_END_DOWN:
                return Gamepad.current[playerInputKeybind.leftEndDown.gamepadBind].wasPressedThisFrame;
            case KeyBind.RIGHT_END_UP:
                return Gamepad.current[playerInputKeybind.rightEndUp.gamepadBind].wasPressedThisFrame;
            case KeyBind.RIGHT_END_DOWN:
                return Gamepad.current[playerInputKeybind.rightEndDown.gamepadBind].wasPressedThisFrame;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }

        return false;
    }

    public void setKeyBind(Key key, KeyBind keyBind)
    {
        switch (keyBind)
        {
            case KeyBind.LEFT_END_UP:
                playerInputKeybind.leftEndUp.key = key;
                break;
            case KeyBind.LEFT_END_DOWN:
                playerInputKeybind.leftEndDown.key = key;
                break;
            case KeyBind.RIGHT_END_UP:
                playerInputKeybind.rightEndUp.key = key;
                break;
            case KeyBind.RIGHT_END_DOWN:
                playerInputKeybind.rightEndDown.key = key;
                break;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }
    }

    public void setGamepadBind(GamepadButton bind, KeyBind keyBind)
    {
        switch (keyBind)
        {
            case KeyBind.LEFT_END_UP:
                playerInputKeybind.leftEndUp.gamepadBind = bind;
                break;
            case KeyBind.LEFT_END_DOWN:
                playerInputKeybind.leftEndDown.gamepadBind = bind;
                break;
            case KeyBind.RIGHT_END_UP:
                playerInputKeybind.rightEndUp.gamepadBind = bind;
                break;
            case KeyBind.RIGHT_END_DOWN:
                playerInputKeybind.rightEndDown.gamepadBind = bind;
                break;
            default:
                Debug.LogAssertion("Unknown KeyBind!");
                break;
        }
    }

    public void setPlayerInputType(InputType type)
    {
        playerInputKeybind.inputType = type;
    }

    public bool isKeyAlreadyBinded(Key key)
    {
        bool isBinded = false;

        if (playerInputKeybind.leftEndUp.key == key)
        {
            isBinded = true;
        }
        if (playerInputKeybind.leftEndDown.key == key)
        {
            isBinded = true;
        }
        if (playerInputKeybind.rightEndUp.key == key)
        {
            isBinded = true;
        }
        if (playerInputKeybind.rightEndDown.key == key)
        {
            isBinded = true;
        }

        return isBinded;
    }

    public bool isGamepadButtonAlreadyBinded(GamepadButton button)
    {
        bool isBinded = false;

        if (playerInputKeybind.leftEndUp.gamepadBind == button)
        {
            isBinded = true;
        }
        if (playerInputKeybind.leftEndDown.gamepadBind == button)
        {
            isBinded = true;
        }
        if (playerInputKeybind.rightEndUp.gamepadBind == button)
        {
            isBinded = true;
        }
        if (playerInputKeybind.rightEndDown.gamepadBind == button)
        {
            isBinded = true;
        }

        return isBinded;
    }

}


/*
 *     public GUIStyle style = new GUIStyle();
    private void OnGUI()
    {
        style.fontSize = 10;
        style.normal.textColor = Color.white;

        string data1 = "___________________________________________\n\n";
        //data1 += $"Player - {playersInputKeybind[playerDropdownIndex].playerIndex}\n";
        //data1 += $"inputType - {playersInputKeybind[playerDropdownIndex].inputType}\n";
        //data1 += $"BOOST (pressed)      - {getBind(KeyBind.BOOST, playerDropdownIndex)}\n";
        //data1 += $"FIRE (pressed)       - {getBind(KeyBind.FIRE, playerDropdownIndex)}\n";
        //data1 += $"FORWARD (pressed)    - {getBind(KeyBind.FORWARD, playerDropdownIndex)}\n";
        //data1 += $"BACKWARD (pressed)   - {getBind(KeyBind.BACKWARD, playerDropdownIndex)}\n";

        //GUI.Label(new Rect(10, 280, 400, 1000), data1, style);
        //GUI.Label(new Rect(210, 280, 400, 1000), data2, style);
        //GUI.Label(new Rect(410, 280, 400, 1000), data3, style);
        //GUI.Label(new Rect(610, 280, 400, 1000), data4, style);
    }

*/
