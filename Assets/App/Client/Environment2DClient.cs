using System.Collections.Generic;
using UnityEngine;

public class Environment2DClient : MonoBehaviour
{
    public ApiClient client;

    public async Awaitable<IApiResponse> ReadEnvironments()
    {
        var route = "/Environment2D";
        IApiResponse response = await client.SendGetRequest(route);
        switch (response)
        {
            case ApiResponse<string> res:
                var jsonData = JsonHelper.ParseJsonArray<Environment2D>(res.data);
                return new ApiResponse<IEnumerable<Environment2D>>(jsonData);
            default:
                return response;
        };
    }
    public async Awaitable<IApiResponse> ReadEnvironment(string envId)
    {
        var route = "/Environment2D/" + envId;
        IApiResponse response = await client.SendGetRequest(route);
        switch (response)
        {
            case ApiResponse<string> res:
                var jsonData = JsonUtility.FromJson<Environment2D>(res.data);
                return new ApiResponse<Environment2D>(jsonData);
            default:
                return response;
        };
    }
    public async Awaitable<IApiResponse> DeleteEnvironment(string envId)
    {
        var route = "/Environment2D/" + envId;
        return await client.SendDeleteRequest(route);
    }
}
