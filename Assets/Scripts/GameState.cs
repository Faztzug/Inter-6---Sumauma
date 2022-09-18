using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameState : MonoBehaviour
{
    static public MainCanvas mainCanvas;
    static public CinemachineFreeLook cinemachineFreeLook;
    public Transform playerTransform;
    static public Transform PlayerTransform => GameStateInstance.playerTransform;
    public bool isPlayerDead = false;
    public Transform playerLookAt;
    private static GameState gameState;
    private GameState() { }

    public static GameState GameStateInstance => gameState;

    private void Awake()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvas>();
        cinemachineFreeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
        cinemachineFreeLook.Follow = playerTransform;
        cinemachineFreeLook.LookAt = playerLookAt;
        gameState = this;
    }
}
