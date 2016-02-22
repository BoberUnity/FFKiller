using UnityEngine;
using System;
using System.Collections.Generic;

public class Party : MonoBehaviour
{
  public List<Transform> vagons = new List<Transform>();
  [SerializeField] private int distance = 40; //diistance = 40 - 1,3 sec
  private Vector3[] trace = new Vector3[0];

  private void Start ()
  {
    Array.Resize(ref trace, vagons.Count * distance + 1);
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
      foreach (var vagon in vagons)
      {
        vagon.position = trace[n * distance];
        n += 1;
      }
    }
  }

  public void Connect(Transform newNPC)
  {
    vagons.Add(newNPC);
    Start();
      //Array.Resize(ref trace, vagons.Count * distance + 1);
  }

  public void Disconnect(Transform nPC)
  {
    vagons.Remove(nPC);
    Start();
    //Array.Resize(ref trace, vagons.Count * distance + 1);
  }  
}
