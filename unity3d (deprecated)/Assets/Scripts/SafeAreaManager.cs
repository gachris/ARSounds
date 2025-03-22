/*===============================================================================
Copyright (c) 2018 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using UnityEngine;
using UnityEngine.UI;

public class SafeAreaManager : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    [System.Serializable]
    private class SafeAreaRect
    {
        public RectTransform rectTransform = null;
        public bool applyTopSafeArea = false;
        public bool applyBottomSafeArea = false;
    }

    [SerializeField] private RectTransform topArea = null;
    [SerializeField] private RectTransform bottomArea = null;
    [SerializeField] private Color topAreaColor;
    [SerializeField] private Color bottomAreaColor;
    [SerializeField] private SafeAreaRect[] safeAreaRects = null;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);
    private ScreenOrientation lastOrientation;

    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS

    private void Awake()
    {
        SetAreaColors();
    }

    private void Start()
    {
        lastOrientation = Screen.orientation;

        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    private void Refresh()
    {
        if (Screen.safeArea != lastSafeArea)
        {
            ApplySafeArea();
            UpdateNonSafeArea();
        }

        if (Screen.orientation != lastOrientation)
        {
            ApplySafeArea();
            UpdateNonSafeArea();
        }
    }

    private void ApplySafeArea()
    {
        lastSafeArea = Screen.safeArea;
        lastOrientation = Screen.orientation;

        foreach (SafeAreaRect areaRect in safeAreaRects)
        {
            Vector2 anchorMin = Screen.safeArea.position;
            Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y = areaRect.applyBottomSafeArea ? anchorMin.y / Screen.height : 0;
            anchorMax.x /= Screen.width;
            anchorMax.y = areaRect.applyTopSafeArea ? anchorMax.y / Screen.height : 1;

            areaRect.rectTransform.anchorMin = anchorMin;
            areaRect.rectTransform.anchorMax = anchorMax;
        }
    }

    private void UpdateNonSafeArea()
    {
        Vector2 anchorMin = Screen.safeArea.position;
        Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y = anchorMin.y / Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y = anchorMax.y / Screen.height;

        SetNonSafeAreaSizes(anchorMin.y, anchorMax.y);
    }

    private void SetNonSafeAreaSizes(float safeAreaAnchorMinY, float safeAreaAnchorMaxY)
    {
        topArea.anchorMin = new Vector2(0, safeAreaAnchorMaxY);
        topArea.anchorMax = Vector2.one;

        bottomArea.anchorMin = Vector2.zero;
        bottomArea.anchorMax = new Vector2(1, safeAreaAnchorMinY);
    }

    private void SetAreaColors()
    {
        topArea.GetComponent<Image>().color = topAreaColor;
        bottomArea.GetComponent<Image>().color = bottomAreaColor;
    }

    #endregion // PRIVATE_METHODS


    #region PUBLIC_METHODS

    public void SetAreasEnabled(bool topAreaEnabled, bool bottomAreaEnabled)
    {
        topArea.gameObject.SetActive(topAreaEnabled);
        bottomArea.gameObject.SetActive(bottomAreaEnabled);
    }

    /// <summary>
    /// Sets the area colors programmatically and bypasses Inspector colors.
    /// </summary>
    /// <param name="topColor">Top color.</param>
    /// <param name="bottomColor">Bottom color.</param>
    public void SetAreaColors(Color topColor, Color bottomColor)
    {
        topAreaColor = topColor;
        bottomAreaColor = bottomColor;

        topArea.GetComponent<Image>().color = topAreaColor;
        bottomArea.GetComponent<Image>().color = bottomAreaColor;
    }

    #endregion // PUBLIC_METHODS
}
