using UnityEngine;

public class Goalflag_Move : MonoBehaviour
{
    [SerializeField]
    GameObject Goal_Image;
    void FixedUpdate()
    {
		GetComponent<RectTransform>().anchoredPosition = Goal_Image.GetComponent<RectTransform>().anchoredPosition;
    }
}
