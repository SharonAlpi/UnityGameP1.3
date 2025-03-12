using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoginManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_Text errorMessage;

    public UserClient client;
    // Update is called once per frame
    public void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
    }
    public async void Login()
    {
        var emailInput = email.text;
        var passwordInput = password.text;
        Debug.Log("Login: user:" + emailInput + "; ww:" + passwordInput);
        //return;
        var user = new User
        {
            Password = passwordInput,
            Email = emailInput,
        };
        var response = await client.Login(user);
        switch (response)
        {
            case ApiResponse<string> res:
                SceneManager.LoadScene("Environments", LoadSceneMode.Single);
                break;
            case ApiError error:
                if (error.statusCode == 401)
                {
                    errorMessage.SetText("Wrong email or password."); 
                }
                else
                {
                    errorMessage.SetText("Something went wrong, try again later");
                }
                break;
            default:
                throw new NotImplementedException("No implementation for ApiResponse of class: " + response.GetType());
        }
    }
    public async void Register()
    {
        var emailInput = email.text;
        var passwordInput = password.text;
        Debug.Log("Register: user:"+ emailInput + "; ww:"+passwordInput);
        //return;
        var user = new User
        {
            Password = passwordInput,
            Email = emailInput,
        };
        var response = await client.Register(user);
        switch (response)
        {
            case ApiResponse<string> res:
                SceneManager.LoadScene("Login", LoadSceneMode.Single);
                break;
            case ApiError error:

                errorMessage.SetText(error.errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for ApiResponse of class: " + response.GetType());
        }
    }
    public void Toggle(GameObject obj)
    {
        var isActive = obj.activeSelf;
        obj.SetActive(!isActive);
    }
}
