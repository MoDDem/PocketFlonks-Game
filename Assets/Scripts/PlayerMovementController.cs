using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    Vector2 movementInput;

    public void OnMovement(InputValue value) {
        movementInput = value.Get<Vector2>();
    }

    void FixedUpdate() {
        SendMovement();
    }

    private void SendMovement() {
        ClientSend.PlayerMovement(gameObject.GetComponent<Player>().id, movementInput);
    }
}
