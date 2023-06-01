﻿using Dynamics.Crm.Http.Connector.Core.Domains.Dynamics.Context;
using Dynamics.Crm.Http.Connector.Core.Facades.Requests;
using Dynamics.Crm.Http.Connector.Core.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics.Crm.Http.Connector.Core.Business.Queries
{
    public class DynamicsQueries : IDynamicsQueries
    {
        private readonly IDynamicsRequest _dynamics;

        public DynamicsQueries(IDynamicsRequest dynamics)
        {
            _dynamics = dynamics;
        }


        public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(HttpRequestMessage requestMessage) where TEntity : class, new()
        {
            // Send async request to Dynamics.
            var response = await _dynamics.SendAsync(requestMessage, false);
            // If is not success return a null value.
            if (!response.IsSuccessStatusCode)
                return null;
            // Convert response to JObject.
            var contentResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
            var content = contentResponse.Value<JArray>("value");
            if (content is null || content.Count <= 0)
                return null;
            // Parse content to model.
            var entity = Parse.ParseTEntityToModel<TEntity>(content.FirstOrDefault());
            return entity;
        }

		public async Task<ICollection<TEntity>> ToListAsync<TEntity>(HttpRequestMessage requestMessage) where TEntity : class, new()
        {
            var collection = new List<TEntity>();
			// Send async request to Dynamics.
			var response = await _dynamics.SendAsync(requestMessage, false);
			// If is not success return a null value.
			if (!response.IsSuccessStatusCode)
				return collection;
			// Convert response to JObject.
			var contentResponse = JObject.Parse(await response.Content.ReadAsStringAsync());
			var content = contentResponse.Value<JArray>("value");
			if (content is null || content.Count <= 0)
				return collection;
			// Parse content to model.
			collection = Parse.ParseTEntityToCollection<TEntity>(content).ToList();
			return collection;
		}
	}
}
