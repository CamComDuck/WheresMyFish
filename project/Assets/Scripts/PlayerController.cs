using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour {
    // camera control
    private float sensitivity = 1;
    private float camRotationMin = -70;
    private float camRotationMax = 70;

    // mouse input
    private float mouseRotationX;
    private float mouseRotationY;

    [SerializeField] private float moveSpeed = 10;

    [SerializeField] private GameObject[] playerObject;
    [SerializeField] private Transform playerHandPoint;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject itemHoldingCopy;
    [SerializeField] private GameObject enterBoatText;
    [SerializeField] private GameObject exitBoatText;
    private int itemHolding = 0;
    private bool isPicturesPanelVisible = false;
    private bool isInWater = false;
    private bool isCloseToBoat = false;
    private string colliderName = null;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        NextItem();
        gameInput.OnChangeItem += GameInput_OnChangeItem;
        gameInput.OnPicturesUI += GameInput_OnPicturesUI;
        gameInput.OnEnterExitBoat += GameInput_OnEnterExitBoat;
    }

    private void GameInput_OnChangeItem(object sender, System.EventArgs e) {

        Destroy(itemHoldingCopy);
        NextItem();
    }

    private void GameInput_OnPicturesUI(object sender, System.EventArgs e) {
        isPicturesPanelVisible = !isPicturesPanelVisible;
        if (isPicturesPanelVisible) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        
    }

    private void GameInput_OnEnterExitBoat(object sender, System.EventArgs e) {
        if (!isInWater) {
            isInWater = true;
            this.transform.localPosition = new Vector3(0, -5, 0);
            exitBoatText.SetActive(false);
        } else if (isCloseToBoat) {
            isInWater = false;
            this.transform.localPosition = new Vector3(0, 1.4f, 0);
            enterBoatText.SetActive(false);
            exitBoatText.SetActive(true);
        }
    }

    private void NextItem() {

        if (itemHolding == playerObject.Length) itemHolding = 0; // Loop back to start

        GameObject itemInHand = Instantiate (playerObject[itemHolding], playerHandPoint);
        itemInHand.transform.localPosition = Vector3.zero;
        itemHoldingCopy = itemInHand;


        itemInHand = playerObject[itemHolding];
        itemHolding++;
    }



    private void Update() {
        if (!isPicturesPanelVisible) HandleCameraMovement();
        if (isInWater) {
            HandlePlayerMovement();
            CheckBoatLocation();
        }
        
        Debug.Log(colliderName);

    }


    void HandleCameraMovement() {
        mouseRotationX += Input.GetAxis("Mouse X")*sensitivity;
        mouseRotationY += Input.GetAxis("Mouse Y")*sensitivity;

        mouseRotationY = Mathf.Clamp(mouseRotationY, camRotationMin, camRotationMax);

        this.transform.localRotation = Quaternion.Euler(-mouseRotationY, mouseRotationX, 0);
    }

    private void HandlePlayerMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (transform.position.y > -2f) moveDir = new Vector3(0f, -1f, 0f);

        transform.Translate(moveDir * Time.deltaTime * moveSpeed);        

    }

    private void CheckBoatLocation() {
        if (transform.position.x > -5 && transform.position.x < 5 && transform.position.y > -7 && transform.position.z > -5 && transform.position.z < 5) {
            isCloseToBoat = true;
            enterBoatText.SetActive(true);
        } else {
            isCloseToBoat = false;
            enterBoatText.SetActive(false);
        }
    }

    public GameObject GetItemHoldingCopy() {
        return itemHoldingCopy;
    }

    private void OnCollisionStay(Collision other) {
        Debug.Log(other.gameObject.name);
        colliderName = other.gameObject.name;   
    }

    private void OnCollisionExit(Collision other) {
        colliderName = null;
    }
}
