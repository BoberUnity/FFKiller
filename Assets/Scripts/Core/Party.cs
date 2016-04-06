using UnityEngine;
using System;
using System.Collections.Generic;

public class Party : MonoBehaviour
{
  [SerializeField] private bool isLevelMap = false; 
  public List<Transform> Vagons = new List<Transform>();
  public List<GameObject> HeroPrefabs = new List<GameObject>();
  [SerializeField] private int distance = 40; //diistance = 40 - 1,3 sec
  private Vector3[] trace = new Vector3[0];

  private void Start ()
  {
    Array.Resize(ref trace, Vagons.Count * distance + 1);
    for (var i = 0; i < trace.Length - 1; i++)
    {
      trace[i] = transform.position;
    }
	}
	
	private void FixedUpdate ()
  {
    if (transform.position != trace[0])
    {
      for (var i = trace.Length - 1; i > 0; i--)
      {
        trace[i] = trace[i - 1];
      }
      trace[0] = transform.position;

      var n = 1;
      foreach (var vagon in Vagons)
      {
        vagon.position = trace[n * distance];
        n += 1;
      }
    }
  }

  public void Connect(string newNpcName)
  {
    foreach (var heroPrefab in HeroPrefabs)
    {
      if (heroPrefab.name == newNpcName || heroPrefab.name == newNpcName + "Vagon")
      {
        GameObject vagon = Instantiate(heroPrefab, transform.position, Quaternion.identity) as GameObject;
        vagon.name = heroPrefab.name;
        Vagons.Add(vagon.transform);
        Start();
        vagon.GetComponent<Hero>().ConnectToPartyGui();
        if (isLevelMap)
          vagon.GetComponentInChildren<SpriteRenderer>().enabled = false;
      }
    }    
  }

  public void Disconnect(Transform nPC)
  {
    Vagons.Remove(nPC);
    Start();    
  }  
}
