using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New BoardLevel", menuName = "BoardLevel", order = 51)]
public class BoardSetup : ScriptableObject
{
    public string Name;

    public ArrayLayout BoardLayout;

    public Sprite[] BackgroundSprite=new Sprite[Enum.GetNames(typeof(StaticConfigurate.BackgroundTileType)).Length];

    public Sprite[] TileSprite;

}
