using System.Collections.Generic;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Interface for access to voxelmap
    /// </summary>
    public interface IVoxelMap
    {
        /// <summary>
        /// Gets the information about the voxelmap
        /// </summary>
        VoxelMapInfo GetInfo(long instanceId);

        /// <summary>
        /// Gets a value, if map has been created
        /// </summary>
        bool IsMapCreated(long instanceId);

        /// <summary>
        /// Creates a simple mal just containing air
        /// </summary>
        void CreateMap(long instanceId, VoxelMapInfo info);
            
        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        void SetFieldType(long instanceId, int x, int y, byte fieldType, float startingHeight, float endingHeight);
            
        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="instanceId">Id of the instance whose information shall be retrieved</param>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        byte GetFieldType(long instanceId, int x, int y, float height);

        /// <summary>
        /// Sets the data for one column
        /// </summary>
        /// <param name="instanceId">Id of the instance whose column shall be modified</param>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        /// <param name="key">Key of the data</param>
        /// <param name="data">Data to be assinged</param>
        void SetData(long instanceId, int x, int y, int key, byte[] data);

        /// <summary>
        /// Gets the data for one column
        /// </summary>
        /// <param name="instanceId">Id of the instance whose column shall be read-out</param>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        /// <param name="key">Key of the data</param>
        byte[] GetData(long instanceId, int x, int y, int key);

        /// <summary>
        /// Gets a complete column of a certain position
        /// </summary>
        /// <param name="x">X-Coordinate of the Position</param>
        /// <param name="y">Y-Coordinate of the position</param>
        /// <returns>Queried column</returns>
        VoxelMapColumn GetColumn(long instanceId, int x, int y);

        /// <summary>
        /// Sets a complete column of a certain position
        /// </summary>
        /// <param name="x">Absolute X-Coordinate of the requested column</param>
        /// <param name="y">Absolute Y-Coordinate of the requested column</param>
        /// <param name="column">Column to be queried</param>
        void SetColumn(long instanceId, int x, int y, VoxelMapColumn column);
            
        /// <summary>
        /// Gets the surface information
        /// </summary>
        /// <param name="x1">Upper left X-Coordinate</param>
        /// <param name="y1">Upper left Y-Coordinate</param>
        /// <param name="x2">Lower right X-Coordinate</param>
        /// <param name="y2">Lower right Y-Coordinate</param>
        /// <returns>Information about the fieldtypes. This array is starting with 0 relative to x1/y1.First coordinate is x-coordinate</returns>
        FieldTypeChangeInfo[][] GetSurfaceInfo(long instanceId, int x1, int y1, int x2, int y2);
    }
}
