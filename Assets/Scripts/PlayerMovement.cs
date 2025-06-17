using UnityEngine;
using System.Text.Json;

public class PlayerMovement : MonoBehaviour
{
    public string roomId = "room123";
    private float sendInterval = 0.2f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= sendInterval && NetworkManager.socket?.Connected == true)
        {
            SendPosition();
            timer = 0f;
        }
    }

    void SendPosition()
    {
        Vector3 pos = transform.position;
        var data = new
        {
            roomId = roomId,
            position = new { x = pos.x, y = pos.y, z = pos.z }
        };

        string json = JsonSerializer.Serialize(data);
        NetworkManager.socket.Emit("move", json);
    }
}
