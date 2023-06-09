using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteFruit : MonoBehaviour
{
    private float fruitFallingGravityScale;
    private float fruitGravityScale;
    private float MinCollisionDistance;

    private float enterTheHoleTime = 1f;
    private float fallingFromTreeTime = 0.1f;
    private float crushedTime = 1f;

    private float fruitToSquirrelRotateSpeed = 200f;
    private float fruitToSquirrelTranslateSpeed = 2f;

    public bool collisionEnabled;

    public Rigidbody2D fruitRigidbody;

    private Vector3 startFruitScale;

    public Transform fruitStackPosition;
    public Transform fruitBottomPosition;
    public Transform fruitStartPosition;
    public Transform fruitSquirrelPosition;


    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startFruitScale = transform.localScale;

        transform.position = fruitStackPosition.position;

        collisionEnabled = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.transform.gameObject.tag == "Hole"
            && Vector2.Distance(collision.transform.localPosition, fruitRigidbody.transform.localPosition) <= MinCollisionDistance
            && collisionEnabled)
        {
            collision.isTrigger = false;

            InfiniteGameController.instance.HandleFruitInHole();
            StartCoroutine(MoveToHoleCoroutine(collision));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionEnabled)
        {
            bool collisionIsNotProximity = false; 

            if (collision.transform.gameObject.tag == "Bee")
            {
                collisionIsNotProximity = true;

                InfiniteGameController.instance.HandleFruitInBee();

                StartCoroutine(FallFromTreeCoroutine(collision.transform));
            }

            if (collision.transform.gameObject.tag == "Worm")
            {
                collisionIsNotProximity = true;

                InfiniteGameController.instance.HandleFruitInWorm();

                StartCoroutine(FallFromTreeCoroutine(collision.transform));
            }

            if (collision.transform.gameObject.tag == "Bear")
            {
                collisionIsNotProximity = true;

                InfiniteGameController.instance.HandleFruitInBear();

                StartCoroutine(CrushedCoroutine(collision.transform));
            }

            if (collision.transform.gameObject.tag == "Points")
            {
                collisionIsNotProximity = true;
                collision.enabled = false;

                collision.transform.gameObject.GetComponent<InfinitePointsAnimation>().HandleFruitInPointsFunction();
                
                InfiniteGameController.instance.HandleFruitInPoints();
            }

            if (collision.transform.gameObject.tag == "Fruit")
            {
                collisionIsNotProximity = true;
                collision.enabled = false;

                collision.transform.gameObject.GetComponent<InfiniteFruitAnimation>().HandleFruitInFruitFunction();
                
                InfiniteGameController.instance.HandleFruitInFruit();
            }

            if (collision.transform.gameObject.tag == "FallingZone")
            {
                collisionIsNotProximity = true;
                InfiniteGameController.instance.HandleFruitFalling();

                StartCoroutine(FallFromTreeCoroutine(collision.transform));
            }

            if (!collisionIsNotProximity && collision.transform.gameObject.tag == "ObstacleProximity")
            {
                collision.enabled = false;
                AnimationManager.instance.PlayAngrySquirrelFaceAnimation();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "ObstacleProximity" && !collision.enabled)
        {
            collision.enabled = true;
        }
    }

    public IEnumerator MoveToSquirrelCoroutine()
    {
        fruitRigidbody.simulated = false;

        Vector3 fruitPosition = transform.localPosition;
        Vector3 squirrelPosition = new Vector3(fruitSquirrelPosition.transform.localPosition.x,
            fruitSquirrelPosition.transform.localPosition.y,
            fruitSquirrelPosition.transform.localPosition.z + 2f);

        bool isFruitAtSquirrel = false;
        bool isFruitStartLeftOfSquirrel = fruitPosition.x < fruitSquirrelPosition.localPosition.x;

        Vector3 positionOffset = new Vector3((isFruitStartLeftOfSquirrel ? 1 : -1), 0, 0);
        float zRotationOffset = isFruitStartLeftOfSquirrel ? -1 : 1;

        while (!isFruitAtSquirrel)
        {
            fruitPosition = transform.localPosition;

            Vector3 newPosition = transform.localPosition + positionOffset * fruitToSquirrelTranslateSpeed * Time.deltaTime;

            if ((isFruitStartLeftOfSquirrel && newPosition.x >= squirrelPosition.x)
                || (!isFruitStartLeftOfSquirrel && newPosition.x <= squirrelPosition.x))
            {
                newPosition.x = squirrelPosition.x;
                isFruitAtSquirrel = true;
            }

            transform.localPosition = newPosition;
            transform.Rotate(0, 0, zRotationOffset * fruitToSquirrelRotateSpeed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator MoveToSquirrelLoveCoroutine()
    {
        transform.localPosition = 
            new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.3f);

        yield return null;
    }

    public IEnumerator AfterSquirrelLoveCoroutine()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 5f);

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = fruitFallingGravityScale;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

        yield return null;
    }

    IEnumerator MoveToHoleCoroutine(Collider2D holeTransform)
    {
        fruitRigidbody.simulated = false;
        AudioManager.instance.PlaySound(SOUND.FRUIT_HOLE);

        float t = 0;
        Vector3 fruitPosition = transform.localPosition;
        Vector3 holePosition = new Vector3(holeTransform.transform.localPosition.x, holeTransform.transform.localPosition.y, holeTransform.transform.localPosition.z + 2f);

        while (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(fruitPosition, holePosition, t);
            transform.localScale = startFruitScale * Mathf.Lerp(1, 0.75f, t);

            t += Time.deltaTime / enterTheHoleTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = holePosition;

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = fruitFallingGravityScale;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

        if (holeTransform != null)
        {
            holeTransform.isTrigger = true;
        }
    }

    IEnumerator FallFromTreeCoroutine(Transform obstacleTransform)
    {
        fruitRigidbody.simulated = false;

        AudioManager.instance.PlaySound(SOUND.FRUIT_FALL);

        float t = 0;
        Vector3 fruitTargetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1f);
        Vector3 fruitPosition = transform.localPosition;

        while (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(fruitPosition, fruitTargetPosition, t);

            t += Time.deltaTime / fallingFromTreeTime;

            yield return new WaitForEndOfFrame();
        }

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = fruitFallingGravityScale;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

    }

    IEnumerator CrushedCoroutine(Transform bearTransform)
    {
        fruitRigidbody.simulated = false;

        AudioManager.instance.PlaySound(SOUND.FRUIT_SQUASH);

        float t = 0;
        Vector3 fruitTargetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 5f);
        Vector3 fruitPosition = transform.localPosition;

        while (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(fruitPosition, fruitTargetPosition, t);

            t += Time.deltaTime / crushedTime;

            yield return new WaitForEndOfFrame();
        }

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = fruitFallingGravityScale;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

    }

    public void ResetFruitPosition()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = false;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;
        transform.position = fruitStackPosition.position;
    }

    public IEnumerator AnimateFruitReset()
    {

        fruitRigidbody.simulated = true;
        transform.position = fruitStackPosition.position;
        transform.localScale = startFruitScale;

        float t = InfiniteGameController.instance.ElevatorDistanceBetweenStartBottom();

        Vector3 startPosition = fruitStackPosition.position;
        Vector3 endPosition = fruitStartPosition.position;

        float distance = Vector3.Distance(startPosition, endPosition);
        float height = distance * 0.25f; // You can adjust the height as desired

        while (t < 1f)
        {
            float easedProgress = EaseOutQuart(t);

            // Calculate the position based on projectile motion
            float x = Mathf.Lerp(startPosition.x, endPosition.x, easedProgress);
            float y = endPosition.y + height * Mathf.Sin(Mathf.Lerp(0f, Mathf.PI, easedProgress));
            float z = Mathf.Lerp(startPosition.z, endPosition.z, easedProgress);
            transform.position = new Vector3(x, y, z);

            t = InfiniteGameController.instance.ElevatorDistanceBetweenStartBottom();

            yield return null;
        }

        transform.position = endPosition;

        GetComponent<CircleCollider2D>().enabled = true;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;
        fruitRigidbody.gravityScale = fruitGravityScale;
    }

    public void QuickResetFruitPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void SetFruitGravityScale(float gravityScale)
    {
        fruitGravityScale = gravityScale;
        fruitRigidbody.gravityScale = gravityScale;
    }

    public void SetFruitFallingGravityScale(float gravityScale)
    {
        fruitFallingGravityScale = gravityScale;
    }

    public void SetFruitMinCollisionDistance(float minCollDis)
    {
        MinCollisionDistance = minCollDis;
    }

    public void SetFruitActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }


    // ====================================== //
    // ========== EASING FUNCTIONS ========== //
    // ====================================== //

    float EaseInQuart(float t)
    {
        return t * t * t * t;
    }

    float EaseOutQuart(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }
}
