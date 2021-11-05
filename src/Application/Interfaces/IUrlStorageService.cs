using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IUrlStorageService
	{
		/// <summary>
		/// Add a url to storage
		/// </summary>
		/// <param name="url"></param>
		/// <returns>id for the url.</returns>
		void Add(string url);

		/// <summary>
		/// Get the url by its id if exists
		/// </summary>
		/// <param name="id">Id of the url</param>
		/// <returns>Url</returns>
		Task<string> Get(int id);

		/// <summary>
		/// Return next id of the memory cache
		/// </summary>
		/// <returns>return id</returns>
		int NextId();
	}
}
