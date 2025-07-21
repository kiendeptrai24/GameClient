
[System.Serializable]
public class SignInDTO
{
    public string username;
    public string password;
    public SignInDTO(string email, string password)
    {
        this.username = email;
        this.password = password;
    }
}
