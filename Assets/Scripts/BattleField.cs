using UnityEngine;
using UnityEngine.EventSystems;

public class BattleField : MonoBehaviour
{
    [SerializeField]
    public GameObject cellPrefab;

    public int rows = 4;
    public int columns = 4;
    public float cellSize = 12f;

    public InputController dermo;

    private Cell[,] _matrix;

    private void Awake()
    {
        _matrix = new Cell[rows, columns];
        GenerateGrid();
    }

    public void OnPointerClick(GameObject cellRef)
    {
        dermo.HandleClickOnObject(cellRef);
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize * 1.05f);

                GameObject cellObj = Instantiate(cellPrefab, position, cellPrefab.transform.rotation);
                cellObj.name = cellPrefab.name + x + "x" + z;
                addEventTrigger(cellObj);

                Cell cell = cellObj.GetComponent<Cell>();

                cell.x = x;
                cell.y = z;
                _matrix[x, z] = cell;
            }
        }
    }

    public void addEventTrigger(GameObject cellRef)
    {
        EventTrigger eventTrigger = cellRef.AddComponent<EventTrigger>();

        EventTrigger.Entry clickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        // Добавляем метод, который будет вызван при клике
        clickEntry.callback.AddListener((some) =>  OnPointerClick(cellRef));

        // Добавляем это событие к EventTrigger
        eventTrigger.triggers.Add(clickEntry);
        
    }
}