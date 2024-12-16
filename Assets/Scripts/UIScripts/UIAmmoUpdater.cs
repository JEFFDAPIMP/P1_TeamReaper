using UnityEngine;
using TMPro;

public class UIAmmoUpdater : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI ammoCountText;

    private string playerTag = "Player";
    private string MainCameraTag = "MainCamera";

    public PlayerInventorySwitcher playerInventorySwitcher;

    // Start is called before the first frame update
    void Start()
    {
        if (playerInventorySwitcher == null)
        {
            playerInventorySwitcher = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerInventorySwitcher>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        weaponNameText.text = playerInventorySwitcher.getCurrentWeapon().weaponName;
        ammoCountText.text = playerInventorySwitcher.getCurrentWeapon().currentAmmoCount.ToString() + "/" + playerInventorySwitcher.getCurrentWeapon().maxAmmoCount.ToString();
    }
}
