using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class RoomUI : KienBehaviour
{
    public TextMeshProUGUI id;
    public TextMeshProUGUI host;
    public TextMeshProUGUI maxMembers;
    public TextMeshProUGUI mode;
    public Image sprite;
    public Transform imageFocus;
    [SerializeField] private Button roomBtn;

    public Room data;
    public void SetData(Room room)
    {
        data = room;
        //this.id.text = room.id;
        this.host.text = room.host.name;
        this.mode.text = room.mode;
        this.maxMembers.text = room.max.ToString();
    }
    protected override void Start()
    {
        base.Start();
        LoadComponent();
        roomBtn.onClick.AddListener(() => LobbySellectedUI.Instance.RoomSelected(this));
    }
    public void HideFocus() => imageFocus.gameObject.SetActive(false);
    public void ShowFocus() => imageFocus.gameObject.SetActive(true);
    protected override void LoadComponent()
    {
        base.LoadComponent();
        id = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "roomId");
        host = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "host");
        maxMembers = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "maxMembers");
        mode = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x.name == "mode");
        sprite = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Flag");
        imageFocus = GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "imageFocus");
        roomBtn = GetComponent<Button>();
    }
}
[System.Serializable]
public class Room
{
    public string id;
    public string name;
    public User host;
    public List<User> members;
    public string mode;
    public string status;
    public int max;
}
[System.Serializable]
public class RoomRequestDTO
{
    public string roomId;
    public string mode;
    public int max;
    public RoomRequestDTO(string id, string mode, int max)
    {
        this.roomId = id;
        this.mode = mode;
        this.max = max;
    }
}
