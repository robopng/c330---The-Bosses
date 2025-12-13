using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform enemyGroup;
    public Collider2D gateCollider;

    private GameObject[] enemies;
    private bool gateOpened = false;

    void Awake()
    {
        // Get all enemy under the group
        int count = enemyGroup.childCount;
        enemies = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            enemies[i] = enemyGroup.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (gateOpened) return;

        bool isAllDead = true;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null) // still alive
            {
                isAllDead = false;
                break;
            }
        }


        if (isAllDead)
        {
            OpenGate();
        }          

    }

    void OpenGate()
    {
        gateOpened = true; // stop future check
        gateCollider.enabled = false; // player can pass!
        
       Debug.Log("Room cleared, gate opened!");
    }
}
