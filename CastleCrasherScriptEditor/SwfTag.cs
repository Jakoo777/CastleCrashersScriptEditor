using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleCrasherScriptEditor
{
    internal class SwfTag
    {
        public ushort tagType;
        public ulong size;
        public byte[] data;
    }
}
