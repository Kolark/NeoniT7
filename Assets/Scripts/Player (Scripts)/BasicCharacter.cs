using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    CharacterMovement character;
    InputController inputController;
    private void Awake()
    {
        character = GetComponent<CharacterMovement>();
        inputController = InputController.Instance;
        inputController.Jump += character.Jump;
    }

    private void FixedUpdate()
    {
        character.Move();
        character.Crouch();
    }
}
