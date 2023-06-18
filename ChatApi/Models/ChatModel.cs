using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApi.Models;

public class ChatModel
{
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
}
