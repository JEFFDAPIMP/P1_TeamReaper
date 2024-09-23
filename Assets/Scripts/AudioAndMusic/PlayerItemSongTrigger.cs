using UnityEngine;

public class PlayerItemSongTrigger : MonoBehaviour
{
    private MusicManager musicManager;
    private PlayerInventorySwitcher playerInventorySwitcher;
    private int currentInventoryIndex = 0;
    private int nextInventoryIndex;
    public int musicManagerIndexOffset = 0;
    public float volume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        playerInventorySwitcher = FindObjectOfType<PlayerInventorySwitcher>();
    }

    // Update is called once per frame
    void Update()
    {
        nextInventoryIndex = playerInventorySwitcher.GetCurrentWeaponIndex();
        if (currentInventoryIndex != nextInventoryIndex)
        {
            musicManager.ChangeTrack(nextInventoryIndex + musicManagerIndexOffset, volume);
            musicManager.StopTrack(currentInventoryIndex + musicManagerIndexOffset);
            currentInventoryIndex = nextInventoryIndex;
        }
    }
}
