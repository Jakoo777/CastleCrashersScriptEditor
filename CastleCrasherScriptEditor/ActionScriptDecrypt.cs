using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleCrasherScriptEditor
{
    public class ActionScriptParam 
    {
        public string name;
        public byte register;
    }

    internal class ActionScriptDecrypt
    {
        byte[] data;
        SwfReader reader;

        List<string> constants = new List<string>();

        public string finalCode;

        public ActionScriptDecrypt(byte[] data)
        {
            this.data = data;

            reader = new SwfReader(this.data);

            ParseAllData(reader, 0);
        }

        private void ParseAllData(SwfReader read, int indentation)
        {
            try
            {
                int instructionNum = 0;
                for (int i = 0; read.BaseStream.Position <= read.BaseStream.Length; i++)
                {
                    
                    if (!ReadInstruction(read, read.ReadByte(), indentation, instructionNum))
                    {
                        read.BaseStream.Position = read.BaseStream.Length + 2;
                    }

                    instructionNum++;
                }
            }
            catch 
            {
                
            }
            
            
        }

        private bool ReadInstruction(SwfReader read, byte instr, int indentation, int instructionNum) 
        {
            int offset = (int)read.BaseStream.Position-1;
            switch (instr) 
            {
                case 0x0:
                    GENERICFUNC(read, indentation, instructionNum, offset, "End");
                    break;
                case 0x4:
                    GENERICFUNC(read, indentation, instructionNum, offset, "NextFrame");
                    break;
                case 0x5:
                    GENERICFUNC(read, indentation, instructionNum, offset, "PreviousFrame");
                    break;
                case 0x6:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Play");
                    break;
                case 0x7:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Stop");
                    break;
                case 0xA:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Add");
                    break;
                case 0xB:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Subtract");
                    break;
                case 0xC:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Multiply");
                    break;
                case 0xD:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Divide");
                    break;
                case 0xE:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Equal_0xE");
                    break;
                case 0xF:
                    GENERICFUNC(read, indentation, instructionNum, offset, "LessThan");
                    break;
                case 0x10:
                    GENERICFUNC(read, indentation, instructionNum, offset, "LogicalAnd");
                    break;
                case 0x11:
                    GENERICFUNC(read, indentation, instructionNum, offset, "LogicalOr");
                    break;
                case 0x12:
                    Not(read, indentation, instructionNum, offset);
                    break;
                case 0x13:
                    GENERICFUNC(read, indentation, instructionNum, offset, "StringEqual");
                    break;
                case 0x14:
                    GENERICFUNC(read, indentation, instructionNum, offset, "StringLength");
                    break;
                case 0x15:
                    GENERICFUNC(read, indentation, instructionNum, offset, "SubString");
                    break;
                case 0x17:
                    Pop(read, indentation, instructionNum, offset);
                    break;
                case 0x18:
                    IntegralPart(read, indentation, instructionNum, offset);
                    break;
                case 0x1C:
                    GetVariable(read, indentation, instructionNum, offset);
                    break;
                case 0x1D:
                    SetVariable(read, indentation, instructionNum, offset);
                    break;
                case 0x23:
                    GENERICFUNC(read, indentation, instructionNum, offset, "SetProperty");
                    break;
                case 0x25:
                    GENERICFUNC(read, indentation, instructionNum, offset, "RemoteSprite");
                    break;
                case 0x26:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Trace");
                    break;
                case 0x30:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Random");
                    break;
                case 0x34:
                    GENERICFUNC(read, indentation, instructionNum, offset, "GetTimer");
                    break;
                case 0x3D:
                    CallFunction(read, indentation, instructionNum, offset);
                    break;
                case 0x3E:
                    Return(read, indentation, instructionNum, offset);
                    break;
                case 0x3F:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Modulo");
                    break;
                case 0x40:
                    GENERICFUNC(read, indentation, instructionNum, offset, "New");
                    break;
                case 0x47:
                    Add(read, indentation, instructionNum, offset);
                    break;
                case 0x48:
                    GENERICFUNC(read, indentation, instructionNum, offset, "LessThan");
                    break;
                case 0x49:
                    Equal(read, indentation, instructionNum, offset);
                    break;
                case 0x4C:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Duplicate");
                    break;
                case 0x4E:
                    GetMember(read, indentation, instructionNum, offset);
                    break;
                case 0x4F:
                    SetMember(read, indentation, instructionNum, offset);
                    break;
                case 0x50:
                    Increment(read, indentation, instructionNum, offset);
                    break;
                case 0x51:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Decrement");
                    break;
                case 0x52:
                    CallMethod(read, indentation, instructionNum, offset);
                    break;
                case 0x60:
                    GENERICFUNC(read, indentation, instructionNum, offset, "And");
                    break;
                case 0x61:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Or");
                    break;
                case 0x62:
                    GENERICFUNC(read, indentation, instructionNum, offset, "Xor");
                    break;
                case 0x63:
                    GENERICFUNC(read, indentation, instructionNum, offset, "ShiftLeft");
                    break;
                case 0x64:
                    GENERICFUNC(read, indentation, instructionNum, offset, "ShiftRight");
                    break;
                case 0x65:
                    GENERICFUNC(read, indentation, instructionNum, offset, "ShiftRightUnsigned");
                    break;
                case 0x66:
                    StrictEqual(read, indentation, instructionNum, offset);
                    break;
                case 0x67:
                    GreaterThan(read, indentation, instructionNum, offset);
                    break;
                case 0x70:
                    Unknown70(read, indentation, instructionNum, offset);
                    break;
                case 0x87:
                    StoreRegister(read, indentation, instructionNum, offset);
                    break;
                case 0x88:
                    ConstantPool(read, indentation, instructionNum, offset);
                    break;
                case 0x8C:
                    GotoLabel(read, indentation, instructionNum, offset);
                    break;
                case 0x8E:
                    DefineFunction2(read, indentation, instructionNum, offset);
                    break;
                case 0x96:
                    Push(read, indentation, instructionNum, offset);
                    break;
                case 0x99:
                    Goto(read, indentation, instructionNum, offset);
                    break;
                case 0x9A:
                    GetUrl2(read, indentation, instructionNum, offset);
                    break;
                case 0x9B:
                    DefineFunction(read, indentation, instructionNum, offset);
                    break;
                case 0x9D:
                    If(read, indentation, instructionNum, offset);
                    break;
                //???
                case 0xA0:
                    Push(read, indentation, instructionNum, offset);
                    break;
                case 0xA1:
                    Push(read, indentation, instructionNum, offset);
                    break;
                case 0xA2:
                    Push(read, indentation, instructionNum, offset);
                    break;
                case 0xA3:
                    Push(read, indentation, instructionNum, offset);
                    break;
                default:
                    finalCode = AddWithIndentation(finalCode, "UNK_" + instr.ToString("X4") + "\n", indentation, instructionNum, offset);
                    return false;
            }

            return true;
        }

        private string AddWithIndentation(string str, string add, int indentation, int instructionNum, int offset) 
        {
            if (instructionNum != -1)
                str += "[" + instructionNum.ToString("D4") + "]";
            if (offset != -1)
                str += "[" + offset.ToString("X8") + "]";

            for (int i = 0; i < indentation; i++) 
            {
                str += "      ";
            }
            str += add;
            return str;
        }

        private void ConstantPool(SwfReader read, int indentation, int instructionNum, int offset) 
        {
            uint len = read.ReadUInt16();
            uint constLen = read.ReadUInt16(); //this is the amount of entries? potentially a good idea to error check? Probably when error handling is a thing.

            for (int i = 0; i < constLen; i++) 
            {
                this.constants.Add(read.ReadCharString());
            }



            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "ConstantPool {\n", indentation, instructionNum, offset);
            for (int i = 0; i < constants.Count(); i++) 
            {
                returnStr = AddWithIndentation(returnStr, "\"" + constants[i] + "\"\n", indentation + 1, -1, -1);
            }
            returnStr = AddWithIndentation(returnStr, "}\n", indentation, -1, -1);

            finalCode += returnStr;
        }

        //Sourced from https://matthewgall.codes/mirrors/ruffle/src/commit/e315fcb6b3c6f6a55b10ef6cd270c4ee5f493ccc/swf/src/avm1/read.rs
        private void DefineFunction2(SwfReader read, int indentation, int instructionNum, int offset) 
        {
            uint len = read.ReadUInt16();
            string fnName = read.ReadCharString();
            ushort paramLen = read.ReadUInt16();
            byte registerLen = read.ReadByte();
            ushort flags = read.ReadUInt16();

            List<ActionScriptParam> paramList = new List<ActionScriptParam>();

            for (int i = 0; i < paramLen; i++) 
            {
                ActionScriptParam param = new ActionScriptParam();
                param.register = read.ReadByte();
                param.name = read.ReadCharString();
            }

            ushort codeLen = read.ReadUInt16();

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "DefineFunction2 " + fnName + "( " + "TODO:STUFF" + " )" +  " {\n", indentation, instructionNum, offset);

            finalCode += returnStr;

            //read the instructions inside
            byte[] instr = read.ReadBytes(codeLen);
            SwfReader instrReader = new SwfReader(instr);
            ParseAllData(instrReader, indentation + 1);


            returnStr = "";
            returnStr = AddWithIndentation(returnStr, "}\n", indentation, -1, -1);

            finalCode += returnStr;

        }

        private void DefineFunction(SwfReader read, int indentation, int instructionNum, int offset)
        {
            uint len = read.ReadUInt16();
            string fnName = read.ReadCharString();
            ushort paramLen = read.ReadUInt16();

            List<string> paramList = new List<string>();

            for (int i = 0; i < paramLen; i++)
            {
                string param = read.ReadCharString();
            }

            ushort codeLen = read.ReadUInt16();

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "DefineFunction " + fnName + "( " + "TODO:STUFF" + " )" + " {\n", indentation, instructionNum, offset);

            finalCode += returnStr;

            //read the instructions inside
            byte[] instr = read.ReadBytes(codeLen);
            SwfReader instrReader = new SwfReader(instr);
            ParseAllData(instrReader, indentation + 1);


            returnStr = "";
            returnStr = AddWithIndentation(returnStr, "}\n", indentation, -1, -1);

            finalCode += returnStr;

        }

        private void Push(SwfReader read, int indentation, int instructionNum, int offset) 
        {
            ushort len = read.ReadUInt16();

            ushort handledBytes = 0;

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Push( ", indentation, instructionNum, offset);

            for (int i = 0; handledBytes < len; i++) 
            {
                byte type = read.ReadByte();
                switch (type)
                {
                    case 3:
                        returnStr += "undefined()";

                        break;
                    case 4:
                        handledBytes++;
                        byte loc = read.ReadByte();
                        returnStr += "Local(" + loc.ToString() + ")";

                        break;
                    case 5:
                        handledBytes++;
                        bool bl = read.ReadBoolean();
                        returnStr += "Bool(" + (bl ? "TRUE" : "FALSE") + ")";

                        break;
                    case 6:
                        handledBytes+= 8;
                        double dbl = read.ReadDouble();
                        returnStr += "Double(\"" + dbl.ToString() + "\")";

                        break;
                    case 7:
                        handledBytes += 4;
                        int integer = read.ReadInt32();
                        returnStr += "Integer(\"" + integer.ToString() + "\")";

                        break;
                    case 8:
                        handledBytes++;
                        byte entry = read.ReadByte();
                        returnStr += "Dictionary(" + constants[entry] + ")";
                        
                        break;
                    case 9:
                        handledBytes++;
                        handledBytes++;
                        ushort entryUshort = read.ReadUInt16();
                        returnStr += "Dictionary(" + constants[entryUshort] + ")";

                        break;
                    default:
                        returnStr += "UNKNOWN[" + type.ToString() + "]! EXPECT BROKEN!";
                        break;
                }

                handledBytes++;
                if (handledBytes < len)
                    returnStr += ", ";
            }

            returnStr += " );\n";

            finalCode += returnStr;
        }

        private void If(SwfReader read, int indentation, int instructionNum, int offset)
        {
            ushort len = read.ReadUInt16();

            ushort instr = read.ReadUInt16();

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "If( ", indentation, instructionNum, offset);
            returnStr += instr.ToString() + " )\n";
            finalCode += returnStr;
        }

        private void Goto(SwfReader read, int indentation, int instructionNum, int offset)
        {
            ushort len = read.ReadUInt16();

            ushort instr = read.ReadUInt16();

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Goto( ", indentation, instructionNum, offset);
            returnStr += instr.ToString() + " )\n";
            finalCode += returnStr;
        }

        private void GotoLabel(SwfReader read, int indentation, int instructionNum, int offset)
        {
            ushort len = read.ReadUInt16();

            string label = read.ReadCharString();

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "GotoLabel( ", indentation, instructionNum, offset);
            returnStr += label.ToString() + " )\n";
            finalCode += returnStr;
        }

        private void StoreRegister(SwfReader read, int indentation, int instructionNum, int offset)
        {
            ushort len = read.ReadUInt16();

            byte id = read.ReadByte();

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "StoreRegister( ", indentation, instructionNum, offset);
            returnStr += id.ToString() + " );\n";
            finalCode += returnStr;
        }

        private void GetUrl2(SwfReader read, int indentation, int instructionNum, int offset)
        {
            ushort len = read.ReadUInt16();

            byte unk = read.ReadByte();

            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "GetUrl2( ", indentation, instructionNum, offset);
            returnStr += unk.ToString() + " );\n";
            finalCode += returnStr;
        }

        private void GetMember(SwfReader read, int indentation, int instructionNum, int offset) 
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "GetMember();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void SetMember(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "SetMember();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void CallFunction(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "CallFunction();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void Pop(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Pop();\n\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void GetVariable(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "GetVariable();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void Unknown70(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Unknown70();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void StrictEqual(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "StrictEqual();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void Equal(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Equal();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void GreaterThan(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "GreaterThan();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void IntegralPart(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "IntegralPart();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void Add(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Add();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void Increment(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Increment();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void CallMethod(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "CallMethod();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void SetVariable(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "SetVariable();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }
        private void Return(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Return();\n\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void Not(SwfReader read, int indentation, int instructionNum, int offset)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, "Not();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }

        private void GENERICFUNC(SwfReader read, int indentation, int instructionNum, int offset, string name)
        {
            string returnStr = "";
            returnStr = AddWithIndentation(returnStr, name + "();\n", indentation, instructionNum, offset);
            finalCode += returnStr;
        }
    }
}
