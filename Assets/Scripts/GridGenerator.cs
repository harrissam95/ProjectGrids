using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector3Int grid;
    public GameObject tile1, tile2;
    // Start is called before the first frame update
    void Start()
    {
        int flag = 0;
        //create int from current position of the grid
        int posX = ((int)this.transform.position.x);
        int posZ = ((int)this.transform.position.z);
        int gridX, gridZ;

        if(posX < 0 && posZ < 0)
        {
            gridX = ((int)grid.x) - (((int)this.transform.position.x) * (-1));
            gridZ = ((int)grid.z) - (((int)this.transform.position.z) * (-1));
        }
        else
        {
            gridX = ((int)grid.x) - ((int)this.transform.position.x);
            gridZ = ((int)grid.z) - ((int)this.transform.position.z);
        }

        for(int i = posX; i<gridX; i++)
        {
            //added this if statement to fix checkerboard when z is even
            if(grid.z % 2 == 0)
            {
                flag++;
            }
            for(int j = posZ; j<gridZ; j++)
            {
                Vector3 pos = new Vector3(i * 1f, 0f, j * 1f);
                if (flag % 2 == 0)
                {
                    Instantiate(tile1, pos, Quaternion.identity);
                    flag++;
                }
                else
                {
                    Instantiate(tile2, pos, Quaternion.identity);
                    flag++;
                }
            }
        }
    }

}