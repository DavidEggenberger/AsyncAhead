﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Modules.Layers.Domain.Attributes
{
    public class JoiningTableAttribute : Attribute
    {
        public JoiningTableAttribute(params string[] tables)
        {

        }
    }
}
