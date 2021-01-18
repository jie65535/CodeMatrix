using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMatrix
{
    public static class Common
    {
        public static byte[] Codes { get; } = new byte[] { 0x55, 0x1C, 0xBD, 0xE9, 0x7A };

        public static Random Random { get; } = new Random();
    }
}
