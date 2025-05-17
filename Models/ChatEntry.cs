using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ChatEntry
    {
        public MensajeDto ? mensaje { get; set; }
        public List<string>? ui_usuarios { get; set; }
    }
}
