﻿using System;
using System.Collections.Generic;
using core.Extensions.Base;

namespace core.Extensions.Data.Mongo
{
    public class Options : IOptions
    {
        public IEnumerable<core.Extensions.Data.DbConnection> Connections { get; set; }
    }
}