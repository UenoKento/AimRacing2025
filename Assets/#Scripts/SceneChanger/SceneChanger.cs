using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string m_sceneName;

    [SerializeField]
    float m_wait;

    float m_counter;

    void Start()
    {
        m_counter = 0f;
    }

    void Update()
    {
        m_counter += Time.deltaTime;

        if (m_wait < m_counter)
            ChangeScene();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(m_sceneName);
    }
}
