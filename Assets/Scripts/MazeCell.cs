// The class representing a single cell of the maze
using System;
public class MazeCell{
    
    public enum CellDirection{
        PositiveZ,
        PositiveX,
        NegativeZ,
        NegativeX
    }
    
    public MazeCellSide[] CellSides;
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
}