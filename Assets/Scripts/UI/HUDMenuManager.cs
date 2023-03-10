using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GAME_DATA
{
    SCORE,
    BONUS_SCORE,
    BEST_SCORE,
    LIFE,
    LEVEL,
    TIMER,
    ALL,
    NONE
};

public class HUDMenuManager : MonoBehaviour
{
    public static HUDMenuManager instance;

    public GameObject infinitePanel;
    public GameObject legacyPanel;

    public TMP_Text legacyScoreText;
    public TMP_Text legacyBonusText;
    public TMP_Text legacyBestScoreText;
    public TMP_Text legacyBallLifeText;

    public TMP_Text infiniteScoreText;
    public TMP_Text infiniteBonusText;
    public TMP_Text infiniteBestScoreText;
    public TMP_Text infiniteLevelText;
    public TMP_Text infiniteFruitLifeText;
    public TMP_Text infiniteTimerText;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            SetInfiniteHUD();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            SetLegacyHUD();
        }
    }

    private void OnEnable()
    {
        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            SetInfiniteHUD();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            SetLegacyHUD();
        }
    }

    public void SetInfiniteHUD()
    {
        legacyPanel.SetActive(false);
        infinitePanel.SetActive(true);
        //InfiniteGameController.instance.UpdateHUD();
        UpdateInfiniteHUD(GAME_DATA.ALL);
    }

    public void SetLegacyHUD()
    {
        infinitePanel.SetActive(false);
        legacyPanel.SetActive(true);
        //LegacyGameController.instance.UpdateHUD();
        UpdateLegacyHUD(GAME_DATA.ALL);
    }




    // =============================================== //
    // ========== MAIN ANIMATION CONTROLLER ========== //
    // =============================================== //

    public void UpdateLegacyHUD(GAME_DATA gameData)
    {
        switch (gameData)
        {
            case GAME_DATA.SCORE:
                StartCoroutine(AnimateText(legacyScoreText, LegacyGameController.instance.score.ToString()));
                break;
            case GAME_DATA.BONUS_SCORE:
                StartCoroutine(AnimateText(legacyBonusText, LegacyGameController.instance.bonusScore.ToString()));
                break;
            case GAME_DATA.BEST_SCORE:
                StartCoroutine(AnimateText(legacyBestScoreText, SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE).ToString()));
                break;
            case GAME_DATA.LIFE:
                StartCoroutine(AnimateText(legacyBallLifeText, LegacyGameController.instance.currentBallNumber.ToString()));
                break;
            case GAME_DATA.ALL:
                StartCoroutine(AnimateText(legacyScoreText, LegacyGameController.instance.score.ToString()));
                StartCoroutine(AnimateText(legacyBonusText, LegacyGameController.instance.bonusScore.ToString()));
                StartCoroutine(AnimateText(legacyBestScoreText, SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE).ToString()));
                StartCoroutine(AnimateText(legacyBallLifeText, LegacyGameController.instance.currentBallNumber.ToString()));
                break;
            case GAME_DATA.NONE:
                legacyScoreText.text = LegacyGameController.instance.score.ToString();
                legacyBonusText.text = LegacyGameController.instance.bonusScore.ToString();
                legacyBestScoreText.text = SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE).ToString();
                legacyBallLifeText.text = LegacyGameController.instance.currentBallNumber.ToString();
                break;
            default:
                Debug.LogAssertion("Unknown Game Data Type!");
                break;
        }
    }

    public void UpdateInfiniteHUD(GAME_DATA gameData)
    {
        switch (gameData)
        {
            case GAME_DATA.SCORE:
                StartCoroutine(AnimateText(infiniteScoreText, InfiniteGameController.instance.score.ToString()));
                break;
            case GAME_DATA.BONUS_SCORE:
                StartCoroutine(AnimateText(infiniteBonusText, InfiniteGameController.instance.bonusScore.ToString()));
                break;
            case GAME_DATA.BEST_SCORE:
                StartCoroutine(AnimateText(infiniteBestScoreText, SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE).ToString()));
                break;
            case GAME_DATA.LIFE:
                StartCoroutine(AnimateText(infiniteFruitLifeText, InfiniteGameController.instance.currentFruitNumber.ToString()));
                break;
            case GAME_DATA.LEVEL:
                StartCoroutine(AnimateText(infiniteLevelText, InfiniteGameController.instance.currentLevel.ToString()));
                break;
            case GAME_DATA.TIMER:
                StartCoroutine(AnimateText(infiniteTimerText, "00:00:00"));
                break;
            case GAME_DATA.ALL:
                StartCoroutine(AnimateText(infiniteScoreText, InfiniteGameController.instance.score.ToString()));
                StartCoroutine(AnimateText(infiniteBonusText, InfiniteGameController.instance.bonusScore.ToString()));
                StartCoroutine(AnimateText(infiniteBestScoreText, SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE).ToString()));
                StartCoroutine(AnimateText(infiniteFruitLifeText, InfiniteGameController.instance.currentFruitNumber.ToString()));
                StartCoroutine(AnimateText(infiniteLevelText, InfiniteGameController.instance.currentLevel.ToString()));
                                StartCoroutine(AnimateText(infiniteTimerText, "00:00:00"));
                break;
            case GAME_DATA.NONE:
                infiniteScoreText.text = InfiniteGameController.instance.score.ToString();
                infiniteBonusText.text = InfiniteGameController.instance.bonusScore.ToString();
                infiniteBestScoreText.text = SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE).ToString();
                infiniteFruitLifeText.text = InfiniteGameController.instance.currentFruitNumber.ToString();
                infiniteLevelText.text = InfiniteGameController.instance.currentLevel.ToString();
                break;
            default:
                Debug.LogAssertion("Unknown Game Data Type!");
                break;
        }
    }







    // ===================================== //
    // ========== TEXT ANIMATIONS ========== //
    // ===================================== //
    private IEnumerator AnimateText(TMP_Text text, string textToUpdate)
    {
        Vector3 startScale = text.transform.localScale;

        float animationDuration = 0.40f;
        float easedTime;

        float t = 0f;

        while (t < 1f)
        {
            easedTime = EaseOutQuint(t);


            text.transform.localScale = Vector3.LerpUnclamped(startScale, Vector3.one * 2f, easedTime);

            t += Time.deltaTime / animationDuration;

            yield return null;
        }

        text.text = textToUpdate;

        t = 0f;

        while (t < 1f)
        {
            easedTime = EaseInQuint(t);

            text.transform.localScale = Vector3.LerpUnclamped(Vector3.one * 2f, Vector3.one, easedTime);

            t += Time.deltaTime / animationDuration;

            yield return null;
        }

        text.transform.localScale = Vector3.one;

    }









    // ====================================== //
    // ========== TIMER ANIMATIONS ========== //
    // ====================================== //

    public void UpdateInfiniteTimer(float gameTime)
    {
        // get the total full seconds.
        var t0 = (int)gameTime;

        // get the number of minutes.
        var m = t0 / 60;

        // get the remaining seconds.
        var s = (t0 - m * 60);

        // get the 2 most significant values of the milliseconds.
        var ms = (int)((gameTime - t0) * 100);

        string timerString = $"{m:00}:{s:00}:{ms:00}";

        infiniteTimerText.text = timerString;
    }

    public void ResetInfiniteTimer()
    {
        StartCoroutine(AnimateInfiniteTimerReset());
    }

    private IEnumerator AnimateInfiniteTimerReset()
    {
        // Get the duration of the animation
        float animationDuration = 1.75f;

        string startTimerString = infiniteTimerText.text;
        string resetTimerString = "00:00:00";

        float timer = 0f;

        // Loop until the timer reaches the animation duration
        while (timer < animationDuration)
        {
            // Calculate the current timer string based on the animation progress
            string currentTimerString = GetAnimatedTimerString(startTimerString, resetTimerString, timer / animationDuration);

            //Mathf.Lerp()

            // Update the UI timer component with the current timer string
            //infiniteTimerText.text = currentTimerString;
            StartCoroutine(AnimateText(infiniteTimerText, currentTimerString));

            // Wait for the next frame
            yield return null;

            // Update the timer
            timer += Time.deltaTime;
        }

        // Set the final timer string to "00:00:00"
        infiniteTimerText.text = resetTimerString;
    }


    private string GetAnimatedTimerString(string startTimerString, string endTimerString, float t)
    {
        // Split the start and end timer strings into their components
        string[] startTimerComponents = startTimerString.Split(':');
        string[] endTimerComponents = endTimerString.Split(':');

        // Convert the components to integers
        int startMinutes = int.Parse(startTimerComponents[0]);
        int startSeconds = int.Parse(startTimerComponents[1]);
        int startMilliseconds = int.Parse(startTimerComponents[2]);
        int endMinutes = int.Parse(endTimerComponents[0]);
        int endSeconds = int.Parse(endTimerComponents[1]);
        int endMilliseconds = int.Parse(endTimerComponents[2]);

        // Calculate the current timer components based on the animation progress
        int currentMinutes = Mathf.RoundToInt(Mathf.Lerp(startMinutes, endMinutes, t));
        int currentSeconds = Mathf.RoundToInt(Mathf.Lerp(startSeconds, endSeconds, t));
        int currentMilliseconds = Mathf.RoundToInt(Mathf.Lerp(startMilliseconds, endMilliseconds, t));

        // Format the current timer components into a timer string
        return string.Format("{0:00}:{1:00}:{2:00}", currentMinutes, currentSeconds, currentMilliseconds);
    }






    // ==================================================== //
    // ========== SCORE & BONUS SCORE ANIMATIONS ========== //
    // ==================================================== //

    public IEnumerator AnimateInfiniteBonusScore(int currentBonusScore, int newBonusScore)
    {
        print($"CURRENT BONUS SCORE: {currentBonusScore}");
        print($"NEW BONUS SCORE WITH TIMER: {newBonusScore}");
        // Get the duration of the animation
        float animationDuration = 1.75f;

        float timer = 0f;

        string startTimerString = infiniteTimerText.text;
        string resetTimerString = "00:00:00";

        // Loop until the timer reaches the animation duration
        while (timer < animationDuration)
        {
            // Calculate the current timer string based on the animation progress
            string currentTimerString = GetAnimatedTimerString(startTimerString, resetTimerString, timer / animationDuration);
            StartCoroutine(AnimateText(infiniteTimerText, currentTimerString));

            string currentBonusScoreString = Mathf.FloorToInt(Mathf.Lerp(currentBonusScore, newBonusScore, timer / animationDuration)).ToString();
            StartCoroutine(AnimateText(infiniteBonusText, currentBonusScoreString));

            // Wait for the next frame
            yield return null;

            // Update the timer
            timer += Time.deltaTime;
        }

        infiniteBonusText.text = newBonusScore.ToString();

        // Set the final timer string to "00:00:00"
        infiniteTimerText.text = resetTimerString;

        yield return new WaitForSeconds(2f);
    }

    public IEnumerator AnimateInfiniteScore(int currentBonusScore, int newBonusScore, int currentScore, int newScore)
    {
        print($"NEW BONUS SCORE WITH TIMER: {currentBonusScore}");
        print($"BONUS SCORE RESET: {newBonusScore}");
        print($"CURRENT SCORE: {currentScore}");
        print($"NEW SCORE WITH BONUS SCORE: {newScore}");
        // Get the duration of the animation
        float animationDuration = 1.75f;

        float timer = 0f;

        // Loop until the timer reaches the animation duration
        while (timer < animationDuration)
        {
            // Calculate the current timer string based on the animation progress
            string currentBonusScoreString = Mathf.FloorToInt(Mathf.Lerp(currentBonusScore, newBonusScore, timer / animationDuration)).ToString();
            StartCoroutine(AnimateText(infiniteBonusText, currentBonusScoreString));

            string currentScoreString = Mathf.FloorToInt(Mathf.Lerp(currentScore, newScore, timer / animationDuration)).ToString();
            StartCoroutine(AnimateText(infiniteScoreText, currentScoreString));

            // Wait for the next frame
            yield return null;

            // Update the timer
            timer += Time.deltaTime;
        }

        infiniteBonusText.text = "0";
        infiniteScoreText.text = newScore.ToString();

        yield return new WaitForSeconds(2f);
    }


    public IEnumerator AnimateLegacyScore(int currentBonusScore, int newBonusScore, int currentScore, int newScore)
    {
        float animationDuration = 1.75f;

        float timer = 0f;

        // Loop until the timer reaches the animation duration
        while (timer < animationDuration)
        {
            // Calculate the current timer string based on the animation progress
            string currentBonusScoreString = Mathf.FloorToInt(Mathf.Lerp(currentBonusScore, newBonusScore, timer / animationDuration)).ToString();
            StartCoroutine(AnimateText(legacyBonusText, currentBonusScoreString));

            string currentScoreString = Mathf.FloorToInt(Mathf.Lerp(currentScore, newScore, timer / animationDuration)).ToString();
            StartCoroutine(AnimateText(legacyScoreText, currentScoreString));

            // Wait for the next frame
            yield return null;

            // Update the timer
            timer += Time.deltaTime;
        }

        legacyBonusText.text = "0";
        legacyScoreText.text = newScore.ToString();

        yield return new WaitForSeconds(2f);
    }





    // ====================================== //
    // ========== EASING FUNCTIONS ========== //
    // ====================================== //

    float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }
    float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }

}
