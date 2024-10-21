using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

    public event EventHandler OnChangeItem, OnUseItem, OnPicturesUI, OnEnterExitBoat;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.ChangeItem.performed += ChangeItem_performed;
        playerInputActions.Player.UseItem.performed += UseItem_performed;
        playerInputActions.Player.PicturesUI.performed += PicturesUI_performed;
        playerInputActions.Player.EnterExitBoat.performed += EnterExitBoat_performed;
    }

    private void ChangeItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnChangeItem?.Invoke(this, EventArgs.Empty);
    }

    private void UseItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnUseItem?.Invoke(this, EventArgs.Empty);
    }

    private void PicturesUI_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPicturesUI?.Invoke(this, EventArgs.Empty);
    }

    private void EnterExitBoat_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnEnterExitBoat?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}