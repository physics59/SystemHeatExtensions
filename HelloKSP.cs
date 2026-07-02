using UnityEngine;

[KSPAddon(KSPAddon.Startup.MainMenu, false)]
public class Fusion : MonoBehaviour
{   
    public void Start()
    {
        foreach (var part in PartLoader.LoadedPartsList)
        {
            if (part.name.Contains("fusion") ||
                part.name.Contains("Fusion"))
            {
                Debug.Log("[Fusion!] detected: " + part.name);
            }
        }
        Debug.Log("[Fusion!] Version 0.0.1 loaded");
    }
}