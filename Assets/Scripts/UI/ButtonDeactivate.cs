using UnityEngine;

public class ButtonDeactivate : MonoBehaviour
{
    public GameObject group;

    public void Disable()
    {
        group.SetActive(false);
    }

    public void Enable()
    {
        group.SetActive(true);
    }
}
