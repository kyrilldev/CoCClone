using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class LoginScreen : MonoBehaviour
{
    private const string url = "http://127.0.0.1/edsa-webdev/api.php";

    private VisualElement root;
    private TextField createEmailText;
    private TextField createUsernameText;
    private TextField createPasswordText;
    private Button createAccountButton;
    private TextField loginTextEmail;
    private TextField loginPasswordText;
    private Button loginButton;
    private Label outputLabel;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        createEmailText = root.Q<TextField>("Create-Email-Text");
        createUsernameText = root.Q<TextField>("Create-Username-Text");
        createPasswordText = root.Q<TextField>("Create-Password-Text");
        createAccountButton = root.Q<Button>("Create-Account-Button");
        loginTextEmail = root.Q<TextField>("Login-Email-Text");
        loginPasswordText = root.Q<TextField>("Login-Password-Text");
        loginButton = root.Q<Button>("Login-Account-Button");
        outputLabel = root.Q<Label>("Output-Label");

        createAccountButton.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("werkt");
            if (coroutine == null)
            {
                coroutine = CreateAccountAsync();
                StartCoroutine(coroutine);
            }
        });
        loginButton.RegisterCallback<ClickEvent>(evt =>
        {

        });
    }
    private IEnumerator CreateAccountAsync()
    {
        CreateAccountRequest request = new CreateAccountRequest();
        request.email = createEmailText.text;
        request.username = createUsernameText.text;
        request.password = createPasswordText.text;

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string json = JsonUtility.ToJson(request);

        MultipartFormDataSection entry = new("json", json);
        formData.Add(entry);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, formData))
        {
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.downloadHandler.text);
            CreateAccountResponse response = JsonUtility.FromJson<CreateAccountResponse>(webRequest.downloadHandler.text);
            Debug.Log(response.serverMessage);
        }
        coroutine = null;
    }
}

[System.Serializable]
public class CreateAccountRequest
{
    public string action = "create_account";
    public string email;
    public string username;
    public string password;
}

[System.Serializable]
public class CreateAccountResponse
{
    public string serverMessage;
}
