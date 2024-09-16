using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private GameObject Boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Boss.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
