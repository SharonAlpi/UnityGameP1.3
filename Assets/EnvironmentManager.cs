using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{
    public TMP_InputField environmentName;
    public TMP_InputField height;
    public TMP_InputField length;

    public Environment2DClient client;
    public GameObject WorldPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    async void Start()
    {
        var response = await client.ReadEnvironments();
        switch (response)
        {
            case ApiResponse<IEnumerable<Environment2D>> res:
                var environments = res.data.ToList();
                for (var i = 0; i < environments.Count; i++)
                {
                    // list all the environments under eachother
                    var position = new Vector3(0, 250 - (i * 30), 0);
                    var gameObject = Instantiate(WorldPrefab, position, Quaternion.identity);
                    Button tempButton = gameObject.GetComponent<Button>();
                    tempButton.onClick.AddListener(() =>
                    {
                        ObjectManager.instance.EnvironmentID = environments[i].id;
                        SceneManager.LoadScene("game", LoadSceneMode.Single);
                    });
                }
                break;
            case ApiError error:
                Debug.Log(error);
                break;
            default:
                throw new NotImplementedException("No implementation for ApiResponse of class: " + response.GetType());
        }
    }

    public async void CreateNewWorld()
    {
        var nameInput = environmentName.text;
        var heightInput = Int32.Parse(height.text);
        var lengthInput = Int32.Parse(length.text);

        Debug.Log("Create world: name:" + nameInput + "; height:" + heightInput + "; length: " + lengthInput);
        //return;
        var environment2D = new Environment2D
        {
            name = nameInput,
            maxLength = lengthInput,
            maxHeight = heightInput,
        };
        var response = await client.AddEnvironment(environment2D);
        switch (response)
        {
            case ApiResponse<string> res:
                SceneManager.LoadScene("environment", LoadSceneMode.Single);
                break;
            case ApiError error:
                Debug.Log(error.errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for ApiResponse of class: " + response.GetType());
        }

    }
}
