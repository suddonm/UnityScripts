using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public SquadController SquadPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SquadController SpawnSquad(string name, HexGridPosition pos)
    {
        SquadController squad = Instantiate<SquadController>(SquadPrefab);
        squad.transform.position = pos.transform.position;
        squad.Position = pos;

        return squad;
    }
}
