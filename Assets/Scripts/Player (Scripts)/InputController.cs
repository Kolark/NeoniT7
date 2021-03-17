using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
/// <summary>
/// Clase que funciona para obtener los inputs del jugador a traves de eventos.
/// Unirse el en start a traves del singleton, si se unen en el awake tirara problema
/// </summary>
public class InputController : MonoBehaviour
{
    private static InputController instance;
    public static InputController Instance { get => instance; }
    public ControllerType CurrentControlScheme { get => (ControllerType)Enum.Parse(typeof(ControllerType), playerInput.currentControlScheme); }
    public Action Jump;
    public Action Attack;
    public Action SpecialAbility;
    public Action DefensiveAbility;
    public Action Throw;
    public Action Pause;
    public Action Escape;
    public Action<Vector2> OnMoveEvent;
    public Action<ControllerType> OnControlChanged;
    public Action Delete;
    private Vector2 move;
    /// <summary>
    /// Vector 2 de joystick|WASD
    /// </summary>
    public Vector2 Move { get => move; }

    PlayerInput playerInput;

    private void Awake()
    {
        #region singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        DontDestroyOnLoad(gameObject);
        playerInput = GetComponent<PlayerInput>();
    }
    //Move event
    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
        OnMoveEvent?.Invoke(move);
    }
    //Jump event
    public void OnJump()
    {
        Jump?.Invoke();
    }
    //Attack event
    public void OnAttack()
    {
        Attack?.Invoke();
    }
    //SpecialAbility event
    public void OnSpecialAbility()
    {
        SpecialAbility?.Invoke();
    }
    //Defensive Ability event
    public void OnDefensiveAbility()
    {
        DefensiveAbility?.Invoke();
    }
    //TODELETE event
    public void OnThrowWeapon()
    {
        Throw?.Invoke();
    }
    //pAUSE event
    public void OnPause()
    {
        Pause?.Invoke();
    }
    //Escape event
    public void OnEscape()
    {
        Escape?.Invoke();
    }
    //ControlsChanged event
    public void OnControlsChanged()
    {
        ControllerType type = CurrentControlScheme;
        OnControlChanged?.Invoke(type);
    }
    //Move event
    public void OnDeleteSave()
    {
        Delete?.Invoke();
    }
}
public enum ControllerType
{
    Keyboard,
    XboxController
}