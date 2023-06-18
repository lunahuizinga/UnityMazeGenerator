using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MazeMeshBuilder : MonoBehaviour{
    
    // The Mesh to use for the walls of the maze
    public Mesh WallMesh;
    // The pivot point to rotate the walls around.
    // Our built-in mesh faces forward in a 1x1x1 cube so we rotate from the middle
    public Vector3 Pivot = new(.5f, 0, .5f);
    
    // We call Initialise on Awake to check if WallMesh is valid and provide it with the default mesh otherwise
    private void Awake() => Initialise();

    // We have to call Initialise in Reset in order to be able to generate mazes in the editor
    private void Reset() => Initialise();
    
    // Initialise WallMesh with a simple built-in mesh if no mesh was provided
    private void Initialise(){
        if (WallMesh != null) return;
        WallMesh = new Mesh{
            name = "Wall Mesh",
            vertices = new Vector3[]{
                new(0, 0, .9f), // The front quad
                new(0, 1, .9f),
                new(1, 1, .9f),
                new(1, 0, .9f),
            
                new(1, 0, 1), // The back quad
                new(1, 1, 1),
                new(0, 1, 1),
                new(0, 0, 1)
            },
            triangles = new[]{
                0, 1, 2, // Front
                0, 2, 3,
            
                3, 2, 5, // Right Side
                3, 5, 4,
            
                4, 5, 6, // Back
                4, 6, 7,
            
                7, 6, 1, // Left Side
                7, 1, 0,
            
                7, 0, 3, // Bottom
                7, 3, 4,
            
                1, 6, 5, // Top
                1, 5, 2
            },
            normals = new[]{
                Vector3.back, // Our front face (backward facing relative to world)
                Vector3.back,
                Vector3.back,
                Vector3.back,
            
                Vector3.forward, // Our back face (forward facing relative to world)
                Vector3.forward,
                Vector3.forward,
                Vector3.forward
            }
        };
    }
    
    /// We build the mesh of the maze by going over every MazeCell in the array and building the individual mesh of the cell.
    /// We create a new translation matrix to move the Mesh of the cell to its position in the maze,
    /// and we add this together with the mesh of the cell in a new CombineInstance.
    /// After this it's as simple as just combining all the CombineInstances in a single mesh and returning that.
    /// <summary>
    /// Build the mesh of a given maze defined by a two-dimensional array of MazeCells.
    /// </summary>
    /// <param name="mazeCells">The MazeCell array to build the mesh of.</param>
    /// <returns>The built mesh.</returns>
    public Mesh BuildMesh(MazeCell[,] mazeCells){
        List<CombineInstance> combineInstances = new();
        
        foreach (MazeCell mazeCell in mazeCells){
            if (BuildCellMesh(mazeCell) is not Mesh cellMesh) continue;
            
            Vector3 cellLocation = new(mazeCell.X, 0, mazeCell.Y);
            Matrix4x4 cellMatrix = transform.localToWorldMatrix * Matrix4x4.Translate(cellLocation);
            
            CombineInstance combineInstance = new(){
                mesh = cellMesh,
                transform = cellMatrix
            };
            
            combineInstances.Add(combineInstance);
        }

        Mesh gridMesh = new();
        gridMesh.name = "Maze Mesh";
        gridMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        gridMesh.CombineMeshes(combineInstances.ToArray());
        return gridMesh;
    }
    
    /// We check every side of the given cell to see if it's solid.
    /// If it is solid then we create a new mesh by cloning the origin mesh (WallMesh),
    /// rotate it according to the MazeCellSide in question in relation to the MazeCell,
    /// and add it to the list of walls to be combined in a single Mesh.
    /// After which we return the combined Mesh.
    /// <summary>
    /// Build the mesh of a given cell by combining the proper rotated Meshes.
    /// </summary>
    /// <param name="mazeCell">The MazeCell to build the Mesh of.</param>
    /// <returns>The built Mesh of the given cell.</returns>
    private Mesh BuildCellMesh(MazeCell mazeCell){
        List<CombineInstance> combineInstances = new();
        
        foreach (MazeCellSide cellSide in mazeCell.CellSides){
            if (!cellSide.IsSolid) continue;
            if (mazeCell.GetCellSideDirection(cellSide) is not MazeCell.CellDirection direction) continue;
            
            Mesh cellSideMesh = CloneMesh(WallMesh);
            
            float rotation = GetWallRotation(direction);
            cellSideMesh = RotateMeshAroundPoint(cellSideMesh, Pivot, Quaternion.AngleAxis(rotation, Vector3.up));
            
            CombineInstance combineInstance = new(){
                mesh = cellSideMesh,
                transform = transform.localToWorldMatrix
            };
            
            combineInstances.Add(combineInstance);
        }
        
        if (combineInstances.Count == 0) return null;
        
        Mesh cellMesh = new();
        cellMesh.CombineMeshes(combineInstances.ToArray());
        return cellMesh;
    }

    /// <summary>
    /// This method gets the corresponding wall rotation value from a CellDirection.
    /// </summary>
    /// <param name="cellDirection">The CellDirection to get the rotation for.</param>
    /// <returns>A float representing the Y rotation in degrees of the corresponding CellDirection.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when cellDirection is not a valid known CellDirection.
    /// </exception>
    private static float GetWallRotation(MazeCell.CellDirection cellDirection){
        return cellDirection switch{
            MazeCell.CellDirection.PositiveY => 0,
            MazeCell.CellDirection.PositiveX => 90,
            MazeCell.CellDirection.NegativeY => 180,
            MazeCell.CellDirection.NegativeX => 270,
            _ => throw new ArgumentOutOfRangeException(nameof(cellDirection), "Invalid CellDirection!")
        };
    }
    
    /// <summary>
    /// This method simply returns a new mesh with the same data as the mesh provided.
    /// </summary>
    /// <param name="original">The mesh to clone.</param>
    /// <returns>A copy of the provided mesh.</returns>
    private static Mesh CloneMesh(Mesh original){
        return new Mesh(){
            name = original.name,
            vertices = original.vertices,
            triangles = original.triangles,
            normals = original.normals,
            tangents = original.tangents,
            uv = original.uv,
            colors = original.colors,
            colors32 = original.colors32,
            bounds = original.bounds
        };
    }
    
    /// For vertices and normals we simply go over all the Vector3's and rotate them with RotateAroundPoint.
    /// For the tangents we have to do some reformatting since they are essentially a Vector3 with an extra
    /// component defining if they are forward or backward facing. We can take the Vector3 portion, rotate it,
    /// and then simply add the w component back on as to keep its proper direction.
    /// <summary>
    /// Rotate a mesh around a point in space.
    /// </summary>
    /// <param name="mesh">The mesh to rotate.</param>
    /// <param name="pivot">The point to rate around.</param>
    /// <param name="rotation">The rotation to apply to the mesh.</param>
    /// <returns>The rotated mesh.</returns>
    private static Mesh RotateMeshAroundPoint(Mesh mesh, Vector3 pivot, Quaternion rotation){
        mesh.vertices = mesh.vertices.Select(vertex => RotateAroundPoint(vertex, pivot, rotation)).ToArray();
        mesh.normals = mesh.normals.Select(normal => RotateAroundPoint(normal, pivot, rotation)).ToArray();

        mesh.tangents = (from tangent in mesh.tangents
            let tangentLocation = new Vector3(tangent.x, tangent.y, tangent.z)
            let rotatedTangent = RotateAroundPoint(tangentLocation, pivot, rotation)
            select new Vector4(rotatedTangent.x, rotatedTangent.y, rotatedTangent.z, tangent.w)).ToArray();
        
        return mesh;
    }
    
    /// Rotate the given Vector3 point around pivot by first subtracting pivot from point
    /// so our translation is now relative to pivot. After this we can rotate by rotation to rotate around pivot.
    /// Finally, we add pivot back to point to our original position, now rotated 'rotation' around the point 'pivot'.
    /// <summary>
    /// Rotates a point a certain amount around another point in space.
    /// </summary>
    /// <param name="point">The point to rotate.</param>
    /// <param name="pivot">The point to rotate around.</param>
    /// <param name="rotation">The rotation to apply to the point.</param>
    /// <returns>The rotated point.</returns>
    private static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion rotation){
        Vector3 direction = point - pivot;
        Vector3 rotatedPoint = rotation * direction;
        return rotatedPoint + pivot;
    }

    // Draw a sphere to more easily see the origin point of the maze in the editor
    private void OnDrawGizmos(){
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, .5f);
    }
}