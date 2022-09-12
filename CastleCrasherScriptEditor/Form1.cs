using SwfLib;

namespace CastleCrasherScriptEditor
{
    public partial class Form1 : Form
    {
        /*
         * 
         *  THE HOLY BIBLE http://www.doc.ic.ac.uk/lab/labman/swwf/SWFalexref.html
         * 
         * 
         */


        List<SwfTag> tags = new List<SwfTag>();

        SwfTag scriptTag;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = File.ReadAllBytes(@"C:\Users\Jakoo\Desktop\CCMod\MAIN.swf");
            
            SwfReader reader = new SwfReader(data);
            reader.ReadUInt32(); //signature
            reader.ReadUInt32(); //length
            reader.ReadUInt32(); //stuff...
            reader.ReadUInt32(); //stuff...
            reader.ReadUInt32(); //stuff...
            reader.ReadByte(); //stuff...


            for (int i = 0; reader.BaseStream.Position < reader.BaseStream.Length; i++) 
            {
                SwfTag tag = reader.ReadTag();
                if (tag != null) 
                {
                    tag.data = reader.ReadBytes((int)tag.size);
                    //462317 by default. i dont think any other script is that long... Needs error handling.
                    if (tag.tagType == 12 && tag.data.Length >= 400000) 
                    {
                        scriptTag = tag;
                    }
                    tags.Add(tag);
                }
                
            }

            ActionScriptDecrypt parser = new ActionScriptDecrypt(scriptTag.data);

            richTextBox1.Text = parser.finalCode;

        }
    }
}