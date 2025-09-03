
public class LoginResponse
{
    public LoginError Status { get; private set; }
        
    public string Message { get; private set; }


    public LoginResponse(LoginError status)
    {
        Status = status;
        Message = string.Empty;
    }

    public LoginResponse(LoginError status, string message)
    {
        Status = status;
        Message = message;
    }

}


