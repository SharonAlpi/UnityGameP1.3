using UnityEngine;

public class ApiResponse<T> : IApiResponse
{
    public T data;
    public ApiResponse(T data)
    {
        this.data = data;
    }
}
