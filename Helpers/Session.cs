using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFK.Models;

namespace WPFK.Helpers
{
    internal class Session
    {
        public static User CurrentUser { get; set; }
    }
}
