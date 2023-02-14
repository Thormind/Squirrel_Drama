using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProceduralGenerationUIController : MonoBehaviour
{

    public TMP_Text HolesQuantityTxt;
    public Slider HolesQuantitySlider;
    public Button HolesGenerateButton;
    public Button HolesClearButton;


    public TMP_Text BeesQuantityTxt;
    public Slider BeesQuantitySlider;
    public Button BeesGenerateButton;
    public Button BeesClearButton;

    // Start is called before the first frame update
    void Start()
    {
        HolesQuantitySlider.onValueChanged.AddListener(delegate { HandleHolesQuantity(HolesQuantitySlider.value); });
        BeesQuantitySlider.onValueChanged.AddListener(delegate { HandleBeesQuantity(BeesQuantitySlider.value); });

        HolesGenerateButton.onClick.AddListener(() => HandleHolesGeneration());
        HolesClearButton.onClick.AddListener(() => HandleHolesClear());

        BeesGenerateButton.onClick.AddListener(() => HandleBeesGeneration());
        BeesClearButton.onClick.AddListener(() => HandleBeesClear());

        HolesQuantitySlider.value = 80;
        HandleHolesQuantity(HolesQuantitySlider.value);
        BeesQuantitySlider.value = 10;
        HandleBeesQuantity(BeesQuantitySlider.value);
    }

    public void HandleHolesQuantity(float newQuantity)
    {
        HoleController.instance.holesQuantity = (int) newQuantity;
        HolesQuantityTxt.text = newQuantity.ToString();
    }

    public void HandleBeesQuantity(float newQuantity)
    {
        BeesController.instance.beesQuantity = (int) newQuantity;
        BeesQuantityTxt.text = newQuantity.ToString();
    }

    public void HandleHolesGeneration()
    {
        HoleController.instance.SpawnHoles();
    }

    public void HandleHolesClear()
    {
        HoleController.instance.RemoveHoles();
    }

    public void HandleBeesGeneration()
    {
        BeesController.instance.SpawnBees();
    }

    public void HandleBeesClear()
    {
        BeesController.instance.RemoveBees();
    }

}
