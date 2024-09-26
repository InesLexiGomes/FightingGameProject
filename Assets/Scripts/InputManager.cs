using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControlls playerControlls { get; private set; }

    public Stack<(PlayerControlls, float)> bufferStack { get; private set; }

    private void Awake()
    {
        playerControlls = new PlayerControlls();
        Clear();
    }

    private void OnEnable()
    {
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void Clear()
    {
        bufferStack = new Stack<(PlayerControlls, float)> { };
    }
}
