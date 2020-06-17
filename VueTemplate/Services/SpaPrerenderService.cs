using System;
using System.Threading.Tasks;
using Jering.Javascript.NodeJS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VueTemplate.Models;

namespace VueTemplate.Services
{
    public interface ISpaPrerenderService
    {
        Task<PrerenderResult> Prerender(SpaPrerenderRequest request);
    }

    public class SpaPrerenderService : ISpaPrerenderService
    {
        private readonly INodeJSService _nodeJsService;
        private readonly IOptionsMonitor<PrerenderOptions> _optionsAccessor;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SpaPrerenderService> _logger;
        private readonly IWebHostEnvironment _environment;

        private PrerenderOptions PrerenderOptions => _optionsAccessor.CurrentValue;

        public SpaPrerenderService(INodeJSService nodeJsService, IOptionsMonitor<PrerenderOptions> optionsAccessor, IMemoryCache cache, ILogger<SpaPrerenderService> logger, IWebHostEnvironment environment)
        {
            _nodeJsService = nodeJsService;
            _optionsAccessor = optionsAccessor;
            _cache = cache;
            _logger = logger;
            _environment = environment;
        }

        public async Task<PrerenderResult> Prerender(SpaPrerenderRequest request)
        {
            var options = PrerenderOptions;

            if (!_cache.TryGetValue(request.CacheIdentifier(), out PrerenderResult result))
            {
                result = await _nodeJsService.InvokeFromFileAsync<PrerenderResult>(options.JsFilePath, "prerender", args: new object[] {request});

                if (options.EnableCaching && options.CacheTimeSeconds > 0)
                {
                    _cache.Set(request.CacheIdentifier(), result, TimeSpan.FromSeconds(options.CacheTimeSeconds));
                }
            }
            else
            {
                _logger.LogInformation("Result from cache");
            }

            return result;
        }
    }

    public class PrerenderOptions
    {
        public bool EnableCaching { get; set; }
        public int CacheTimeSeconds { get; set; }
        public string JsFilePath { get; set; }
    }
}
