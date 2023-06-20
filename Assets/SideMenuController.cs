using System;
using DG.Tweening;
using UnityEngine;
public class SideMenuController : MonoBehaviour{

    // The different sides the menu can slide away into
    public enum MenuSide{
        Left,
        Right,
        Top,
        Bottom
    }
    
    // Does the menu start opened?
    public bool StartOpen = true;
    // How long our tween should last
    [Range(.1f, 2)] public float TweenDuration = .25f;
    // Which tween type to use
    public Ease TweenType = Ease.OutQuad;
    // The side to slide away into
    public MenuSide MenuSlideSide = MenuSide.Left;
    
    // Is the menu currently open?
    private bool menuIsOpen;
    // The parameters for the tweener
    private TweenParams tweenParams;
    // Our attached RectTransform
    private RectTransform rectTransform;
    
    // Initialise our variables
    private void Start(){
        tweenParams = TweenParams.Params.SetEase(TweenType);
        rectTransform = GetComponent<RectTransform>();
        menuIsOpen = StartOpen;
    }
    
    /// <summary>
    /// Change the open state of the menu by setting it to openState.
    /// </summary>
    /// <param name="openState">The new open state of the menu.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when MenuSlideSide is not a valid MenuSide.
    /// </exception>
    public void SetMenuOpenState(bool openState){
        // Is our menu already of the desired state? If so, we have nothing else to do.
        if (openState == menuIsOpen) return;
        
        // Assign variables for easy access
        Rect rect = rectTransform.rect;
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        
        // Assign the new position based on what side we want to slide away into
        Vector2 newPosition = MenuSlideSide switch{
            MenuSide.Left => new Vector2(openState ? 0 : -rect.width, anchoredPosition.y),
            MenuSide.Right => new Vector2(openState ? 0 : rect.width, anchoredPosition.y),
            MenuSide.Top => new Vector2(anchoredPosition.x, openState ? 0 : rect.height),
            MenuSide.Bottom => new Vector2(anchoredPosition.x, openState ? 0 : -rect.height),
            _ => throw new InvalidOperationException("Invalid MenuSide.")
        };
        
        // Tween to the new position
        TweenToPosition(newPosition);
        // Update our open state
        menuIsOpen = openState;
    }
    
    // Public getter for the open state of the menu
    public bool GetMenuOpenState() => menuIsOpen;
    
    // Easily toggle the open state of the menu
    public void ToggleOpenState(){
        SetMenuOpenState(!GetMenuOpenState());
    }
    
    /// <summary>
    /// This method will tween the current GameObject using the tween parameters of the class by adjusting its RectTransform.anchoredPosition.
    /// </summary>
    /// <param name="newAnchoredPosition">The new anchoredPosition value to tween to.</param>
    private void TweenToPosition(Vector2 newAnchoredPosition){
        // We call DOTween.To and pass in the corresponding variables
        DOTween.To(
            () => rectTransform.anchoredPosition,
            x => rectTransform.anchoredPosition = x,
            newAnchoredPosition,
            TweenDuration
        ).SetAs(tweenParams);
    }
}