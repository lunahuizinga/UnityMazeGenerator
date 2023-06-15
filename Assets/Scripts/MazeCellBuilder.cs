using System;
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
        
        Instantiate(cellPrefab, transform.position + GetCellPositionOffset(rotation), Quaternion.Euler(0, rotation, 0), transform);
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
            >= 225 and < 315 => new Vector3(1, 0, 0),
            >= 315 or < 45 => new Vector3(0, 0, 0),
            _ => throw new ArgumentException("Invalid rotation.")
        };
    }
}