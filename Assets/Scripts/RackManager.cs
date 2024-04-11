using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RackManager : MonoBehaviour
{

    [SerializeField] private Utils.ShirtColors m_color;
    [SerializeField] private GameObject m_shirtsParent;
    [SerializeField] private Image m_shirtImage;
    [SerializeField] private Image m_progressBar;
    [SerializeField] private GameObject m_collectProgressbarHolder;

    private bool m_isPlayerInArea = false;
    private IEnumerator m_countdown;

    public static event Action<Utils.ShirtColors> OnCollectedCloth; 

    public bool isUnlocked;

    private void Start()
    {
        if(!isUnlocked) return;
        
        if (m_shirtsParent != null)
        {
            foreach (MeshRenderer meshRenderer in m_shirtsParent.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                meshRenderer.material.color = Utils.GetShirtColor(m_color);
                m_shirtImage.color = Utils.GetShirtColor(m_color);
            }
        }
    }

    private void Update()
    {
        if (m_collectProgressbarHolder != null && m_collectProgressbarHolder.activeSelf)
        {
            m_collectProgressbarHolder.transform.LookAt(Camera.main.transform);
        }
    }

    private void ConfigureCollectProgressBar(bool active)
    {
        m_collectProgressbarHolder.SetActive(active);

        if (active)
        {
            m_countdown = Countdown();
            StartCoroutine(m_countdown);
        }
        else
        {
            StopCoroutine(m_countdown);
            m_countdown = null;
        }
    }

    private IEnumerator Countdown()
    {
        float duration = 2f;
        float normalizedTime = 0;
        while(normalizedTime <= 1f)
        {
            m_progressBar.fillAmount = normalizedTime;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        CollectedCloth();
    }

    private void CollectedCloth()
    {
        StopCoroutine(m_countdown);
        OnCollectedCloth?.Invoke(m_color);
        m_countdown = Countdown();
        StartCoroutine(m_countdown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isUnlocked) return;
        
        if (other.CompareTag("Player"))
        {
            m_isPlayerInArea = true;
            ConfigureCollectProgressBar(m_isPlayerInArea);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!isUnlocked) return;
        
        if (other.CompareTag("Player"))
        {
            m_isPlayerInArea = false;
            ConfigureCollectProgressBar(m_isPlayerInArea);
        }
    }
}