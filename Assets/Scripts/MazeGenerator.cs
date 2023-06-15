using System;
using Algorithms;
using UnityEngine;
public class MazeGenerator : MonoBehaviour{
    public enum AlgorithmType{
        RandomDepthFirst
    }
    
    // Sets the size that we want the maze to be
    public Vector2Int Size;
    // The maze generation algorithm to use for generating the maze
    public AlgorithmType Algorithm;
    // The prefab we use for spawning a cell
    public GameObject CellPrefab;

    // Our instance of the selected maze algorithm
    private MazeAlgorithm algorithmInstance;

    // Generate is used to start a new maze generation cycle
    public void Generate(){
        Initialise();
        MazeCell[,] generatedMaze = algorithmInstance.Generate(Size.x, Size.y);
        BuildMaze(generatedMaze);
    }

    // Initialise initialises the necessary variables to the correct values
    private void Initialise(){
        algorithmInstance = Algorithm switch{
            AlgorithmType.RandomDepthFirst => new RandomDepthFirst(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    // Loop through all cells in the array and build the maze
    private void BuildMaze(MazeCell[,] mazeCells){
        GameObject maze = new("Maze");
        foreach (MazeCell mazeCell in mazeCells){
            Vector3 position = new(mazeCell.X, 0, mazeCell.Y);
            GameObject cellObject = Instantiate(CellPrefab, position, Quaternion.Euler(Vector3.zero), maze.transform);
            MazeCellBuilder mazeCellBuilder = cellObject.GetComponent<MazeCellBuilder>();
            mazeCellBuilder.BuildCell(mazeCell);
        }
    }
}