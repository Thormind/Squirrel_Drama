using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteFruitAnimation : MonoBehaviour
{

    private float rotationSpeed = 25f;

    private float movementSpeed = 0.75f;

    private float maxY = 1.5f;
    private Vector3 endOfLifePosition;

    public GameObject ShinyVFX;
    public GameObject endOfLifeVFX;

    // Start is called before the first frame update
    void Start()
    {
        maxY = transform.localPosition.y + maxY;
        endOfLifePosition = new Vector3(transform.localPosition.x, maxY, transform.localPosition.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * Time.fixedDeltaTime * rotationSpeed, Space.World);
    }

    public void HandleFruitInFruitFunction()
    {
        StartCoroutine(HandleFruitInFruit());
    }

    IEnumerator HandleFruitInFruit()
    {
        rotationSpeed = 600f;

        ShinyVFX.SetActive(false);

        bool soundPlayed = false;

        while (transform.localPosition.y < endOfLifePosition.y - 0.2f)
        {
            if (!soundPlayed)
            {
                AudioManager.instance.PlaySound(SOUND.LIFE_SPIN);
                soundPlayed = true;
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, endOfLifePosition, Time.fixedDeltaTime * movementSpeed);

            yield return new WaitForEndOfFrame();
        }

        AudioManager.instance.PlaySound(SOUND.LIFE_POP);
        

        yield return new WaitForSecondsRealtime(0.2f);

        GameObject vfx = Instantiate(endOfLifeVFX, transform.localPosition, Quaternion.identity);
        vfx.transform.parent = transform.parent;
        vfx.transform.localPosition = transform.localPosition;
        vfx.GetComponent<ParticleSystem>().Play();

        Destroy(gameObject);
    }
}
