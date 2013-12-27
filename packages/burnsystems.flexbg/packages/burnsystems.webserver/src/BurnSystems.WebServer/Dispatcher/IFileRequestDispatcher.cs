
namespace BurnSystems.WebServer.Dispatcher
{
    /// <summary>
    /// Defines the dispatcher for special file types
    /// </summary>
    public interface IFileRequestDispatcher : IRequestDispatcher
    {
        /// <summary>
        /// Gets or sets the physical path being dispatched
        /// </summary>
        string PhysicalPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the virtual path being dispatched
        /// </summary>
        string VirtualPath
        {
            get;
            set;
        }
    }
}
