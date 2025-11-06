using UnityEngine;

public class PlayerEconomy : MonoBehaviour
{
    public int gold = 1000;

    public bool Spend(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}
