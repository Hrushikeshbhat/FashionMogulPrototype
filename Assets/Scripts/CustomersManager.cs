using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CustomersManager : MonoBehaviour
{
    [SerializeField] private List<StandingArea> m_mirrorLocations;
    [SerializeField] private GameObject m_customer;
    [SerializeField] private float m_spawnInterval = 5f;

    private void Awake()
    {
        GameManager.StartGame += OnStartGame;
    }

    private void OnStartGame()
    {
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            StandingArea area = GetAvailableMirror();
            if (area != null)
            {
                GameObject obj = Instantiate(m_customer, transform.position, transform.rotation);
                obj.GetComponent<NavMeshAgent>().destination = area.transform.position;
            }
            yield return new WaitForSeconds(m_spawnInterval);
        }
    }

    private StandingArea GetAvailableMirror()
    {
        return m_mirrorLocations.FirstOrDefault(x=> !x.isOccupied);
    }
}
