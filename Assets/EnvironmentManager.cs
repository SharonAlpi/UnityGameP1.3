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
                    var position = new Vector3(-250, 375 - (i * 175), 0);
                    instantiateObject(position, environments[i]);
                }
                break;
            case ApiError error:
                Debug.Log(error);
                break;
            default:
                throw new NotImplementedException("No implementation for ApiResponse of class: " + response.GetType());
        }
    }

    public void instantiateObject(Vector3 pos, Environment2D env)
    {
        var gameObject = Instantiate(WorldPrefab, pos, Quaternion.identity);
        // make sure the object is a child of the canvas
        gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
        // set the name of the object
        TMP_Text nameComponent = gameObject.transform.Find("WorldName")?.GetComponent<TMP_Text>();
        if (nameComponent != null)
        {
            nameComponent.SetText(env.name);
        }
        //Text sizeComponent = gameObject.transform.Find("Size")?.GetComponent<Text>();
        //if (sizeComponent != null)
        //{
        //    sizeComponent.text = $"W:{env.maxLength}\nH:{env.maxHeight}";
        //}


        Button loadButton = gameObject.transform.Find("LoadButton").GetComponent<Button>();
        loadButton.onClick.AddListener(() =>
        {
            ObjectManager.EnvironmentID = env.id;
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        });
        Button deleteButton = gameObject.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(async () =>
        {
            await Deleteworld(env.id);
            SceneManager.LoadScene("Environments", LoadSceneMode.Single);
        });
    }
    public async Awaitable Deleteworld(string envId)
    {
        var response = await client.DeleteEnvironment(envId);
        switch (response)
        {
            case ApiResponse<string> res:
                break;
            case ApiError error:
                Debug.Log(error.errorMessage);
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
                SceneManager.LoadScene("Environments", LoadSceneMode.Single);
                break;
            case ApiError error:
                Debug.Log(error.errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for ApiResponse of class: " + response.GetType());
        }
    }
}
