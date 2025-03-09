
using System.Security.Cryptography;
using UnityEngine;

public class UserClient : MonoBehaviour 
{
    private string registerRoute = "/account/register";
    private string loginRoute = "/account/login";

    public async Awaitable<IApiResponse> Register(User user)
    {
        string data = JsonUtility.ToJson(user);

        return await ApiClient.instance.SendPostRequest(registerRoute, data);
    }

    public async Awaitable<IApiResponse> Login(User user)
    {
        string data = JsonUtility.ToJson(user);

        var response = await ApiClient.instance.SendPostRequest(loginRoute, data);
        switch (response)
        {
            case ApiResponse<string> res:
                Token token = JsonUtility.FromJson<Token>(res.data);
                Debug.Log(res.data);
                Debug.Log(JsonUtility.ToJson(token));
                ApiClient.instance.SetToken(token.accessToken);
                return response;
            default:
                return response;
        }
    }
}
