﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Ws.Core.Extensions.Routing
{
    public class Extension : Base.Extension
    {
        //private Options _options => GetOptions<Options>();
        public override void Execute(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {            
        }
        public override void Execute(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {         
            applicationBuilder.UseRouting();
        }
    }
}
