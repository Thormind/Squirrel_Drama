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

    public bool enableAlbedo;
    public bool enableBumpMap;
    public bool enableMetallicSmoothness;

    // Start is called before the first frame update
    void Start()
    {
        enableAlbedo = true;
        enableBumpMap = true;
        enableMetallicSmoothness = true;

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

        DestroyModels();

        index++;
        if (index > (prefabsRef.Length - 1))
        {
            index = 0;
        }

        InstanciateModel();
    }

    private void DestroyModels()
    {
        for (var i = gameObjectVizualizer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gameObjectVizualizer.transform.GetChild(i).gameObject);
        }
    }

    private void InstanciateModel()
    {
        GameObject obj = prefabsRef[index];

        GameObject objectInstaciated = Instantiate(obj);
        objectInstaciated.transform.SetParent(gameObjectVizualizer.transform, false);

        Renderer renderer = objectInstaciated.GetComponent<Renderer>();
        Material[] materials = renderer.materials;

        foreach (Material material in materials)
        {
            material.mainTexture = enableAlbedo ? material.mainTexture : null;

            material.SetTexture("_BumpMap", enableBumpMap ? material.GetTexture("_BumpMap") : null);

            // Set the Metallic and Smoothness properties of the material to 0 to disable them, or to 1 to enable them
            material.SetFloat("_Metallic", enableMetallicSmoothness ? material.GetFloat("_Metallic") : 0f);
            material.SetFloat("_Glossiness", enableMetallicSmoothness ? 1f : 0f);
        }

        // Assign the modified material to the game object's renderer
        renderer.materials = materials;

    }

    void Update()
    {
        gameObjectVizualizer.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);

        if (Input.GetKeyDown(KeyCode.E))
        {
            enableAlbedo = !enableAlbedo;
            DestroyModels();
            InstanciateModel();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            enableBumpMap = !enableBumpMap;
            DestroyModels();
            InstanciateModel();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            enableMetallicSmoothness = !enableMetallicSmoothness;
            DestroyModels();
            InstanciateModel();
        }
    }

}
