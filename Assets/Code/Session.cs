using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

//TODO: Maybe we move the connection and other Singleton-like things over here
public class Session : Singleton<Session> 
{
    protected Session() {}
    // Inventory
    public IList<SpeciesModel> Species = new List<SpeciesModel>();
}