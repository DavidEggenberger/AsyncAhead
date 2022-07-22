﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShared.Features.Channel
{
    public class ChannelDTO : BaseDTO
    {
        public string Name { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}