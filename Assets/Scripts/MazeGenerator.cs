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
    // The material to use for the walls of our maze
    public Material MazeMaterial;

    // Our instance of the selected maze algorithm
    private MazeAlgorithm algorithmInstance;
    // The instance of the maze GameObject that we create
    private GameObject maze;

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

    // Set up a new maze GameObject, build the maze mesh, and assign and initialise the proper components
    private void BuildMaze(MazeCell[,] mazeCells){
        if (maze != null){
            if(Application.isPlaying) Destroy(maze);
            else DestroyImmediate(maze);
        }
        maze = new GameObject("Maze");
        
        MazeMeshBuilder mazeMeshBuilder = maze.AddComponent<MazeMeshBuilder>();
        Mesh mazeMesh = mazeMeshBuilder.BuildMesh(mazeCells);
        
        MeshFilter meshFilter = maze.AddComponent<MeshFilter>();
        meshFilter.mesh = mazeMesh;
        
        MeshRenderer meshRenderer = maze.AddComponent<MeshRenderer>();
        meshRenderer.material = MazeMaterial;
    }
}