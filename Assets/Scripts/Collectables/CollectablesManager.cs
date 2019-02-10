using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    #region Singleton
    private static CollectablesManager _instance;

    public static CollectablesManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public List<Collectable> AvailableBuffs;
    public List<Collectable> AvailableDebuffs;

    [Range(0, 100)]
    public float BuffChance;

    [Range(0, 100)]
    public float DebuffChance;
}
