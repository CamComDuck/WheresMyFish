using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class HandleBattery : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject sliderUI;
    private GameObject itemHoldingCopy;
    private bool isLightOn = false;

    private void Start() {
        gameInput.OnUseItem += GameInput_OnUseItem;
    }

    private void GameInput_OnUseItem(object sender, System.EventArgs e) {
        itemHoldingCopy = playerController.GetItemHoldingCopy();
        if (itemHoldingCopy.name == "Flashlight(Clone)") {
            isLightOn = !isLightOn;
            itemHoldingCopy.GetComponent<Flashlight>().ChangeLightState(isLightOn);
        } 
    }

}