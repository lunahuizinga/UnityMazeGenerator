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
    // The prefab we use for building walls
    public GameObject WallPrefab;

    private MazeAlgorithm algorithmInstance;

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
        algorithmInstance = Algorithm switch{
            AlgorithmType.RandomDepthFirst => new RandomDepthFirst(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}