using ChatLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary.Models
{
    public class NewMessageModel
    {
        public Guid ToUserId { get; set; }
        public string Text { get; set; }
    }
}
