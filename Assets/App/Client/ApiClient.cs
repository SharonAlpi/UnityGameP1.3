using System.Text;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;


public class ApiClient : MonoBehaviour
{
    public string baseUrl;
    private string token;

    public void SetToken(string token)
    {
        this.token = token;
    }

    public async Awaitable<IApiResponse> SendGetRequest(string route)
    {
        var request = CreateWebRequest("GET", route, "");
        return await SendWebRequest(request);
    }
    public async Awaitable<IApiResponse> SendPostRequest(string route, string data)
    {
        var request = CreateWebRequest("POST", route, data);
        return await SendWebRequest(request);
    }
    public async Awaitable<IApiResponse> SendPutRequest(string route, string data)
    {
        var request = CreateWebRequest("PUT", route, data);
        return await SendWebRequest(request);
    }
    public async Awaitable<IApiResponse> SendDeleteRequest(string route)
    {
        var request = CreateWebRequest("DELETE", route, "");
        return await SendWebRequest(request);
    }
    private UnityWebRequest CreateWebRequest(string method, string route, string data)
    {
        string url = baseUrl + route;
        Debug.Log("Creating " + method + " request to " + url + " with data: " + data);

        data = RemoveIdFromJson(data); // Backend throws error if it receiving empty strings as a GUID value.
        var webRequest = new UnityWebRequest(url, method);
        byte[] dataInBytes = new UTF8Encoding().GetBytes(data);
        webRequest.uploadHandler = new UploadHandlerRaw(dataInBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        AddToken(webRequest);
        return webRequest;
    }

    private async Awaitable<IApiResponse> SendWebRequest(UnityWebRequest req)
    {
        await req.SendWebRequest();
        switch (req.result) 
        { 
            case UnityWebRequest.Result.Success:
                return new ApiResponse<string>(req.downloadHandler.text);
            default:
                return new ApiError(req.error);
        }
    }

    private void AddToken(UnityWebRequest webRequest)
    {
        webRequest.SetRequestHeader("Authorization", "Bearer " + token);
    }

    private string RemoveIdFromJson(string json)
    {
        return json.Replace("\"id\":\"\",", "");
    }

}

[Serializable]
public class Token
{
    public string tokenType;
    public string accessToken;
}
