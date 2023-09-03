using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class Fruit : MonoBehaviour
{
    string uri = "http://127.0.0.1/edsa-webdev/sendingtest.php";

    //IEnumerator Start()
    //{
    //    Request request = new Request();
    //    //request.fruits = new string[] { "Peer", "Appel", "PineApple" };
    //    //request.name = new string("Kyrill");
    //    string json = JsonUtility.ToJson(request);

    //    List<IMultipartFormSection> formData = new();
    //    MultipartFormDataSection entry = new MultipartFormDataSection("json", json);
    //    formData.Add(entry);

    //    using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, formData))
    //    {
    //        yield return webRequest.SendWebRequest();
    //        //Debug.Log(webRequest.downloadHandler.text);
    //        FruitResponse response = JsonUtility.FromJson<FruitResponse>(webRequest.downloadHandler.text);
    //        Debug.Log(response.fruit[UnityEngine.Random.Range(0, 3)]);
    //    }
    //}
}

public class Request
{
    public string action = "get_fruit";
}

public class FruitResponse
{
    public string[] fruit;
}