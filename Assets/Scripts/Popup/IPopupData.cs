public interface IPopupData
{
    string Title { get; }
}

public interface IPopup
{
    void Show();
    void Hide();
    bool IsVisible { get; }
}

public interface IPopupCallback<T>
{
    void OnConfirm(T result);
    void OnCancel();
}
