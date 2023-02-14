using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModelController : MonoBehaviour
{

    [SerializeField] private GameObject[] prefabsRef;

    [SerializeField] private GameObject gameObjectVizualizer;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private int index = 0;

    public float rotationSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        previousButton.onClick.AddListener(() => HandlePreviousModelAction());
        nextButton.onClick.AddListener(() => HandleNextModelAction());

        HandleNextModelAction();
    }

    private void HandlePreviousModelAction()
    {
        for (var i = gameObjectVizualizer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gameObjectVizualizer.transform.GetChild(i).gameObject);
        }

        index--;
        if (index < 0)
        {
            index = (prefabsRef.Length - 1);
        }

        GameObject obj = prefabsRef[index];

        GameObject objectInstaciated = Instantiate(obj);
        objectInstaciated.transform.SetParent(gameObjectVizualizer.transform, false);
    }
    private void HandleNextModelAction()
    {
        for (var i = gameObjectVizualizer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gameObjectVizualizer.transform.GetChild(i).gameObject);
        }

        index++;
        if (index > (prefabsRef.Length - 1))
        {
            index = 0;
        }

        GameObject obj = prefabsRef[index];

        GameObject objectInstaciated = Instantiate(obj);
        objectInstaciated.transform.SetParent(gameObjectVizualizer.transform, false);
    }

    void Update()
    {
        gameObjectVizualizer.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
    }

}
