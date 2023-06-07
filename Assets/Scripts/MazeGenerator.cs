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

    private MazeCell[,] mazeCells;
    private IMazeAlgorithm algorithmInstance;

    // Start is called before the first frame update
    private void Start(){
    }

    // Update is called once per frame
    private void Update(){
    }

    // Generate is used to start a new maze generation cycle
    public void Generate(){
    }

    // Initialise initialises the necessary variables to the correct values
    private void Initialise(){
        InitialiseCells();
    }

    private void InitialiseCells(){
        // Initialise the two dimensional cell array with the dimensions set by the user
        mazeCells = new MazeCell[Size.x, Size.y];
        
        // Loop through the array and initialise every cell in the array
        for (int x = 0; x < mazeCells.GetLength(0); x++)
        for (int y = 0; y < mazeCells.GetLength(1); y++){
            MazeCell mazeCell = new();
            mazeCells[x, y] = mazeCell;
        }
    }

    private void InitialiseAlgorithm(){
        algorithmInstance = Algorithm switch{
            AlgorithmType.RandomDepthFirst => new RandomDepthFirst(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}