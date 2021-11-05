using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
	/// <summary>
	/// Provide memory cache support to store url
	/// </summary>
	public class MemoryCacheUrlStorage : IUrlStorageService
	{
		private readonly IMemoryCache _cache;
		private MemoryCacheEntryOptions _cacheOptions;
		private int _id;

		public MemoryCacheUrlStorage(IMemoryCache cache)
		{
			_id = 100;
			_cache = cache;
			_cacheOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
		}

		/// <summary>
		/// Add a url to memory cache.
		/// </summary>
		/// <param name="url"></param>
		public void Add(string url)
		{
			_id = NextId();
			_cache.Set<string>(_id, url, _cacheOptions);
		}

		/// <summary>
		/// Retrieve url from Memory Cache if exists. 
		/// </summary>
		/// <param name="id">Id of the reord</param>
		/// <returns></returns>
		public async Task<string> Get(int id)
		{
			string url = null;
			await Task.Run(() => _cache.TryGetValue<string>(id, out url));
			return url;
		}

		/// <summary>
		/// Return next id of the memory cache
		/// </summary>
		/// <returns>return id</returns>
		public int NextId()
		{
			return _id + 1;
		}
	}
}