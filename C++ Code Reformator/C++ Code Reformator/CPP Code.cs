using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C___Code_Reformator
{
    class CPP_Code
    {
        //static HashSet<string> TYPE;
        static HashSet<char> CHAR_IN_NAME = new HashSet<char>();
        static string[] BASE_TYPES = new string[] { "bool", "char", "short", "int", "long long", "float", "double", "long double" };
        static string NextName(ref string code,int idx)
        {
            while (idx < code.Length && !CHAR_IN_NAME.Contains(code[idx])) idx++;
            StringBuilder ans = new StringBuilder();
            while (idx < code.Length && CHAR_IN_NAME.Contains(code[idx])) ans.Append(code[idx++]);
            return ans.ToString();
        }
        static List<int> IdxesOf(ref string code,string text)
        {
            int idx = code.IndexOf(text);
            List<int> ans = new List<int>();
            while(idx!=-1)
            {
                ans.Add(idx++);
                if (idx >= code.Length) break;
                idx = code.IndexOf(text, idx);
            }
            return ans;
        }
        static void GetAllTypes(ref string code)
        {
            //TYPE.Clear();
            //foreach (var c in BASE_TYPES) TYPE.Add(c);
            string[] typedefs = new string[] { "struct", "class","typedef","#define" };
            foreach (var t in typedefs)
            {
                foreach (var i in IdxesOf(ref code, t + " "))
                {
                    //if(t=="typedef")TYPE.Add()
                    //else TYPE.Add(NextName(ref code, i + t.Length));
                }
            }
        }
        static void Process(ref string code)
        {
            if (code[0] == '#') return;
        }
        public static string Reformat(string _code)
        {
            GetAllTypes(ref _code);
            _code = _code.Replace("\r", "");
            string[] code = _code.Split('\n');
            for (int i = 0; i < code.Length; i++)
            {
                Process(ref code[i]);
            }
            StringBuilder ans = new StringBuilder();
            for (int i = 0; i < code.Length; i++)
            {
                ans.Append(code[i]);
                ans.AppendLine();
            }
            return ans.ToString();
        }
        static CPP_Code()
        {
            CHAR_IN_NAME.Add('_');
            for (char c = '0'; c <= '9'; c++) CHAR_IN_NAME.Add(c);
            for (char c = 'a'; c <= 'z'; c++) CHAR_IN_NAME.Add(c);
            for (char c = 'A'; c <= 'Z'; c++) CHAR_IN_NAME.Add(c);
        }
    }
}
