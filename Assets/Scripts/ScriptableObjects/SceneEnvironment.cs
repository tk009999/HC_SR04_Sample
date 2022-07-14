using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "ScriptableObjects/SceneEnvironment", order = 1)]
public class SceneEnvironment : ScriptableObject
{
    public bool IsGameStart;   
}