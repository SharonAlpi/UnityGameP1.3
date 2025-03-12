using UnityEngine;

public class ApiError : IApiResponse
{
    public string errorMessage;
    public long statusCode;

    public ApiError(string errorMessage, int code)
    { 
        this.errorMessage = errorMessage; 
        statusCode = code;
    }
}
