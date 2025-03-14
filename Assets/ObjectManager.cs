using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectManager : MonoBehaviour
{
    [Serializable]
    public class GameObjectEntry
    {
        public string key;
        public GameObject value;
    }
    public List<GameObjectEntry> RegisterdGameObjectsList;
    private Dictionary<string,GameObject> RegisterdGameObjects = new();
    public GameObjectEntry currentObject;
 
    private IDictionary<string, Object2D> createdObjects = new Dictionary<string, Object2D>();
    public Object2DClient client;
    public static string EnvironmentID = "<environment id>";
    public static ObjectManager instance { get; private set; }
public async void Start()
    {
        RegisterdGameObjectsList.ForEach(entry => RegisterdGameObjects[entry.key] = entry.value);
        await InitializeObjects();
    }

    void Awake()
    {
        // hier controleren we of er al een instantie is van deze singleton
        // als dit zo is dan hoeven we geen nieuwe aan te maken en verwijderen we deze
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public async void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentObject != null && !IsPointerOverUI())
        {
            await CreateObject();
        }
    }
    public void SetObjectPlace(string prefabId)
    {
        this.currentObject = new GameObjectEntry();
        this.currentObject.key = prefabId;
        this.currentObject.value = RegisterdGameObjects[prefabId];
        
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private async  Task CreateObject()
    {
        Vector3 mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePositon.z = 0;
        Object2D newObject = new()
        {
            positionX = mousePositon.x,
            positionY = mousePositon.y,
            environmentId = EnvironmentID,
            rotationZ = 0,
            scaleX = 1, 
            scaleY = 1,
            prefabId = currentObject.key,
        };
        IApiResponse res = await client.CreateObject(newObject);
        switch (res)
        {
            case ApiResponse<Object2D> newObj:
                Debug.Log(JsonUtility.ToJson(newObj.data));
                createdObjects.Add(newObj.data.id, newObj.data);
                Instantiate(currentObject.value, mousePositon, Quaternion.identity);
                break;
            case ApiError err:
                Debug.Log(err.errorMessage);
                break;
        }

    }
    public async Task InitializeObjects()
    {
        IApiResponse res = await client.ReadObjects(EnvironmentID);
        switch (res)
        {
            case ApiResponse<IEnumerable<Object2D>> objs :
                objs.data.ToList().ForEach(obj => {
                    createdObjects.Add(obj.id, obj);
                    Vector3 objPos = new Vector3(obj.positionX, obj.positionY, 0);
                    Instantiate(RegisterdGameObjects[obj.prefabId], objPos, Quaternion.identity);
                });
                break;
            case ApiError err:
                Debug.Log(err);
                break;
        }
    }
}
