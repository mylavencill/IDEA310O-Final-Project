using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey1 = false;
    public bool hasKey2 = false;
    public bool hasKey3 = false;

    public bool HasAllKeys()
    {
        return hasKey1 && hasKey2 && hasKey3;
    }
}