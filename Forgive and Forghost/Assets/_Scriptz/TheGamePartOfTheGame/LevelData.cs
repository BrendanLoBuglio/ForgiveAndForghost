using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scriptz.TheGamePartOfTheGame;

[CreateAssetMenu(fileName = "LevelData_LevelName", menuName = "ScriptableObjects", order = 1)]
public class LevelData : ScriptableObject
{
	public string levelName;
	public Mission[] missions;
}
