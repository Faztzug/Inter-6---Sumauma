using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public enum FlipDirection
    {
        Right,
        Left,
    }
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private RectTransform leftPageHolder;
    [SerializeField] private RectTransform rightPageHolder;
    [SerializeField] private RectTransform transitionPageHolder;
    private GameObject currentLeftPageGO;
    private GameObject currentRightPageGO;
    private int[] currentPages = new int[2]{0,0};

    private void Start() 
    {
        currentPages = new int[2]{0,1};
        InstantiatePages();
    }

    private void Update() 
    {
        var input = Input.GetAxis("Horizontal");
        Debug.Log(input);
        if(!GameState.isGamePaused) return;
        if(input == 1) FlipPages(FlipDirection.Right);
        else if(input == -1) FlipPages(FlipDirection.Left);
    }

    private void FlipPages(FlipDirection direction)
    {
        Debug.Log("FLIP PAGE: " + direction);
        var newPages = new int[2]{currentPages[0],currentPages[1]};
        if(direction == FlipDirection.Right)
        {
            newPages[0] += 2;
            newPages[1] += 2;
        }
        else if(direction == FlipDirection.Right)
        {
            newPages[0] -= 2;
            newPages[1] -= 2;
        }
        if(newPages == currentPages) return;

        if(newPages[0] >= 0 || newPages[0] < pages.Count)
        {
            currentPages = new int[2]{newPages[0],newPages[1]};
            CleanPages();
        }
    }

    private void CleanPages()
    {
        if(currentLeftPageGO) Destroy(currentLeftPageGO);
        if(currentRightPageGO) Destroy(currentRightPageGO);
    }

    private void InstantiatePages()
    {
        if(currentPages[0] >= 0 || currentPages[0] < pages.Count) 
        {
            currentLeftPageGO = GameObject.Instantiate(pages[0], leftPageHolder, false);
            currentLeftPageGO.SetActive(true);
        }

        if(currentPages[1] >= 0 || currentPages[1] < pages.Count)
        {
            currentRightPageGO = GameObject.Instantiate(pages[1], rightPageHolder, false);
            currentRightPageGO.SetActive(true);
        }
    }
}
