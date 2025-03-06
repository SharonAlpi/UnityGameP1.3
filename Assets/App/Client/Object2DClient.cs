using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit;
using UnityEngine;

public class Object2DClient : MonoBehaviour
{
    public ApiClient client;
    
    public async Awaitable<IApiResponse> ReadObjects(string env)
    {
        var route = "/Environment2D/" + env + "/Object2D";
        IApiResponse response = await client.SendGetRequest(route);
        switch (response) 
        {
            case ApiResponse<string> res:
                var jsonData = JsonHelper.ParseJsonArray<Object2D>(res.data);
                return new ApiResponse<IEnumerable<Object2D>>(jsonData);
            default:
                return response;
        }
    }
    public async Awaitable<IApiResponse> CreateObject(Object2D obj)
    {
        string data = JsonUtility.ToJson(obj);
        var route = "/Environment2D/" + obj.environmentId + "/Object2D";

        IApiResponse response = await client.SendPostRequest(route, data);
        switch (response) 
        {
            case ApiResponse<string> res: 
                var jsonData = JsonHelper.ParseJsonArray<Object2D>(res.data);
                return new ApiResponse<Object2D>(JsonUtility.FromJson<Object2D>(res.data));
            default:
                return response;
        };
    }
    public async Awaitable<IApiResponse> UpdateObject2D(Object2D obj)
    {
        var route = "/Environment2D/" + obj.environmentId + "/Object2D/" + obj.environmentId;
        string data = JsonUtility.ToJson(obj);
        return await client.SendPutRequest(route, data);
    }


}
