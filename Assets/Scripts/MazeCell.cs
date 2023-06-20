// The class representing a single cell of the maze
using System;
using System.Collections.Generic;
public class MazeCell{
    
    // Define the different directions of a cell
    public enum CellDirection{
        PositiveY,
        PositiveX,
        NegativeY,
        NegativeX
    }
    
    // Define the types of cell we can have in a two dimensional maze
    public enum CellType{
        Corner,
        DeadEnd,
        Hallway,
        ThreeWay,
        Intersection,
        Closed
    }
    
    // Every MazeCellSide represents a side of the cell
    public MazeCellSide[] CellSides;
    // Have we visited this cell yet during our maze generation?
    public bool IsVisited;
    // The cell keeps track of its own X and Y coordinates in the maze
    public readonly int X;
    public readonly int Y;
    
    // Define the amount of sides we want for our cell
    private const int SideAmount = 4;

    // In case no cellSides array was provided to the constructor, we call GenerateDefaultCellSides,
    // fill it with new instances, and pass the parameters on to the other constructor
    public MazeCell(int x, int y) : this(x, y, GenerateDefaultCellSides()){}

    // Overload the constructor to add the option of providing an existing array of MazeCellSides
    public MazeCell(int x, int y, MazeCellSide[] cellSides){
        // Copy the provided x and y values into the corresponding variables to be used later
        X = x;
        Y = y;
        
        CellSides = cellSides;
    }
    
    /// <summary>
    /// This method checks the CellSides of the cell and matches it with
    /// the corresponding CellType and accompanying rotation.
    /// </summary>
    /// <returns>A tuple containing the CellType and a float describing the rotation along the Y axis in degrees.</returns>
    /// <exception cref="InvalidOperationException">
    /// This method returns an InvalidOperationException when the pattern of the CellSides
    /// of the cell the method is called on does not correspond to any known state.
    /// </exception>
    public (CellType cellType, float rotationDegrees) GetCellTypeAndRotation(){
        // Convert the IsSolid state of the MazeCellSides into a nibble
        int bitPattern = 0;
        for (int i = 0; i < CellSides.Length; i++){
            if (CellSides[i].IsSolid) bitPattern |= 1 << i;
        }

        return bitPattern switch{
            0b0000 => (CellType.Intersection, 0),
            
            0b0001 => (CellType.ThreeWay, 0),
            0b0010 => (CellType.ThreeWay, 90),
            0b0100 => (CellType.ThreeWay, 180),
            0b1000 => (CellType.ThreeWay, 270),
            
            0b0011 => (CellType.Corner, 90),
            0b0110 => (CellType.Corner, 180),
            0b1100 => (CellType.Corner, 270),
            0b1001 => (CellType.Corner, 0),
            
            0b0101 => (CellType.Hallway, 90),
            0b1010 => (CellType.Hallway, 0),
            
            0b0111 => (CellType.DeadEnd, 90),
            0b1110 => (CellType.DeadEnd, 180),
            0b1101 => (CellType.DeadEnd, 270),
            0b1011 => (CellType.DeadEnd, 0),
            
            0b1111 => (CellType.Closed, 0),
            
            _ => throw new InvalidOperationException("Invalid bit pattern.")
        };
    }

    // Return the MazeCellSide instance corresponding to the CellDirection
    public MazeCellSide GetCellSideInstance(CellDirection direction){
        return CellSides[(int) direction];
    }

    /// <summary>
    /// Attempt to determine the direction of the cell that matches the MazeCellSide provided.
    /// If the MazeCellSide provided is not part of the MazeCell this method returns null.
    /// </summary>
    /// <param name="cellSide">The MazeCellSide instance of which to determine the direction of the cell.</param>
    /// <returns>
    /// The CellDirection of the cell that corresponds to the MazeCellSide passed in.
    /// Will be null if the MazeCellSide is not a part of the MazeCell.
    /// </returns>
    public CellDirection? GetCellSideDirection(MazeCellSide cellSide){
        int index = Array.IndexOf(CellSides, cellSide);
        return index != -1 ? (CellDirection) index : null;
    }
    
    /// <summary>
    /// Get the surrounding neighbours of the specified MazeCell, if they exist.
    /// </summary>
    /// <param name="mazeArray">The two-dimensional maze array that the MazeCell is part of. </param>
    /// <param name="originCell">The MazeCell to get the neighbours of.</param>
    /// <returns>
    /// An array containing the neighbours of the given MazeCell.
    /// The neighbours are given in clockwise order starting at positive Y.
    /// </returns>
    public static IEnumerable<MazeCell> GetNeighbours(MazeCell[,] mazeArray, MazeCell originCell){
        MazeCell[] neighbours = new MazeCell[SideAmount];
        
        // Initialise all the MazeCells in the array with null
        for (int i = 0; i < neighbours.Length; i++){
            neighbours[i] = null;
        }
        
        // Get the size of the maze
        int mazeSizeX = mazeArray.GetLength(0);
        int mazeSizeY = mazeArray.GetLength(1);
        
        // Check to see if the neighbour exists and assign the
        // corresponding index in the array with the neighbour if they do
        if (originCell.Y < (mazeSizeY - 1)) neighbours[0] = mazeArray[originCell.X, originCell.Y + 1];
        if (originCell.X < (mazeSizeX - 1)) neighbours[1] = mazeArray[originCell.X + 1, originCell.Y];
        if (originCell.Y > 0) neighbours[2] = mazeArray[originCell.X, originCell.Y - 1];
        if (originCell.X > 0) neighbours[3] = mazeArray[originCell.X - 1, originCell.Y];
        
        // Return the neighbours
        return neighbours;
    }
    
    /// <summary>
    /// This method will get the CellDirection of the given neighbouring MazeCell.
    /// </summary>
    /// <param name="neighbour">The neighbour to get the direction of.</param>
    /// <returns>The CellDirection to the neighbour relative to the cell the method is called on.</returns>
    public CellDirection GetNeighbourDirection(MazeCell neighbour){
        int deltaX = neighbour.X - X;
        int deltaY = neighbour.Y - Y;

        return deltaX switch{
            0 when deltaY == 1 => CellDirection.PositiveY,
            1 when deltaY == 0 => CellDirection.PositiveX,
            0 when deltaY == -1 => CellDirection.NegativeY,
            -1 when deltaY == 0 => CellDirection.NegativeX,
            _ => throw new ArgumentException("Given MazeCell is not a neighbour!")
        };
    }
    
    /// <summary>
    /// Gets the opposite CellDirection of the given CellDirection.
    /// </summary>
    /// <param name="direction">The CellDirection to get the opposite CellDirection of.</param>
    /// <returns>The opposite CellDirection of the given CellDirection.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Returns an ArgumentOutOfRangeException when the given CellDirection is not valid.
    /// </exception>
    public static CellDirection GetOppositeDirection(CellDirection direction){
        return direction switch{
            CellDirection.PositiveY => CellDirection.NegativeY,
            CellDirection.PositiveX => CellDirection.NegativeX,
            CellDirection.NegativeY => CellDirection.PositiveY,
            CellDirection.NegativeX => CellDirection.PositiveX,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Not a valid direction!")
        };
    }

    // Create a new MazeCellSide array and fill it with new instances of MazeCellSide
    private static MazeCellSide[] GenerateDefaultCellSides(){
        // Initialise the array of MazeCellSides
        MazeCellSide[] cellSides = new MazeCellSide[SideAmount];
        
        // Initialise every MazeCellSide in the array
        for (int i = 0; i < cellSides.Length; i++){
            cellSides[i] = new MazeCellSide();
        }
        
        // Return the newly initialised array
        return cellSides;
    }
}