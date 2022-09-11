using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameState : MonoBehaviour
{
  static public MainCanvas mainCanvas;
  static public CinemachineFreeLook cinemachineFreeLook;
  public Transform playerTransform;
  public Transform playerLookAt;
  private static GameState gameState;
  private GameState() {}

  public static GameState GameStateInstance
  {
    get 
    {
      if (gameState == null)
      {
          gameState = new GameState();
      }
      return gameState;
    }
  }

  private void Awake() 
  {
    mainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MainCanvas>();
    cinemachineFreeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
    cinemachineFreeLook.Follow = playerTransform;
    cinemachineFreeLook.LookAt = playerLookAt;
  }
}
