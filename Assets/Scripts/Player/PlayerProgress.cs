using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    [Header("Upgradeable Stats")]
    public int magicLevel = 1;
    public int maxMagicLevel = 3;

    public bool hasBoat = false;

    public bool CanUpgradeMagic() => magicLevel < maxMagicLevel;
    public bool TryUpgradeMagic()
    {
        if (!CanUpgradeMagic()) return false;
        magicLevel++;
        return true;
    }
}
