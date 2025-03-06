using UnityEngine;

public class ApiError : IApiResponse
{
    public string errorMessage;

    public ApiError(string errorMessage)
    { 
        this.errorMessage = errorMessage; 
    }
}
