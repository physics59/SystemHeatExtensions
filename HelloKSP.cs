using UnityEngine;

[KSPAddon(KSPAddon.Startup.MainMenu, false)]
public class StartUp : MonoBehaviour
{   
    public void Start()
    {
        Debug.Log("[SystemHeatExpansion] Version 0.1.0 loaded");
    }
}