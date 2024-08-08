using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private GameInput input;

    private InputAction move;

    private void Awake()
    {
        input = new GameInput();
        move = input.Player.Move;
    }

    private void OnEnable()
    {
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    void Update()
    {
        float MoveX = move.ReadValue<Vector2>().x;
        float MoveY = move.ReadValue<Vector2>().y;

        
    }

}
