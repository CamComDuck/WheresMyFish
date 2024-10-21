using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    [SerializeField] private GameObject lightObject;


    public void ChangeLightState(bool isLightOn) {
        lightObject.SetActive(isLightOn);
    }


}