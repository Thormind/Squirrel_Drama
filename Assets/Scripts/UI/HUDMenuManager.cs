using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GAME_DATA
{
    SCORE,
    BONUS_SCORE,
    BEST_SCORE,
    LIFE,
    LEVEL,
    TIMER,
    MAP,
    CURRENT_MULTIPLIER,
    MULTIPLIER_STREAK,
    MULTIPLIER_TIME,
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
    public Slider infiniteMapSlider;

    public Slider infiniteMultiplierSlider;
    public TMP_Text infiniteMultiplierText;

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

    private void OnEnable()
    {
        GlobalUIManager.instance.specificMenu = MENU.MENU_PAUSE;

        if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
        {
            SetInfiniteHUD();
        }
        if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
        {
            SetLegacyHUD();
        }
    }

    public void SetInfiniteHUD()
    {
        legacyPanel.SetActive(false);
        infinitePanel.SetActive(true);
        infiniteMapSlider.transform.parent.gameObject.SetActive(true);
        //InfiniteGameController.instance.UpdateHUD();
        UpdateInfiniteHUD(GAME_DATA.ALL);
    }

    public void SetLegacyHUD()
    {
        infinitePanel.SetActive(false);
        legacyPanel.SetActive(true);
        infiniteMapSlider.transform.parent.gameObject.SetActive(false);
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
            case GAME_DATA.MAP:
                StartCoroutine(AnimateMapSlider(infiniteMapSlider, InfiniteGameController.instance.GetFruitHeightForMap()));
                break;
            case GAME_DATA.CURRENT_MULTIPLIER:
                string currentMultiplier = "x" + InfiniteGameController.instance.GetCurrentMultiplier().ToString();
                StartCoroutine(AnimateText(infiniteMultiplierText, currentMultiplier));
                break;
            case GAME_DATA.MULTIPLIER_TIME:
                StartCoroutine(AnimateMultiplierSlider(infiniteMultiplierSlider, InfiniteGameController.instance.GetMultiplierTimeLeft()));
                break;
            case GAME_DATA.MULTIPLIER_STREAK:
                StartCoroutine(AnimateMultiplierSlider(infiniteMultiplierSlider, InfiniteGameController.instance.GetMultiplierTimeLeft()));
                //infiniteMultiplierSlider.value = InfiniteGameController.instance.GetCurrentMultiplierStreak();
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


    // =========================================== //
    // ========== MAP SLIDER ANIMATIONS ========== //
    // =========================================== //

    private IEnumerator AnimateMapSlider(Slider mapSlider, float fruitHeight)
    {
        float sliderValue = fruitHeight / 40f; // assuming max height is 40
        sliderValue = Mathf.Clamp01(sliderValue); // clamp the value between 0 and 1

        float elapsedTime = 0f;
        float startValue = mapSlider.value;
        float duration = 0.5f; // adjust as needed

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            mapSlider.value = Mathf.Lerp(startValue, sliderValue, t);
            yield return null;
        }

        mapSlider.value = sliderValue; // ensure that the slider ends up with the exact target value
    }

    private IEnumerator AnimateMultiplierSlider(Slider multiplierSlider, float multiplierTimeStreak)
    {
        float startValue = multiplierSlider.value; // clamp the value between 0 and 1
        float endValue = multiplierTimeStreak;

        float elapsedTime = 0f;
        float duration = 0.2f; // adjust as needed

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            multiplierSlider.value = Mathf.Lerp(startValue, endValue, t);
            yield return null;
        }

        multiplierSlider.value = multiplierTimeStreak; // ensure that the slider ends up with the exact target value
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

    public IEnumerator AnimateInfiniteTimerReset()
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

        StartCoroutine(AnimateText(infiniteTimerText, resetTimerString));

        yield return new WaitForSeconds(2f);

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
        // Get the duration of the animation
        float animationDuration = 1.75f;

        float timer = 0f;

        string startTimerString = infiniteTimerText.text;
        string resetTimerString = "00:00:00";

        AudioManager.instance.PauseMusic();
        AudioManager.instance.PlaySound(SOUND.SCORE_BONUS_1);

        int i = 0;
        // Loop until the timer reaches the animation duration
        while (timer < animationDuration && gameObject.activeInHierarchy)
        {
            // Calculate the current timer string based on the animation progress
            string currentTimerString = GetAnimatedTimerString(startTimerString, resetTimerString, timer / animationDuration);
            StartCoroutine(AnimateText(infiniteTimerText, currentTimerString));

            string currentBonusScoreString = Mathf.FloorToInt(Mathf.Lerp(currentBonusScore, newBonusScore, timer / animationDuration)).ToString();
            StartCoroutine(AnimateText(infiniteBonusText, currentBonusScoreString));

            UpdateInfiniteHUD(GAME_DATA.CURRENT_MULTIPLIER);

            // Wait for the next frame
            yield return null;

            // Update the timer
            timer += Time.deltaTime;
        }

        StartCoroutine(AnimateText(infiniteBonusText, newBonusScore.ToString()));
        StartCoroutine(AnimateText(infiniteTimerText, resetTimerString));

        yield return new WaitForSeconds(2f);

        infiniteBonusText.text = newBonusScore.ToString();

        // Set the final timer string to "00:00:00"
        infiniteTimerText.text = resetTimerString;
    }

    public IEnumerator AnimateInfiniteScore(int currentBonusScore, int newBonusScore, int currentScore, int newScore)
    {
        // Get the duration of the animation
        float animationDuration = 1.75f;

        float timer = 0f;

        AudioManager.instance.PlaySound(SOUND.SCORE_BONUS_2);

        // Loop until the timer reaches the animation duration
        while (timer < animationDuration && gameObject.activeInHierarchy)
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

        StartCoroutine(AnimateText(infiniteBonusText, "0"));
        StartCoroutine(AnimateText(infiniteScoreText, newScore.ToString()));

        yield return new WaitForSeconds(2f);

        infiniteBonusText.text = "0";
        infiniteScoreText.text = newScore.ToString();
        AudioManager.instance.AdjustMusic();
    }


    public IEnumerator AnimateLegacyScore(int currentBonusScore, int newBonusScore, int currentScore, int newScore)
    {
        float animationDuration = 1.75f;

        float timer = 0f;

        // Loop until the timer reaches the animation duration
        while (timer < animationDuration && gameObject.activeInHierarchy)
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

        StartCoroutine(AnimateText(legacyBonusText, "0"));
        StartCoroutine(AnimateText(legacyScoreText, newScore.ToString()));

        yield return new WaitForSeconds(2f);

        legacyBonusText.text = "0";
        legacyScoreText.text = newScore.ToString();
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
