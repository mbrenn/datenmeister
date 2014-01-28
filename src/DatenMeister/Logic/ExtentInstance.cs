using System;
namespace DatenMeister.Logic
{
	/// <summary>
	/// Stores the datapool and some additional information
	/// </summary>
	public class ExtentInstance
	{
		/// <summary>
		/// Gets or sets the extent.
		/// </summary>
		/// <value>
		/// The extent.
		/// </value>
		public IURIExtent Extent
		{
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the path, where the extent will be stored. 
		/// This may be a filename or a database connection string. The Path may be null, if it is just stored 
		/// in memory. 
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string Path
		{
			get;
			set;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="DatenMeister.PoolInstance"/> class.
		/// </summary>
		/// <param name='extent'>
		/// Extent.
		/// </param>
		/// <param name='path'>
		/// Path.
		/// </param>
		public ExtentInstance (IURIExtent extent, string path)
		{
			this.Extent = extent;
			this.Path = path;
		}
	}
}

