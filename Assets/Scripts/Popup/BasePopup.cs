using System.Linq;
using System;
using TMPro;
using UnityEngine;

public abstract class BasePopup<TData, TResult> : KienBehaviour, IPopup
    where TData : IPopupData
{
    [Header("Base Popup UI")]
    [SerializeField] private bool closeWhenClickOkBtn = true;
    [SerializeField] private bool closeWhenClickCancelBtn = true;
    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected Button_UI okBtn;
    [SerializeField] protected Button_UI cancelBtn;
    [SerializeField] protected Button_UI closeBtn;
    [Header("Animation Popup")]
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected CanvasGroup group;

    protected Action<TResult> onConfirm;
    protected Action onCancel;

    public bool IsVisible => gameObject.activeInHierarchy;

    protected override void Awake()
    {
        base.Awake();
        LoadComponent();
        
        if (group == null)
            group = gameObject.AddComponent<CanvasGroup>();

        SetupButtons();
        Hide();
    }

    private void SetupButtons()
    {
        if (okBtn != null)
        {
            okBtn.SetHoverBehaviourType();
            okBtn.ClickFunc = OnOkClicked;
        }

        if (cancelBtn != null  )
        {
            cancelBtn.SetHoverBehaviourType();
            cancelBtn.ClickFunc = OnCancelClicked;
        }
        if(closeBtn != null)
        {
            closeBtn.SetHoverBehaviourType();
            closeBtn.ClickFunc = OnCancelClicked;
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowPopup(TData data, Action<TResult> onConfirm = null, Action onCancel = null)
    {
        this.onConfirm = onConfirm;
        this.onCancel = onCancel;

        SetupPopupData(data);
        PopupManager.Instance.ShowPopup<BasePopup<TData, TResult>>(this);
    }

    protected virtual void SetupPopupData(TData data)
    {
        if (titleText != null && !string.IsNullOrEmpty(data.Title))
        {
            titleText.text = data.Title;
        }
    }

    protected virtual void OnOkClicked()
    {
        var result = GetResult();
        if (ValidateResult(result))
        {
            onConfirm?.Invoke(result);
            if (!closeWhenClickOkBtn) return;
            PopupManager.Instance.HidePopup(this);
        }
    }

    protected virtual void OnCancelClicked()
    {
        onCancel?.Invoke();
        if (!closeWhenClickCancelBtn) return;
        PopupManager.Instance.HidePopup(this);
    }

    protected abstract TResult GetResult();
    protected virtual bool ValidateResult(TResult result) => true;

    protected char ValidateChar(string validCharacters, char addedChar)
    {
        if (string.IsNullOrEmpty(validCharacters) || validCharacters.Contains(addedChar))
        {
            return addedChar;
        }
        return '\0';
    }
    protected override void LoadComponent()
    {
        titleText = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "titleText");
        okBtn = GetComponentsInChildren<Button_UI>().FirstOrDefault(x => x.name == "okBtn");
        cancelBtn = GetComponentsInChildren<Button_UI>().FirstOrDefault(x => x.name == "cancelBtn");
        closeBtn = GetComponentsInChildren<Button_UI>().FirstOrDefault(x => x.name == "closeBtn");
        group = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
    }
}