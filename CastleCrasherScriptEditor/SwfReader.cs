using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleCrasherScriptEditor
{
    internal class SwfReader : BinaryReader
    {
        public SwfReader(Stream s) : base(s)
        {
        }

        public SwfReader(byte[] bytes) : base(new MemoryStream(bytes))
        {
        }

        public SwfReader(byte[] bytes, uint position, uint size)
            : base(new MemoryStream(bytes, (int)position, (int)size))
        {
        }

        public SwfTag ReadTag() 
        {
            //Castle crasher SWF has a problem?
            try
            {
                SwfTag tag = new SwfTag();

                ushort f_tag_and_size = ReadUInt16();
                ushort f_tag = (ushort)(f_tag_and_size >> 6);
                tag.tagType = f_tag;

                ushort f_tag_data_size = (ushort)(f_tag_and_size & 0x3F);

                if (f_tag_data_size == 63)
                {
                    ulong l = ReadUInt32();
                    tag.size = l;
                }
                else
                {
                    ushort f_tag_data_real_size = f_tag_data_size;
                    tag.size = f_tag_data_real_size;
                }

                return tag;
            }
            catch 
            {
                return null;
            }
            
        }

        public string ReadCharString() 
        {
            var builder = new StringBuilder();

            while (true)
            {
                char c = (char)ReadByte();
                if (c != 0x0)
                {
                    builder.Append(c);
                }
                else 
                {
                    return builder.ToString();
                }
            }


        }
        
    }
}
