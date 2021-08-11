using System.Threading.Tasks;

namespace Ekin.Clarizen.Interfaces
{
    /// <summary>
    /// Interface for Call<T>
    /// </summary>
    /// <typeparam name="T">Response Data Type</typeparam>
    public interface ICall<T> : ICallBase
    {
        /// <summary>
        /// The data returned by the Clarizen API call
        /// </summary>
        T Data { get; set; }
    }

    /// <summary>
    /// ICall Base class
    /// </summary>
    public interface ICallBase
    {
        /// <summary>
        /// Returns true if the Http call to Clarizen was completed successfully, even if the result of the call has errors that Clarizen returned
        /// </summary>
        bool IsCalledSuccessfully { get; set; }

        /// <summary>
        /// Error message returned by Clarizen or the REST adapter
        /// </summary>
        string Error { get; set; }

        /// <summary>
        /// Make a REST call to Clarizen's API
        /// </summary>
        /// <returns>Returns true if the Http call to Clarizen was completed successfully, even if the result of the call has errors that Clarizen returned</returns>
        Task<bool> Execute();
    }
}
