using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Algorithms{
    public class RandomDepthFirst : MazeAlgorithm{
        public override MazeCell[,] Generate(int sizeX, int sizeY){
            Stack<MazeCell> cellStack = new();
            Initialise(sizeX, sizeY);

            MazeCell currentCell = MazeCells[Random.Range(0, sizeX), Random.Range(0, sizeY)];
            currentCell.IsVisited = true;
            cellStack.Push(currentCell);

            while (cellStack.Count > 0){
                currentCell = cellStack.Pop();
                
                IEnumerable<MazeCell> neighbours = MazeCell.GetNeighbours(MazeCells, currentCell);
                MazeCell[] unvisitedNeighbours = neighbours.Where(neighbour => neighbour is { IsVisited: false }).ToArray();

                if (unvisitedNeighbours.Length == 0) continue;
                
                cellStack.Push(currentCell);
                MazeCell chosenNeighbour = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Length)];
                
                MazeCell.CellDirection currentToNeighbour = currentCell.GetNeighbourDirection(chosenNeighbour);
                currentCell.GetCellSideInstance(currentToNeighbour).IsSolid = false;
                
                MazeCell.CellDirection neighbourToCurrent = MazeCell.GetOppositeDirection(currentToNeighbour);
                chosenNeighbour.GetCellSideInstance(neighbourToCurrent).IsSolid = false;

                chosenNeighbour.IsVisited = true;
                cellStack.Push(chosenNeighbour);
            }
            
            return MazeCells;
        }
    }
}