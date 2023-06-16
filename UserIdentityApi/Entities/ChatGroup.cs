﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary.Entities
{
    public class ChatGroup
    {
        [Key]
        public Guid Id { get; set; }
        public List<Message> Messages { get; set; }
        public List<Guid> UserIds { get; set; }
    }
}
