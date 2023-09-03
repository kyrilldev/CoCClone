using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugRequestGUI : MonoBehaviour
{
    private VisualElement root;
    private Button requestButton;
    private Label requestLabel;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        requestButton = root.Q<Button>("SendButton");
        requestLabel = root.Q<Label>("Output-Label");
        requestButton.RegisterCallback<ClickEvent>(evt =>
        {
            requestLabel.text += "Het werk \n";
        });
    }
}
