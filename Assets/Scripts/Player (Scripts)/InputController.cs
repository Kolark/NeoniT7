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
    /// <summary>
    /// Evento Salto
    /// </summary>
    public Action Jump;
    /// <summary>
    /// Evento Ataque
    /// </summary>
    public Action Attack;

    private Vector2 move;
    /// <summary>
    /// Vector 2 de joystick|WASD
    /// </summary>
    public Vector2 Move { get => move;}

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump()
    {
        Jump?.Invoke();
    }

    public void OnAttack()
    {
        Attack?.Invoke();
    }
}
