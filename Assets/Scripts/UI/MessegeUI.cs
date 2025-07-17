using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessegeUI : KienBehaviour
{
    public TextMeshProUGUI nameTbx;
    public TextMeshProUGUI messegeTbx;
    public TextMeshProUGUI timestampTbx;
    public void SetData(Messege messege)
    {
        this.nameTbx.text = messege?.username;
        this.messegeTbx.text = messege?.messege;
        this.timestampTbx.text = messege?.timestamp == null ? DateTime.Now.ToString() : messege.timestamp;

    }
    protected override void LoadComponent()
    {
        nameTbx = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "nameTbx");
        messegeTbx = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "messegeTbx");
        timestampTbx = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "timestampTbx");
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