using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;

public class Book : MonoBehaviour
{
    public enum FlipDirection
    {
        Right,
        Left,
    }
    public enum WichPage
    {
        Right,
        Left,
        Both,
    }
    [SerializeField] private List<GameObject> pages;
    [SerializeField] private RectTransform leftPageHolder;
    [SerializeField] private RectTransform rightPageHolder;
    [SerializeField] private RectTransform transitionPages;
    [SerializeField] private RectTransform transitionPagesRight;
    [SerializeField] private RectTransform transitionPagesLeft;
    private GameObject currentLeftPageGO;
    private GameObject currentRightPageGO;
    private GameObject currentTransitionLeftPageGO;
    private GameObject currentTransitionRightPageGO;
    private int[] currentPages = new int[2]{0,0};
    private bool inTransition;
    [SerializeField] float rotationTime = 1f;

    private void Start() 
    {
        ResetPagesToStart();
    }

    public void ResetPagesToStart()
    {
        currentPages = new int[2]{0,1};
        InstantiatePage(WichPage.Both);
    }

    private void Update() 
    {
        if(!GameState.isGamePaused || inTransition) return;
        if(Input.GetButtonDown("Right")) FlipPages(FlipDirection.Right);
        else if(Input.GetButtonDown("Left")) FlipPages(FlipDirection.Left);
    }

    public async void FlipPages(FlipDirection direction)
    {
        var newPages = new int[2]{currentPages[0],currentPages[1]};
        if(direction == FlipDirection.Right)
        {
            newPages[0] += 2;
            newPages[1] += 2;
        }
        else if(direction == FlipDirection.Left)
        {
            newPages[0] -= 2;
            newPages[1] -= 2;
        }
        if(newPages == currentPages) return;

        if(newPages[0] >= 0 && newPages[0] < pages.Count)
        {
            currentPages = new int[2]{newPages[0],newPages[1]};
            await FlipingTransition(direction);
        }
        Debug.Log("FLIP PAGE: " + direction + "  " + currentPages[0].ToString() + " " +  currentPages[1].ToString());
    }

    // private void CleanPages()
    // {
    //     if(currentLeftPageGO) Destroy(currentLeftPageGO);
    //     if(currentRightPageGO) Destroy(currentRightPageGO);
    // }

    private async Task FlipingTransition(FlipDirection direction)
    {
        inTransition = true;

        transitionPages.gameObject.SetActive(true);

        if(currentTransitionLeftPageGO) Destroy(currentTransitionLeftPageGO);
        if(currentTransitionRightPageGO) Destroy(currentTransitionRightPageGO);

        var rotateTo = Vector3.zero;
        WichPage firstPage = WichPage.Both;
        if(direction == FlipDirection.Right)
        {
            transitionPages.localRotation = Quaternion.identity;
            rotateTo.y = 180;
            currentTransitionRightPageGO = GameObject.Instantiate(currentRightPageGO, transitionPagesRight, false);
            if(pages[currentPages[0]]) 
            currentTransitionLeftPageGO = GameObject.Instantiate(pages[currentPages[0]], transitionPagesLeft, false);
            firstPage = WichPage.Right;
            transitionPagesRight.parent.SetAsLastSibling();
        }
        else if(direction == FlipDirection.Left)
        {
            transitionPages.localRotation = new Quaternion(0, 180, 0, 0);
            rotateTo.y = 0;
            if(pages[currentPages[1]]) 
            currentTransitionRightPageGO= GameObject.Instantiate(pages[currentPages[1]], transitionPagesRight, false);
            currentTransitionLeftPageGO = GameObject.Instantiate(currentLeftPageGO, transitionPagesLeft, false);
            firstPage = WichPage.Left;
            transitionPagesLeft.parent.SetAsLastSibling();
        }

        
        //CleanPages();
        InstantiatePage(firstPage);

        currentTransitionRightPageGO.SetActive(true);
        currentTransitionLeftPageGO.SetActive(true);

        var middleRot = new Vector3(0,90,0);
        await transitionPages.DORotate(middleRot, rotationTime/2f, RotateMode.Fast).SetUpdate(true)
        .SetEase(Ease.InCirc)
        .OnComplete(() => {transitionPages.GetChild(0).SetAsLastSibling(); 
        InstantiatePage(WichPage.Both);})
        .AsyncWaitForCompletion();

        await transitionPages.DORotate(rotateTo, rotationTime/2f, RotateMode.Fast).SetUpdate(true)
        .SetEase(Ease.OutCirc).AsyncWaitForCompletion();

        inTransition = false;
        transitionPages.gameObject.SetActive(false);
    }

    private void InstantiatePage(WichPage wichPage)
    {
        if(wichPage == WichPage.Left || wichPage == WichPage.Both)
        {
            if(currentLeftPageGO) Destroy(currentLeftPageGO);
            if(currentPages[0] >= 0 && currentPages[0] < pages.Count) 
            {
                currentLeftPageGO = GameObject.Instantiate(pages[currentPages[0]], leftPageHolder, false);
                currentLeftPageGO.SetActive(true);
            }
        }

        if(wichPage == WichPage.Right || wichPage == WichPage.Both)
        {
            if(currentRightPageGO) Destroy(currentRightPageGO);
            if(currentPages[1] >= 0 && currentPages[1] < pages.Count) 
            {
                currentRightPageGO = GameObject.Instantiate(pages[currentPages[1]], rightPageHolder, false);
                currentRightPageGO.SetActive(true);
            }
        }
    }
}
