// The class implementing the randomised depth-first search algorithm
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Algorithms{
    public class RandomDepthFirst : MazeAlgorithm{
        /// <summary>
        /// This method generates a maze using the randomised depth-first search algorithm with the size defined by sizeX and sizeY.
        /// </summary>
        /// <param name="sizeX">The size of the maze to generate along the X axis in "maze space".</param>
        /// <param name="sizeY">The size of the maze to generate along the Y axis in "maze space".</param>
        /// <returns>A two-dimensional array of MazeCells containing the generated maze.</returns>
        public override MazeCell[,] Generate(int sizeX, int sizeY){
            Stack<MazeCell> cellStack = new();
            Initialise(sizeX, sizeY);

            // Pick a random cell to be our starting point
            MazeCell currentCell = MazeCells[Random.Range(0, sizeX), Random.Range(0, sizeY)];
            // Mark the current cell as visited
            currentCell.IsVisited = true;
            // Add the current cell to the stack
            cellStack.Push(currentCell);

            while (cellStack.Count > 0){
                // Get the first new cell off the stack
                currentCell = cellStack.Pop();
                
                // Get the current cell's neighbours
                IEnumerable<MazeCell> neighbours = MazeCell.GetNeighbours(MazeCells, currentCell);
                // Filter the neighbours by their visited status to be left with only unvisited neighbours
                MazeCell[] unvisitedNeighbours = neighbours.Where(neighbour => neighbour is { IsVisited: false }).ToArray();
                
                // If we have no unvisited neighbours we go the next iteration of the while loop
                if (unvisitedNeighbours.Length == 0) continue;
                
                // We have unvisited neighbours so add the current cell to the stack for later
                cellStack.Push(currentCell);
                // Pick a random unvisited neighbour
                MazeCell chosenNeighbour = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Length)];
                
                // Get the direction from our current cell to the chosen neighbour defined by a CellDirection
                MazeCell.CellDirection currentToNeighbour = currentCell.GetNeighbourDirection(chosenNeighbour);
                // Set the corresponding MazeCellSide's IsSolid to false, removing the wall
                currentCell.GetCellSideInstance(currentToNeighbour).IsSolid = false;
                
                // Get the direction from our neighbour to our current cell by
                // getting the opposite direction of our current to neighbour direction
                MazeCell.CellDirection neighbourToCurrent = MazeCell.GetOppositeDirection(currentToNeighbour);
                // Set the appropriate MazeCellSide IsSolid to false,
                // removing the wall of our neighbour with the current cell
                chosenNeighbour.GetCellSideInstance(neighbourToCurrent).IsSolid = false;

                // Mark the neighbour as visited
                chosenNeighbour.IsVisited = true;
                // Add the neighbour to the stack
                cellStack.Push(chosenNeighbour);
            }
            
            // This code runs after the while loop so our maze is complete
            // at this point and we simply return the generated array
            return MazeCells;
        }
    }
}