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
    // Whether or not we should move the camera to the newly generated maze
    public bool FocusCameraOnMaze = true;
    // The camera to focus on the maze
    public Camera MazeCamera;

    // Public setter for the X component of Size
    public void SetMazeSizeX(int sizeX) => Size = new Vector2Int(sizeX, Size.y);
    
    // Create an overload of SetMazeSizeX in order for us to use it in the UI
    public void SetMazeSizeX(float sizeX) => SetMazeSizeX((int) sizeX);
    
    // Public setter for the Y component of Size
    public void SetMazeSizeY(int sizeY) => Size = new Vector2Int(Size.x, sizeY);

    // Create an overload of SetMazeSizeY in order for us to use it in the UI
    public void SetMazeSizeY(float sizeY) => SetMazeSizeY((int) sizeY);
    
    // Public setter for Size
    public void SetMazeSize(Vector2Int size) => Size = size;
    
    // Public getter for maze
    public GameObject GetCurrentMaze() => maze;

    // Generate is used to start a new maze generation cycle
    public void Generate(){
        // Initialise the algorithmInstance with our chosen algorithm
        Initialise();
        // Generate a new maze using the chosen algorithm
        MazeCell[,] generatedMaze = algorithmInstance.Generate(Size.x, Size.y);
        // Build the newly generated maze
        BuildMaze(generatedMaze);

        if (!FocusCameraOnMaze) return;
        Camera cameraToFocus = MazeCamera != null ? MazeCamera : Camera.main;
        if (cameraToFocus == null) return;
        CameraObjectFocus cameraFocus = cameraToFocus.GetComponent<CameraObjectFocus>();
        cameraFocus.FocusOnObject(maze);
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
        // Check to see if our maze instance is already assigned and destroy it if it is
        if (maze != null){
            if(Application.isPlaying) Destroy(maze);
            else DestroyImmediate(maze);
        }
        // Assign a new GameObject to maze
        maze = new GameObject("Maze");
        
        // Add a new MazeMeshBuilder to maze
        MazeMeshBuilder mazeMeshBuilder = maze.AddComponent<MazeMeshBuilder>();
        // Generate the maze mesh
        Mesh mazeMesh = mazeMeshBuilder.BuildMesh(mazeCells);
        
        // Add a new MeshFilter to the maze GameObject
        MeshFilter meshFilter = maze.AddComponent<MeshFilter>();
        // Assign the newly generated maze mesh to the MeshFilter
        meshFilter.mesh = mazeMesh;
        
        // Add a new MeshRenderer to the maze GameObject
        MeshRenderer meshRenderer = maze.AddComponent<MeshRenderer>();
        // Assign our chosen material to the MeshRenderer
        meshRenderer.material = MazeMaterial;
    }
}