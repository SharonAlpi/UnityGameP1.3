
using System.Security.Cryptography;
using UnityEngine;

public class UserClient : MonoBehaviour 
{
    public ApiClient client;
    private string registerRoute = "/account/register";
    private string loginRoute = "/account/register";

    public async Awaitable<IApiResponse> Register(User user)
    {
        string data = JsonUtility.ToJson(user);

        return await client.SendPostRequest(registerRoute, data);
    }

    public async Awaitable<IApiResponse> Login(User user)
    {
        string data = JsonUtility.ToJson(user);

        var response = await client.SendPostRequest(loginRoute, data);
        switch (response)
        {
            case ApiResponse<string> res:
                Token token = JsonUtility.FromJson<Token>(res.data);
                client.SetToken(token.accessToken);
                return response;
            default:
                return response;
        }
    }
}
