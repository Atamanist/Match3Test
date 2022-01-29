using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct rowData
    {
        public StaticConfigurate.BackgroundTileType[] row;
    }

    public rowData[] rows=new rowData[20];

}
