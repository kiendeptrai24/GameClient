using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessegeUI : KienBehaviour
{
    public TextMeshProUGUI userName;
    public TextMeshProUGUI messege;
    public TextMeshProUGUI timestamp;
    public void SetData(Messege messege)
    {
        this.userName.text = messege.username;
        this.messege.text = messege.messege;
        this.timestamp.text = messege.timestamp;

    }
    protected override void LoadComponent()
    {
        userName = GameObject.Find("name").GetComponent<TextMeshProUGUI>();
        messege = GameObject.Find("chat").GetComponent<TextMeshProUGUI>();
        timestamp = GameObject.Find("timestamp").GetComponent<TextMeshProUGUI>();
    }
}
public class Messege
{
    public string userId;
    public string username;
    public string messege;
    public string timestamp;
    public Messege() { }
    public Messege(string userId, string userName, string messege,string timestamp) 
    { 
        this.userId = userId;
        this.username = userName;
        this.messege = messege;
        this.timestamp = timestamp;
    }
}