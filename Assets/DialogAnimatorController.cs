using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum DIALOG_ANIMATION_MODE
{
    NORMAL,
    PANIC
}

public enum DIALOG_POSITION
{
    LEFT,
    RIGHT
}


public class DialogAnimatorController : MonoBehaviour
{
    private DIALOG_POSITION _dialogPosition;
    private Vector3 targetDialogPosition;
    private Vector3 targetDialogScale;

    private Vector3 chatBoxPosition;
    public GameObject squirrelWorldPosition;

    public GameObject chatBox;
    public GameObject charactersPrefab;
    public GameObject charactersParent;
    private GameObject[] characterObjects;
    public TextMeshProUGUI mainText;
    public GameObject parentDialog;
    public RectTransform dialogBoxTransform;

    private float timePerCharacter = 0.01f;
    private float shakeOffset = 0.5f;
    private float shakeOffsetSpeed = 25.0f;
    private float shakeMagnitude = 0.25f;
    private float shakeSpeed = 2.5f;

    private float hueSpeed = 0.1f;
    private float valueSpeed = 1f;
    private float minValue = 0.5f;
    private float value;
    private float hue;
    [SerializeField] private Image ChatBoxImage;

    private Coroutine currentTextAnimation;
    private bool isRunning = true;

    public delegate void SelectedButtonChangedEventHandler(BUTTON newSelectedButton);
    public delegate void TimeOfDayChangedEventHandler(TIME_OF_DAY newTimeOfDay);
    public delegate void GameModeChangedEventHandler(GAME_MODE newGameMode);
    public delegate void GameStateChangedEventHandler(GAME_STATE newGameState);

    void Start()
    {
        GlobalUIManager.OnSelectedButtonChanged += HandleSelectedButtonChanged;
        ScenesManager.OnGameStateChanged += HandleGameStateChanged;
        ScenesManager.OnGameModeChanged += HandleGameModeChanged;
        SaveManager.OnTimeOfDayChanged += HandleTimeOfDayChanged;

        Color textColor = mainText.color;
        textColor.a = 0f;
        mainText.color = textColor;
        mainText.text = "";
        targetDialogScale = Vector3.zero;
        parentDialog.transform.localScale = targetDialogScale;

        AnimateDialogText("HI!", DIALOG_ANIMATION_MODE.NORMAL);
    }

    private void OnDisable()
    {
        GlobalUIManager.OnSelectedButtonChanged -= HandleSelectedButtonChanged;
        ScenesManager.OnGameStateChanged -= HandleGameStateChanged;
        ScenesManager.OnGameModeChanged -= HandleGameModeChanged;
        SaveManager.OnTimeOfDayChanged -= HandleTimeOfDayChanged;
    }

    void Update()
    {
        hue += hueSpeed * Time.deltaTime;
        if (hue >= 1f) hue -= 1f;

        value = Mathf.Max(minValue, Mathf.PingPong(Time.time * valueSpeed, 1f));

        Color color = Color.HSVToRGB(hue, 0.4f, 1f);
        ChatBoxImage.color = color;
    }

    void FixedUpdate()
    {
        CalculateWorldToCanvasPoint();
    }

    public void CalculateWorldToCanvasPoint()
    {
        if (squirrelWorldPosition != null)
        {
            chatBoxPosition.x = squirrelWorldPosition.transform.position.x;
            chatBoxPosition.y = squirrelWorldPosition.transform.position.y;
            chatBoxPosition.z = squirrelWorldPosition.transform.position.z;
        }

        if (Camera.main != null)
        {
            Camera cameraRef = Camera.main;
            parentDialog.transform.position = cameraRef.WorldToScreenPoint(chatBoxPosition);
            float scale = ConvertValue(cameraRef.orthographicSize);
            parentDialog.transform.localScale = targetDialogScale * scale;
        }
    }

    public float ConvertValue(float value)
    {
        if (value <= 75f)
        {
            return 0.7f;
        }
        else if (value >= 150f)
        {
            return 0.3f;
        }
        else
        {
            float normalizedValue = (value - 75f) / 75f;
            float reversedValue = 1f - normalizedValue;
            return reversedValue * 0.4f + 0.3f;
        }
    }

    public void AnimateDialogText(string text, DIALOG_ANIMATION_MODE animationMode)
    {
        ClearText();

        if (currentTextAnimation != null)
        {
            StopCoroutine(currentTextAnimation);
            currentTextAnimation = null;
            isRunning = false;
        }

        mainText.text = text;
        characterObjects = new GameObject[mainText.text.Length];

        for (int i = 0; i < characterObjects.Length; i++)
        {
            GameObject characterObj = Instantiate(charactersPrefab, charactersParent.transform);
            characterObjects[i] = characterObj;
        }

        currentTextAnimation = StartCoroutine(AnimateText(text, animationMode));
    }

    public void ClearDialogText()
    {
        ClearText();

        if (currentTextAnimation != null)
        {
            StopCoroutine(currentTextAnimation);
            currentTextAnimation = null;
            isRunning = false;
        }

        currentTextAnimation = StartCoroutine(AnimateChatBox(false));
    }


    private IEnumerator AnimateText(string text, DIALOG_ANIMATION_MODE animationMode)
    {
        float animationDuration = 0.5f;
        float easedTime;

        Vector3 startScale = parentDialog.transform.localScale;
        Vector3 endScale = Vector3.zero;

        float t = 0f;

        while (t < 1f)
        {
            easedTime = EaseInOutShine(t);

            targetDialogScale = Vector3.Lerp(startScale, endScale, easedTime);

            t += Time.fixedDeltaTime / animationDuration;

            yield return null;
        }

        startScale = Vector3.zero;
        endScale = GetDialogPosition();

        t = 0f;

        while (t < 1f)
        {
            easedTime = EaseInOutShine(t);

            targetDialogScale = Vector3.Lerp(startScale, endScale, easedTime);

            t += Time.fixedDeltaTime / animationDuration;

            yield return null;
        }

        for (int i = 0; i < characterObjects.Length; i++)
        {
            // Create a new character GameObject with TextMeshProUGUI component
            GameObject characterObj = characterObjects[i];
            TextMeshProUGUI character = characterObj.GetComponent<TextMeshProUGUI>();
            character.text = text[i].ToString();
            character.rectTransform.sizeDelta = new Vector2(mainText.fontSize, mainText.fontSize);

            // Set initial properties for the character
            Vector3 targetPosition = GetCharacterPosition(i);
            Vector3 initialPosition = targetPosition + Vector3.up * 2f + Vector3.right * -2f;

            float initialZRotation = -20f;
            character.rectTransform.localRotation = Quaternion.Euler(0f, 0f, initialZRotation);

            Vector3 initialScale = Vector3.one * 1.5f;
            character.rectTransform.localScale = initialScale;

            float initialAlpha = 0.5f;
            Color initialColor = character.color;
            initialColor.a = initialAlpha;
            character.color = initialColor;

            // Set target properties for the character
            float targetZRotation = 0f;
            Vector3 targetScale = Vector3.one;
            float targetAlpha = 1f;

            // Animate the character
            float timeElapsed = 0f;
            while (timeElapsed < timePerCharacter)
            {
                easedTime = EaseInOutShine(timeElapsed / timePerCharacter);

                // Lerp Position
                character.rectTransform.localPosition = Vector3.Lerp(initialPosition, targetPosition, easedTime);

                // Lerp rotation
                character.rectTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(initialZRotation, targetZRotation, easedTime));

                // Lerp scale
                character.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, easedTime);

                // Lerp alpha
                Color newColor = character.color;
                newColor.a = Mathf.Lerp(initialAlpha, targetAlpha, easedTime);
                character.color = newColor;

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Set final properties for the character
            character.rectTransform.localPosition = targetPosition;
            character.rectTransform.localRotation = Quaternion.Euler(0f, 0f, targetZRotation);
            character.rectTransform.localScale = targetScale;
            Color finalColor = character.color;
            finalColor.a = targetAlpha;
            character.color = finalColor;

            // Set properties based on animation mode
            if (animationMode == DIALOG_ANIMATION_MODE.PANIC)
            {
                // Add shake effect to the character
                StartCoroutine(Shake(characterObj));
            }
        }
    }

    public Vector2 GetCharacterPosition(int charIndex)
    {
        TMP_TextInfo textInfo = mainText.textInfo;

        // Make sure the character index is valid
        if (charIndex < 0 || charIndex >= textInfo.characterCount)
        {
            Debug.LogWarning("Invalid character index: " + charIndex);
            return Vector2.zero;
        }

        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

        Vector3 charPos = (charInfo.topLeft + charInfo.bottomRight) / 2f;

        return charPos;
    }

    private IEnumerator AnimateChatBox(bool InOut)
    {
        float animationDuration = 1f;
        float easedTime;

        Vector3 startScale = InOut ? Vector3.zero : parentDialog.transform.localScale;
        Vector3 endScale = InOut ? GetDialogPosition() : Vector3.zero;

        float t = 0f;

        while (t < 1f)
        {
            easedTime = EaseInOutShine(t);

            targetDialogScale = Vector3.Lerp(startScale, endScale, easedTime);

            t += Time.fixedDeltaTime / animationDuration;

            yield return null;
        }
    }


    private IEnumerator Shake(GameObject characterObject)
    {
        isRunning = true;

        Vector3 initialPosition = characterObject.transform.localPosition;
        Quaternion initialRotation = characterObject.transform.localRotation;
        float timeElapsed = 0f;

        while (isRunning)
        {
            timeElapsed += Time.deltaTime;

            float x = initialPosition.x + Mathf.Sin(timeElapsed * shakeOffsetSpeed) * shakeOffset;
            float y = initialPosition.y + Mathf.Cos(timeElapsed * shakeOffsetSpeed) * shakeOffset;
            Vector3 newPosition = new Vector3(x, y, initialPosition.z);

            float zRot = initialRotation.eulerAngles.z + Mathf.Sin(timeElapsed * shakeSpeed * 10f) * shakeMagnitude * 10f;
            Quaternion newRotation = Quaternion.Euler(0f, 0f, zRot);

            characterObject.transform.localPosition = newPosition;
            characterObject.transform.localRotation = newRotation;

            yield return null;
        }
    }

    private void ClearText()
    {
        mainText.text = "";

        for (int i = 0; i < charactersParent.transform.childCount; i++)
        {
            GameObject childChar = charactersParent.transform.GetChild(i).gameObject;
            Destroy(childChar);
        }
    }

    private Vector3 GetDialogPosition()
    {
        Vector3 dialogScale = Vector3.one;

        switch (_dialogPosition)
        {
            case DIALOG_POSITION.LEFT:
                dialogScale.x *= -1;
                break;
            case DIALOG_POSITION.RIGHT:
                break;
        }

        charactersParent.transform.localScale = dialogScale;
        mainText.transform.localScale = dialogScale;
        return dialogScale;
    }

    private void HandleSelectedButtonChanged(BUTTON newSelectedButton)
    {
        switch (newSelectedButton)
        {
            case BUTTON.INFINITE_MODE:
                AnimateDialogText("Dodge holes, bees, worms and bears! Be careful! Also, bring me my fruit!", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.LEGACY_MODE:
                AnimateDialogText("I have an original Ice Cold Beer Arcade in my lounge, you wanna play?", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.SETTINGS:
                AnimateDialogText("Let's tweak some things!", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.ANIMATION:
                AnimateDialogText("Let's watch a movie!", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.MISC:
                AnimateDialogText("A lot of stuff going on here!", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.QUIT:
                AnimateDialogText("Heh, don't you dare to leave! There is still plenty of fun to be made!", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.CONFIRM_QUIT:
                AnimateDialogText("YOU REALLY WANNA LEAVE?!?!", DIALOG_ANIMATION_MODE.PANIC);
                break;
            case BUTTON.TIME_OF_DAY:
                AnimateDialogText("We can skip time here, try it!", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.RESET_DATA:
                AnimateDialogText("You wanna reset your data?", DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.CONFIRM_RESET_DATA:
                AnimateDialogText("ARE YOU SURE YOU WANNA RESET ALL OF YOUR BEST SCORES?", DIALOG_ANIMATION_MODE.PANIC);
                break;
            case BUTTON.ANY_KEY:
                if (ScenesManager.gameMode == GAME_MODE.NONE)
                {
                    AnimateDialogText("HI!", DIALOG_ANIMATION_MODE.NORMAL);
                }
                if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
                {
                    AnimateDialogText("GET READY FOR SOME FUN!", DIALOG_ANIMATION_MODE.NORMAL);
                }
                break;
            case BUTTON.NONE:
                ClearDialogText();
                break;
            case BUTTON.MUSIC:
                AnimateDialogText("THIS WILL SOUND SO GOOD IN MY EARS!", DIALOG_ANIMATION_MODE.NORMAL);
                break;
        }
    }


    private void HandleGameStateChanged(GAME_STATE newGameState)
    {
        switch (newGameState)
        {
            case GAME_STATE.PRE_GAME:
                break;
            case GAME_STATE.PREPARING:
                break;
            case GAME_STATE.ACTIVE:
                break;
            case GAME_STATE.INACTIVE:
                break;
            case GAME_STATE.LOADING:
                break;
            case GAME_STATE.GAME_OVER:
                break;
            case GAME_STATE.LEVEL_COMPLETED:
                break;
            case GAME_STATE.GAME_COMPLETED:
                break;
        }
    }

    private void HandleGameModeChanged(GAME_MODE newGameMode)
    {
        //ClearDialogText();

        switch (newGameMode)
        {
            case GAME_MODE.NONE:
                break;
            case GAME_MODE.INFINITE_MODE:
                break;
            case GAME_MODE.LEGACY_MODE:
                break;
        }
    }

    private void HandleTimeOfDayChanged(TIME_OF_DAY newTimeOfDay)
    {
        switch (newTimeOfDay)
        {
            case TIME_OF_DAY.NOON:
                _dialogPosition = DIALOG_POSITION.RIGHT;
                break;
            case TIME_OF_DAY.NIGHT:
                _dialogPosition = DIALOG_POSITION.LEFT;
                break;
        }

        AnimateDialogText(mainText.text, DIALOG_ANIMATION_MODE.NORMAL);
    }


    // ====================================== //
    // ========== EASING FUNCTIONS ========== //
    // ====================================== //

    float EaseInOutShine(float x)
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }

}
