using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Joystick m_joystick;
    [SerializeField] private float m_speed;
    [SerializeField] private Animator m_animator;
    [SerializeField] private GameObject m_clothSocket;
    [SerializeField] private GameObject m_clothPrefab;

    private int m_collectedCloths;
    private Rigidbody m_Rb;

    private Dictionary<Utils.ShirtColors, int> m_collectedClothsInventory = new Dictionary<Utils.ShirtColors, int>();

    public bool IsClothInInventory(Utils.ShirtColors color)
    {
        if (m_collectedClothsInventory.ContainsKey(color))
        {
            if (m_collectedClothsInventory[color] > 1)
            {
                m_collectedClothsInventory[color]--;
            }
            else
            {
                m_collectedClothsInventory.Remove(color);
            }

            m_collectedCloths--;
            RemoveClothFromInventory(color);
            return true;
        }

        return false;
    }

    private void Awake()
    {
        RackManager.OnCollectedCloth += OnOnCollectedCloth;
    }

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_animator.SetBool("IsWalking", GetComponent<Rigidbody>().velocity.magnitude > 0);
        m_animator.SetBool("IsHolding", m_collectedCloths > 0);
    }
    
    private void FixedUpdate()
    {
        float horizontalInput = m_joystick.Horizontal;
        float verticalInput = m_joystick.Vertical;
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        
        if (movement == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(movement);
        targetRotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360 * Time.fixedDeltaTime);
        m_Rb.MovePosition(m_Rb.position + movement * (m_speed * Time.fixedDeltaTime));
        m_Rb.MoveRotation(targetRotation);
    }
    
    private void OnOnCollectedCloth(Utils.ShirtColors color)
    {
        m_collectedCloths++;
        if (m_collectedClothsInventory.ContainsKey(color))
        {
            m_collectedClothsInventory[color] += 1;
        }
        else
        {
            m_collectedClothsInventory[color] = 1;
        }

        Vector3 position = m_clothSocket.transform.position;
        Vector3 spawnPos = new Vector3(position.x,
            position.y + (m_collectedCloths * 0.08f),
            position.z);

        GameObject obj = Instantiate(m_clothPrefab, spawnPos, Quaternion.identity);
        obj.GetComponent<MeshRenderer>().material.color = Utils.GetShirtColor(color);
        obj.transform.SetParent(m_clothSocket.transform);
    }

    private void RemoveClothFromInventory(Utils.ShirtColors color)
    {
        foreach (MeshRenderer meshRenderer in m_clothSocket.GetComponentsInChildren<MeshRenderer>())
        {
            if (meshRenderer.material.color == Utils.GetShirtColor(color))
            {
                Destroy(meshRenderer.gameObject);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        RackManager.OnCollectedCloth -= OnOnCollectedCloth;
    }
}
