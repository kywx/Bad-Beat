using UnityEngine;

public class LocatorScript : MonoBehaviour
{
    public static LocatorScript Instance {get; private set; }

    public BossMinionSpawner SpawnerManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);



        GameObject SpawnerManage = GameObject.FindWithTag("Miniboss");
        SpawnerManager = SpawnerManage.GetComponent<BossMinionSpawner>();

        //Ex on how to use to get a reference --> LocatorScript.Instance.SpawnerManager.METHOD_NAME_HERE
    }

    
}
