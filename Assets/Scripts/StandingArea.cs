using UnityEngine;

public class StandingArea : MonoBehaviour
{
    public bool isOccupied;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            isOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            isOccupied = false;
        }
    }
}
