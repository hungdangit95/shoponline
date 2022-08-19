using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using ShopOnline.Api.Servers.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopOnline.Api.Servers.Admin
{
    public class TodosController : V1Controller
    {
        List<string> todos = new List<string> { "shopping", "Watch Movie", "Gardening" };
        private readonly IDistributedCache _distributedCache;
        public TodosController(IDistributedCache distributedCache, IStringLocalizer<HomeController> localizer) : base(localizer)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            List<string> myTodos = new List<string>();
            bool IsCached = false;
            string cachedTodosString = string.Empty;
            cachedTodosString = await _distributedCache.GetStringAsync("_todos");
            if (!string.IsNullOrEmpty(cachedTodosString))
            {
                // loaded data from the redis cache.
                myTodos = JsonSerializer.Deserialize<List<string>>(cachedTodosString);
                IsCached = true;
            }
            else
            {
                // loading from code (in real-time from database)
                // then saving to the redis cache 
                myTodos = todos;
                IsCached = false;
                cachedTodosString = JsonSerializer.Serialize<List<string>>(todos);
                await _distributedCache.SetStringAsync("_todos", cachedTodosString);
            }
            return Ok(new { IsCached, myTodos });
        }
    }
}
