using System;
using Flux.NewRelic.DeploymentReporter.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Flux.NewRelic.DeploymentReporter.Manager
{
	public class NewRelicMapping
	{
		private IMemoryCache _memoryCache;

		public NewRelicMapping(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache; // = new MemoryCache(new MemoryCacheOptions {ExpirationScanFrequency = TimeSpan.FromDays(1)});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="application">NewRelic Application</param>
		/// <param name="objectName">Liggo Object name</param>
		public void SetMapping(Application application, string objectName)
		{
			var key = $"application_{application.id}";
			_memoryCache.Set(key, new {Application = application, InvolvedObjectName = objectName}, TimeSpan.FromDays(1));
		}
	}
}
