using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    [SerializeField] private Animator m_anim;
    [SerializeField] private Utils.ShirtColors m_shirtColor;
    [SerializeField] private Image m_shirtImage;
    [SerializeField] private GameObject m_shirtImageContainer;

    private Vector3 m_outside = new Vector3(0, 0, -16);

    private void OnEnable()
    {
        m_shirtColor = (Utils.ShirtColors)Random.Range(0, 4);
        m_shirtImage.color = Utils.GetShirtColor(m_shirtColor);
    }

    private void Update()
    {
        if (m_shirtImageContainer != null && m_shirtImageContainer.activeSelf)
        {
            m_shirtImageContainer.transform.LookAt(Camera.main.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StandingArea"))
        {
            m_anim.SetBool("IsWaiting", true);
        }

        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerManager>().IsClothInInventory(m_shirtColor))
            {
                m_anim.SetBool("IsWaiting", false);

                GetComponent<NavMeshAgent>().destination = m_outside;
                
                m_shirtImage.color = Color.green;
                
                Destroy(m_shirtImageContainer.gameObject, 1f);
                Destroy(gameObject, 5f);
            }
        }
    }
}
