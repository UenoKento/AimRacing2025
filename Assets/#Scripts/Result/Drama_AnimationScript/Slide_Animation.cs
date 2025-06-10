using UnityEngine;

public class Slide_Animation : MonoBehaviour
{

    [SerializeField]
    float m_WaitTime;
    float time;

    [SerializeField]
    Vector3 Target_Pos;

    [SerializeField]
    float Lerp_Speed;

    void Start()
    {
        
    }

    void Update()
    {

        time += Time.deltaTime;

        if(!(time >= m_WaitTime)) { return; }

        Vector3 pos = GetComponent<RectTransform>().anchoredPosition;

        pos.x = Mathf.Lerp(GetComponent<RectTransform>().anchoredPosition.x, Target_Pos.x, Lerp_Speed);

        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
