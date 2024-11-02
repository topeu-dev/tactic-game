using UnityEngine;

public class GenericChar : MonoBehaviour
{
    public int startCellX;
    public int startCellY;


    public void moveToCell(Cell cell)
    {
        Vector3 ignoreY = new Vector3(cell.transform.position.x, transform.position.y, cell.transform.position.z);
        transform.position = ignoreY;
    }
    
    
}