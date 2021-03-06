﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Ws.Core.Extensions.Base;

namespace xCore
{
    public class Startup : Ws.Core.Startup<Ws.Core.AppConfig>
    {
        public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration) : base(hostingEnvironment, configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            //services.AddResponseCompression(_ => _.EnableForHttps = true);

            base.ConfigureServices(services);

            Ws.Core.AppInfo<Ws.Core.AppConfig>.Set(env: env, config: config, services: services);
        }

        public override void Configure(IApplicationBuilder app, IOptionsMonitor<Ws.Core.AppConfig> appConfigMonitor, IOptionsMonitor<Ws.Core.Extensions.Base.Configuration> extConfigMonitor, IHostApplicationLifetime applicationLifetime, ILogger<Ws.Core.Program> logger)
        {
            logger.LogInformation("Start");

            //app.UseResponseCompression();

            Ws.Core.AppInfo<Ws.Core.AppConfig>.Set(app: app, appConfigMonitor: appConfigMonitor, extConfigMonitor: extConfigMonitor, loggerFactory: app.ApplicationServices?.GetRequiredService<ILoggerFactory>(), applicationLifetime: applicationLifetime);

            base.Configure(app, appConfigMonitor, extConfigMonitor, applicationLifetime, logger);

            //shutdown
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Shutdown");
            }
            );
        }

    }
}
