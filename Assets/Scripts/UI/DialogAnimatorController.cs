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
    private List<string> welcomeDialogList = new List<string>()
    {
        "HI!",
        "Greetings, friend!",
        "Hey there, welcome aboard!",
        "Good to see you! Let's have some fun!",
        "Welcome, friend! Ready for a challenge?",
        "Hiya! Welcome to the game!",
        "Welcome, welcome! Glad you could make it.",
        "Howdy, partner! Welcome to the wild.",
        "Ahoy there! Welcome aboard our fruity adventure.",
        "Welcome to the funhouse! Let's get started.",
        "Squeak-squeak, welcome to my nutty world!",
        "Hey there, nut-lover! Welcome to our squirrelly adventure!",
        "Welcome to the squirrel's nest! Let's have some nutty fun!",
        "Get ready to go nuts, my friend! Welcome to my fun-filled world!",
        "Welcome to our forest, where the fruits and nuts are plentiful and the fun never stops!",
        "Squirrelly greetings! Welcome to our playful world!",
        "Welcome to the treehouse, where the fun and games never end!",
        "Get your tail shaking and your acorns ready, because you're in for a fun-filled welcome!",
        "Welcome to our cozy burrow, where the fun and games are endless!",
        "Let's play and explore together! Welcome to the squirrel's wonderland!"
    };

    private List<string> settingsDialogList = new List<string>()
    {
        "Let's tweak some things!",
        "Squeak-squeak! Let's get nutty and customize the game!",
        "This is where the magic happens, acorn addicts! Let's tweak some things!",
        "Woo-hoo! Welcome to the treehouse of customization!",
        "Let's shake our tails and make some changes in the settings, woohoo!",
        "Nutty squirrel alert! Get ready to make some adjustments!",
        "Squirrelly customization awaits! Let's go crazy with the settings!",
        "Nuts about tweaking settings? Welcome to the squirrel zone!",
        "Are you ready to play with the settings? Because I'm ready to play with my nuts!",
        "Welcome to the squirrel's playground! Let's hop into the settings and have some fun!",
        "You're in the right place if you want to make some nutty changes, my friends!"
    };

    private List<string> animationDialogList = new List<string>()
    {
        "Grab some nuts and get cozy, because it's movie time!",
        "Squirrelly movie alert! Let's get our popcorn and watch some animations!",
        "Welcome to our movie theatre in the tree! Let's get started with some animated fun!",
        "Get your acorns ready and your tail comfortable, because we're watching a video animation!",
        "Let's go on a nutty adventure together and watch a movie!",
        "It's time to sit back, relax, and enjoy some squirrel-approved entertainment!",
        "Get ready to be entertained, because we're watching a video animation!",
        "Welcome to our animated wonderland! Let's watch a movie and escape reality for a while!",
        "Get ready for some animated fun, because the movie is about to start!",
        "It's time to curl up and watch a video animation, squirrel-style!"
    };

    private List<string> miscDialogList = new List<string>()
    {
        "A lot of stuff going on here!",
        "Welcome to the Misc zone! This is where you can find all kinds of fun squirrel stuff!",
        "Miscellaneous? More like mis-squirrel-aneous! Check out all the cool stuff we have in here!",
        "Ready for a little squirrel exploration? Click on Misc to discover all the hidden treasures!",
        "Buckle up, friend! The Misc button is a wild ride full of squirrel art, trophies, and more!",
        "Don't be fooled by the name, Misc has a lot of fun stuff! Come and see for yourself!"
    };

    private List<string> quitDialogList = new List<string>()
    {
        "Heh, don't you dare to leave! There is still plenty of fun to be made!",
        "Wait! Are you sure you wanna leave? I still have plenty of fruits left for you to collect!",
        "Leaving so soon? But we were having so much fun! Let's keep playing!",
        "Don't press that button! There's still so much to explore in our squirrelly world!",
        "Hold on a sec! If you leave now, you'll miss out on all the nutty adventures we have planned!",
        "Please don't leave me hanging! Stay and play a little longer with me, pretty please?",
        "Leaving already? But I haven't even shown you my favorite tree in the forest yet!",
        "Hey, don't go! You're my favorite player! Let's play some more!",
        "Are you really leaving? But I was just about to teach you some new squirrel tricks!",
        "Stay and play a little longer! I promise we'll have a squirrel-tastic time together!",
        "Don't give up on the fun just yet! Let's keep playing and see what other squirrelly shenanigans we can get into!"
    };

    private List<string> confirmQuitDialogList = new List<string>()
    {
        "YOU REALLY WANNA LEAVE?!?!",
        "Are you sure you want to leave? Don't make me beg like a squirrel for you to stay!",
        "Wait, hold on a sec! Think about all the fun you're going to miss out on if you quit now!",
        "You really want to leave me all alone? But we were having so much fun together!",
        "Don't do it, friend! Think of all the squirrelly mischief we could get into if you stay a little longer!",
        "Really? You're going to leave just like that? What am I going to do with all these fruits?"
    };

    private List<string> resetDialogList = new List<string>()
    {
        "Feeling like a fresh start, huh? Click here to reset all your squirrelly data!",
        "Hey there! Want to erase all your high scores and start from scratch? This button's for you!",
        "Looking to shake things up? Resetting your data is the perfect way to do it! Just click here!",
        "Ready to turn over a new leaf? Hit that button and let's reset all your squirrel data together!",
        "Thinking about starting fresh? Just click here to reset everything and embark on a brand new squirrelly adventure!"
    };

    private List<string> confirmResetDialogList = new List<string>()
    {
        "ARE YOU SURE YOU WANNA RESET ALL OF YOUR BEST SCORES?",
        "You sure you want to reset everything? Are you absolutely positively sure?",
        "Wait, wait, wait! Before you hit that button, let's talk about all the memories we'll be deleting! Are you ready for this?",
        "You're really gonna reset all your data? That's a pretty big decision, squirrel friend! Are you sure you're ready for it?",
        "Think long and hard about this, my friend! Once you hit that button, there's no turning back! Are you ready to start fresh?",
        "I can't believe you're really going to do it! Are you sure you're not going to miss all those old high scores? You're really sure?"
    };

    private List<string> preGameDialogList = new List<string>()
    {
        "Let's go nuts!",
        "Game on, squirrel friend!",
        "Ready, set, climb!",
        "Get set, collect!",
        "Time to show off those squirrel skills!"
    };

    private List<string> musicDialogList = new List<string>()
    {
        "Let's groove to some tunes!",
        "Pump up the jams, squirrel style!",
        "Let's turn up the beat, acorn style!",
        "Are you ready to rock, squirrel friend?",
        "Let's make some noise, nutty style!"
    };

    private List<string> timeOfDayDialogList = new List<string>()
    {
        "We can skip time here, try it!",
        "Let's time travel, acorn style!",
        "Are you a morning or a night squirrel?",
        "Time flies when you're having fun!",
        "Let's see what time of day suits your squirrel style!",
        "Time waits for no squirrel, so let's change it up!"
    };

    private List<string> legacyDialogList = new List<string>()
    {
        "I have an original Ice Cold Beer Arcade in my lounge, you wanna play?",
        "Do you wanna play the original Ice Cold Beer arcade game with me?",
        "Get ready to play the arcade classic, Ice Cold Beer! Join me in Legacy mode for some retro squirrel fun!",
        "Hey, have you heard of Ice Cold Beer? It's the one game customers ask for by name! Come play with me in Legacy mode!",
        "Do you have the skills to play the classic Ice Cold Beer arcade game? Let's find out in Legacy mode!",
        "Come join me in Legacy mode for a trip down memory lane with the original arcade game, Ice Cold Beer!",
        "Ready to play the game that started it all? Let's go old school with Ice Cold Beer in Legacy mode!",
        "In Legacy mode, we can play the arcade classic, Ice Cold Beer! Let's see who can get the high score!",
        "Let's take a break from the squirrelly adventures and play some Ice Cold Beer in Legacy mode!",
        "I've got an original Ice Cold Beer arcade in my lounge! Wanna play? Join me in Legacy mode!",
        "Get your game on and let's play the classic Ice Cold Beer arcade game in Legacy mode! Can you beat my high score?"
    };

    private List<string> infiniteDialogList = new List<string>()
    {
        "Welcome to the infinite game mode! Let's dodge some obstacles and get some fruits!",
        "Squirrelly game alert! Let's climb the tree and bring me my fruit, while dodging bees, worms, holes, and bears!",
        "Get ready to dodge and climb, because we're playing Infinite mode! Also, don't forget to bring me some fruit!",
        "Hold onto your acorns, because we're going on a squirrel adventure! Watch out for the bees, worms, holes, and bears while bringing me my fruit!",
        "Get your tail ready for some squirrel acrobatics! In Infinite mode, you must dodge bees, worms, holes, and bears to bring me my delicious fruit!",
        "Let's get nutty in Infinite mode! We'll climb the tree, dodge obstacles, and grab fruit, all while having fun!",
        "Welcome to the squirrel's playground! In Infinite mode, we'll dodge bees, worms, holes, and bears while bringing me my precious fruit!",
        "Get your paws ready, because we're climbing the tree in Infinite mode! Watch out for the obstacles, and don't forget to bring me my fruit!",
        "Let's get our game on in Infinite mode! We'll climb higher and higher, dodge obstacles, and bring me my fruit!",
        "It's time to show off our squirrel skills in Infinite mode! Let's climb to the top, dodge obstacles, and bring me my delicious fruit!",
        "Dodge holes, bees, worms and bears! Be careful! Also, bring me my fruit!"
    };

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
                AnimateDialogText(RandomDialogChooser(infiniteDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.LEGACY_MODE:
                AnimateDialogText(RandomDialogChooser(legacyDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.SETTINGS:
                AnimateDialogText(RandomDialogChooser(settingsDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.ANIMATION:
                AnimateDialogText(RandomDialogChooser(animationDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.MISC:
                AnimateDialogText(RandomDialogChooser(miscDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.QUIT:
                AnimateDialogText(RandomDialogChooser(quitDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.CONFIRM_QUIT:
                AnimateDialogText(RandomDialogChooser(confirmQuitDialogList), DIALOG_ANIMATION_MODE.PANIC);
                break;
            case BUTTON.TIME_OF_DAY:
                AnimateDialogText(RandomDialogChooser(timeOfDayDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.RESET_DATA:
                AnimateDialogText(RandomDialogChooser(resetDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                break;
            case BUTTON.CONFIRM_RESET_DATA:
                AnimateDialogText(RandomDialogChooser(confirmResetDialogList), DIALOG_ANIMATION_MODE.PANIC);
                break;
            case BUTTON.ANY_KEY:
                if (ScenesManager.gameMode == GAME_MODE.NONE)
                {
                    AnimateDialogText(RandomDialogChooser(welcomeDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                }
                if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
                {
                    AnimateDialogText(RandomDialogChooser(preGameDialogList), DIALOG_ANIMATION_MODE.NORMAL);
                }
                break;
            case BUTTON.NONE:
                ClearDialogText();
                break;
            case BUTTON.MUSIC:
                AnimateDialogText(RandomDialogChooser(musicDialogList), DIALOG_ANIMATION_MODE.NORMAL);
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


    // Function to randomly choose a string from the list
    private string RandomDialogChooser(List<string> dialogList)
    {
        // Shuffle the list using Fisher-Yates algorithm
        for (int i = 0; i < dialogList.Count - 1; i++)
        {
            int j = UnityEngine.Random.Range(i, dialogList.Count);
            string temp = dialogList[i];
            dialogList[i] = dialogList[j];
            dialogList[j] = temp;
        }

        // Pick a random element from the shuffled list
        int randomIndex = Random.Range(0, dialogList.Count);
        return dialogList[randomIndex];
    }


    // ====================================== //
    // ========== EASING FUNCTIONS ========== //
    // ====================================== //

    float EaseInOutShine(float x)
    {
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
    }

}
