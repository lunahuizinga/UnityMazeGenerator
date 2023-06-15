using System;
using System.Linq;
using UnityEngine;
public class MazeCellBuilder : MonoBehaviour{
    
    // The prefabs to use and instantiate for each type of cell
    public GameObject CornerPrefab;
    public GameObject DeadEndPrefab;
    public GameObject HallwayPrefab;
    public GameObject ThreeWayPrefab;
    public GameObject ClosedOffPrefab;
    
    /// <summary>
    /// Spawn a new Maze Cell instance in world corresponding to the given MazeCell.
    /// </summary>
    /// <param name="x">The size of the maze along the "horizontal" axis.</param>
    /// <param name="y">The size of the maze along the "vertical" axis.</param>
    /// <param name="mazeCell">The instance of the MazeCell to construct in the world.</param>
    public void BuildCell(MazeCell mazeCell){
        (MazeCell.CellType type, float rotation) = mazeCell.GetCellTypeAndRotation();
        if (type == MazeCell.CellType.Intersection) return;

        GameObject cellPrefab = type switch{
            MazeCell.CellType.Corner => CornerPrefab,
            MazeCell.CellType.DeadEnd => DeadEndPrefab,
            MazeCell.CellType.Hallway => HallwayPrefab,
            MazeCell.CellType.ThreeWay => ThreeWayPrefab,
            MazeCell.CellType.Closed => ClosedOffPrefab,
            _ => throw new ArgumentException("Invalid MazeCell.")
        };

        Instantiate(cellPrefab, GetCellPositionOffset(rotation), Quaternion.Euler(0, rotation, 0), transform);
    }
    
    /// <summary>
    /// This method gets the offset to use for a certain rotation when placing a cell.
    /// </summary>
    /// <param name="rotationDegrees">
    /// The rotation of the MazeCell in degrees.
    /// This is expected to be in positive 90 degree angles (0, 90, 180, or 270).
    /// </param>
    /// <returns>A Vector3 that offsets the given rotation.</returns>
    /// <exception cref="ArgumentException"></exception>
    private static Vector3 GetCellPositionOffset(float rotationDegrees){
        return (rotationDegrees % 360) switch{
            >= 45 and < 135 => new Vector3(0, 0, 1),
            >= 135 and < 225 => new Vector3(1, 0, 1),
            >= 225 and < 315 => new Vector3(1, 0, 1),
            >= 315 or < 45 => new Vector3(0, 0, 0),
            _ => throw new ArgumentException("Invalid rotation.")
        };
    }
    
    /// <summary>
    /// Get the matching CellType for a given MazeCell instance.
    /// </summary>
    /// <param name="mazeCell">The MazeCell instance to get the CellType of.</param>
    /// <returns>
    /// The CellType that matches the walls of the MazeCell.
    /// Will be null if no matching CellType can be found.
    /// </returns>
    private static MazeCell.CellType? GetCellType(MazeCell mazeCell){
        // Count the amount of walls we need in our prefab by checking the
        // amount of MazeCellSides in our MazeCell that have IsSolid set to true
        int wallAmount = mazeCell.CellSides.Count(cellSide => cellSide.IsSolid);
        
        // We can tell what kind of cell we have depending on the amount of walls it has, except for
        // when we have two walls, in which case we should check if the walls are consecutive (next to each other).
        return wallAmount switch{
            0 => MazeCell.CellType.Intersection,
            1 => MazeCell.CellType.ThreeWay,
            2 => HasConsecutiveWalls(mazeCell, 2) ? MazeCell.CellType.Corner : MazeCell.CellType.Hallway,
            3 => MazeCell.CellType.DeadEnd,
            4 => MazeCell.CellType.Closed,
            _ => null
        };
    }
    
    /// <summary>
    /// Loops through the MazeCell's CellSides array and counts the amount
    /// of consecutive walls it encounters. If this is more than the provided
    /// consecutiveWallAmount this method returns true, otherwise false.
    /// </summary>
    /// <param name="mazeCell">The MazeCell instance to count the walls of.</param>
    /// <param name="consecutiveWallAmount">The amount of consecutive walls the MazeCell should have.</param>
    /// <returns>Returns true if the cell has at least the specified amount of consecutive walls.</returns>
    protected static bool HasConsecutiveWalls(MazeCell mazeCell, int consecutiveWallAmount){
        int consecutiveWalls = 0;
        foreach (MazeCellSide cellSide in mazeCell.CellSides.Concat(mazeCell.CellSides)){
            if (cellSide.IsSolid) consecutiveWalls++;
            else consecutiveWalls = 0;
            if (consecutiveWalls >= consecutiveWallAmount) return true;
        }
        return false;
    }

    // Start is called before the first frame update
    private void Start(){
    }

    // Update is called once per frame
    private void Update(){
    }
}