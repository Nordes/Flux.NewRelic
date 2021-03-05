using System;
using System.Collections.Generic;

namespace Flux.NewRelic.DeploymentReporter.Security
{
	public class ApiKey
	{
		public ApiKey(){}
		public ApiKey(int id, string owner, string key, DateTime created, IReadOnlyCollection<string> roles)
		{
			Id = id;
			Owner = owner ?? throw new ArgumentNullException(nameof(owner));
			Key = key ?? throw new ArgumentNullException(nameof(key));
			Created = created;
			Roles = roles ?? throw new ArgumentNullException(nameof(roles));
		}

		public int Id { get; set; }
		public string Owner { get; set; }
		public string Key { get; set; }
		public DateTime Created { get; set; }
		public IReadOnlyCollection<string> Roles { get; set; }
	}
}