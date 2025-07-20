
[System.Serializable]
public class SignInDTO
{
    public string email;
    public string password;
    public SignInDTO(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}
