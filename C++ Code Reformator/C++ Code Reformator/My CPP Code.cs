using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C___Code_Reformator
{
    class My_CPP_Code
    {
        struct Change : IComparable
        {
            public int l, r;
            public string s;
            public Change(int l, int r, string s) { this.l = l; this.r = r; this.s = s; }
            public int CompareTo(object o)
            {
                if (o.GetType() != typeof(Change)) return 0;
                Change a = (Change)o;
                if (r == a.r) return 0;
                return r - a.r;
            }
        };
        static bool BelongTo(char a, char[] v)
        {
            for (int i = 0; i < v.Length; i++) if (a == v[i]) return true;
            return false;
        }
        static bool IsUpper(char a) { return a >= 'A' && a <= 'Z'; }
        static bool IsLower(char a) { return a >= 'a' && a <= 'z'; }
        static bool IsLetter(char a) { return IsUpper(a) || IsLower(a); }
        static bool IsDigit(char a) { return a >= '0' && a <= '9'; }
        static bool IsTermOnR(char a)
        {
            if (IsLetter(a) || IsDigit(a)) return true;
            if (a == '(' || a == '{') return true;
            if (a == '-' || a == '+') return true;
            if (a == '\'') return true;
            return false;
        }
        static bool IsTermOnL(char a)
        {
            if (IsLetter(a) || IsDigit(a)) return true;
            if (a == ')' || a == ']') return true;
            if (a == '-' || a == '+') return true;
            if (a == '\'') return true;
            return false;
        }
        static bool IsLBrack(char a) { return a == '(' || a == '[' || a == '{'; }
        static bool IsRBrack(char a) { return a == ')' || a == ']' || a == '}'; }
        static char[] UNVISABLE = new char[] { ' ', '\t' };
        static List<Change> CHANGE = new List<Change>();
        static string CODE;
        static bool[] VIS = new bool[0];
        static bool[] TEM = new bool[0];
        static int[] LIN = new int[0];
        static void OperatorAddSpace()
        {
            CHANGE.Clear();
            MarkString();
            MarkTemplate();
            for (int i = 1; i + 1 < CODE.Length; )
            {
                Show((i - 1) * 100 / (CODE.Length - 2), 0);
                if (VIS[i]) { i++; continue; }
                int ipre = i;
                switch (CODE[i])
                {
                    case '(':
                    case '[':
                    case '{':
                        {
                            if (CODE[i + 1] == '\n' || IsRBrack(CODE[i + 1])) break;
                            CHANGE.Add(new Change(i + 1, i + 1, " "));
                            i++;
                        } break;
                    case ')':
                    case ']':
                    case '}':
                        {
                            if (IsTermOnR(CODE[i + 1])) CHANGE.Add(new Change(i + 1, i + 1, " "));
                            if (IsLBrack(CODE[i - 1])) break;
                            bool allblank = true;
                            for (int j = i - 1; j >= 0; j--)
                            {
                                if (CODE[j] == '\n') break;
                                else if (!BelongTo(CODE[j], UNVISABLE)) { allblank = false; break; }
                            }
                            if (allblank) break;
                            CHANGE.Add(new Change(i, i, " "));
                            i++;
                        } break;
                }
                if (i != ipre) continue;
                if (i + 3 < CODE.Length)
                {
                    switch (CODE.Substring(i, 3))
                    {
                        case "<<=":
                        case ">>=":
                            {
                                if (!IsTermOnR(CODE[i + 3]) || !IsTermOnL(CODE[i - 1])) break;
                                CHANGE.Add(new Change(i, i, " "));
                                CHANGE.Add(new Change(i + 3, i + 3, " "));
                                i += 3;
                            } break;
                    }
                }
                if (i != ipre) continue;
                if (i + 2 < CODE.Length)
                {
                    switch (CODE.Substring(i, 2))
                    {
                        case "+=":
                        case "-=":
                        case "*=":
                        case "/=":
                        case "%=":
                        case "&=":
                        case "|=":
                        case "^=":
                        case "||":
                        case "&&":
                        case "==":
                        case "!=":
                        case ">=":
                        case "<=":
                        case ">>":
                        case "<<":
                            {
                                if (!IsTermOnR(CODE[i + 2]) || !IsTermOnL(CODE[i - 1])) break;
                                CHANGE.Add(new Change(i, i, " "));
                                CHANGE.Add(new Change(i + 2, i + 2, " "));
                                i += 2;
                            } break;
                        case "++":
                        case "--":
                            {
                                i += 2;
                            } break;
                    }
                }
                if (i != ipre) continue;
                switch (CODE[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                    case '=':
                    case '>':
                    case '<':
                    case '&':
                    case '|':
                    case '^':
                    case ':':
                    case '?':
                        {
                            if (TEM[i])
                            {
                                //if (CODE[i] == '<' && IsTermOnR(CODE[i + 1])) CHANGE.Add(new Change(i + 1, i + 1, " "));
                                //if (CODE[i] == '>' && IsTermOnL(CODE[i - 1])) CHANGE.Add(new Change(i, i, " "));
                                if (CODE[i] == '>' && IsTermOnR(CODE[i + 1])) CHANGE.Add(new Change(i + 1, i + 1, " "));
                                break;
                            }
                            if (!IsTermOnR(CODE[i + 1]) || !IsTermOnL(CODE[i - 1])) break;
                            CHANGE.Add(new Change(i, i, " "));
                            CHANGE.Add(new Change(i + 1, i + 1, " "));
                            i++;
                        } break;
                    case ',':
                    case ';':
                        {
                            if (CODE[i + 1] == '\n') break;
                            CHANGE.Add(new Change(i + 1, i + 1, " "));
                            i++;
                        } break;
                }
                if (i != ipre) continue;
                i++;
            }
            ApplyChange();
        }
        static void ApplyChange()
        {
            if (CHANGE.Count == 0) return;
            CHANGE.Sort();
            Stack<string> stk = new Stack<string>();
            var lastchange = CHANGE[CHANGE.Count - 1];
            if (lastchange.r < CODE.Length) stk.Push(CODE.Substring(lastchange.r));
            stk.Push(lastchange.s);
            for (int i = CHANGE.Count - 2; i >= 0; i--)
            {
                var cnow = CHANGE[i];
                var cpre = CHANGE[i + 1];
                if (cpre.l - cnow.r >= 1) stk.Push(CODE.Substring(cnow.r, cpre.l - cnow.r));
                stk.Push(cnow.s);
            }
            if (CHANGE[0].l > 0) stk.Push(CODE.Remove(CHANGE[0].l));
            StringBuilder ans = new StringBuilder();
            while (stk.Count > 0) ans.Append(stk.Pop());
            CODE = ans.ToString();
        }
        static void MarkTemplate(string name)
        {
            int idx = CODE.IndexOf(name);
            for (; idx != -1; idx = CODE.IndexOf(name, idx + 1))
            {
                if (VIS[idx]) continue;
                if (idx - 1 >= 0 && (IsLetter(CODE[idx - 1]) || IsDigit(CODE[idx - 1]) || CODE[idx - 1] == '_')) continue;
                int l = idx + name.Length;
                if (l >= CODE.Length || CODE[l] != '<') continue;
                for (int r = l, cnt = 0; r < CODE.Length; r++)
                {
                    if (CODE[r] == '<') cnt++;
                    else if (CODE[r] == '>')
                    {
                        cnt--;
                        if (cnt == 0)
                        {
                            TEM[l] = TEM[r] = true;
                            break;
                        }
                    }
                }
            }
        }
        static void MarkTemplate()
        {
            TEM = new bool[CODE.Length];
            MarkTemplate("array");
            MarkTemplate("vector");
            MarkTemplate("deque");
            MarkTemplate("list");
            MarkTemplate("forward_list");
            MarkTemplate("stack");
            MarkTemplate("queue");
            MarkTemplate("priority_queue");
            MarkTemplate("set");
            MarkTemplate("multiset");
            MarkTemplate("map");
            MarkTemplate("multimap");
            MarkTemplate("unordered_set");
            MarkTemplate("unordered_multiset");
            MarkTemplate("unordered_map");
            MarkTemplate("unordered_multimap");
            MarkTemplate("bitset");
            MarkTemplate("valarray");
        }
        static void MarkString()
        {
            VIS = new bool[CODE.Length];
            for (int i = 0; i < CODE.Length; )
            {
                if (CODE[i] == '\'' || CODE[i] == '"')
                {
                    char c = CODE[i];
                    VIS[i] = true;
                    for (i++; i < CODE.Length && CODE[i] != c; i++)
                    {
                        VIS[i] = true;
                        if (CODE[i] == '\\')
                        {
                            i++;
                            VIS[i] = true;
                        }
                    }
                    VIS[i] = true;
                    i++;
                    continue;
                }
                else if (CODE[i] == '#')
                {
                    VIS[i] = true;
                    int r = i + 1;
                    while (r < CODE.Length && CODE[r] != '\n') { VIS[r] = true; r++; }
                    i = r;
                    continue;
                }
                else if (i + 1 < CODE.Length)
                {
                    if (CODE[i] == '/' && CODE[i + 1] == '/')
                    {
                        VIS[i] = VIS[i + 1] = true;
                        int r = i + 2;
                        while (r < CODE.Length && CODE[r] != '\n') { VIS[r] = true; r++; }
                        i = r;
                        continue;
                    }
                    if (CODE[i] == '/' && CODE[i + 1] == '*')
                    {
                        int r = CODE.IndexOf("*/", i);
                        if (r == -1) r = CODE.Length;
                        else r += 2;
                        for (int j = i; j < r; j++) VIS[j] = true;
                        i = r;
                        continue;
                    }
                }
                i++;
            }
        }
        static void Replace(string s1, string s2)
        {
            CHANGE.Clear();
            MarkString();
            int idx = CODE.IndexOf(s1);
            for (; idx != -1; idx = CODE.IndexOf(s1, idx + 1))
            {
                if (VIS[idx]) continue;
                CHANGE.Add(new Change(idx, idx + s1.Length, s2));
            }
            ApplyChange();
        }
        static void MarkLine()
        {
            LIN = new int[CODE.Length];
            for (int i = 0, line = 0; i < CODE.Length; i++)
            {
                LIN[i] = line;
                if (CODE[i] == '\n') line++;
            }
        }
        struct BigBrack
        {
            public int l, r;
            List<BigBrack> ch;
            public BigBrack(ref int idx)
            {
                l = idx;
                ch = new List<BigBrack>();
                for (idx++; idx < CODE.Length; idx++)
                {
                    Show(idx * 100 / (CODE.Length - 1), 0);
                    if (VIS[idx]) continue;
                    if (CODE[idx] == '{') ch.Add(new BigBrack(ref idx));
                    else if (CODE[idx] == '}') break;
                }
                r = idx;
            }
            string Tabs(int cnt)
            {
                StringBuilder ans = new StringBuilder();
                for (int i = 0; i < cnt; i++) ans.Append("\t");
                return ans.ToString();
            }
            public void MarkSplitLine()
            {
                int chi = 0;
                List<int> ls = new List<int>();
                List<int> rs = new List<int>();
                ls.Add(l + 1);
                for (int i = l + 1; i < r; i++)
                {
                    Show(i * 100 / (CODE.Length - 1), 1);
                    if (VIS[i]) continue;
                    if (chi < ch.Count && i == ch[chi].l)
                    {
                        ch[chi].MarkSplitLine();
                        i = ch[chi].r;
                        if (i + 1 >= r) break;
                        rs.Add(i);
                        ls.Add(i + 1);
                        chi++;
                        continue;
                    }
                    if (CODE[i] == '(')
                    {
                        for (int cnt = 0; i <= r; i++)
                        {
                            if (CODE[i] == '(') cnt++;
                            else if (CODE[i] == ')')
                            {
                                cnt--;
                                if (cnt == 0) break;
                            }
                        }
                        if (i + 1 >= r) break;
                        continue;
                    }
                    if (CODE[i] == ';')
                    {
                        if (i + 1 >= r) break;
                        rs.Add(i);
                        ls.Add(i + 1);
                        continue;
                    }
                }
                rs.Add(r - 1);
                if (ls.Count > 1 && LIN[l] == LIN[r])
                {
                    int idx = l, tab = 0;
                    for (; idx >= 0; idx--)
                    {
                        if (VIS[idx]) continue;
                        if (CODE[idx] == '{') tab++;
                        else if (CODE[idx] == '}') tab--;
                        else if (CODE[idx] == '\t') tab++;
                        else if (CODE[idx] == '\n') break;
                    }
                    CHANGE.Add(new Change(r, r, "\n" + Tabs(tab - 1)));
                    CHANGE.Add(new Change(l, l, "\n" + Tabs(tab - 1)));
                    for (int i = 0; i < ls.Count; i++)
                    {
                        CHANGE.Add(new Change(ls[i], ls[i], "\n" + Tabs(tab)));
                    }
                }
            }
        }
        static void SplitLine()
        {
            if (CODE.Length == 0) return;
            MarkString();
            MarkLine();
            CHANGE.Clear();
            int idx = 0;
            for (; idx < CODE.Length; idx++)
            {
                if (VIS[idx]) continue;
                if (CODE[idx] == '{')
                {
                    BigBrack BRACK = new BigBrack(ref idx);
                    if (BRACK.r >= CODE.Length) return;
                    PERCENT.Add(0); Show();
                    BRACK.MarkSplitLine();
                    Show(100, 1); PERCENT.RemoveAt(1);
                }
            }
            ApplyChange();
        }
        static int STAGE = 0;
        static List<int> PERCENT = new List<int>();
        static Form1 F;
        static void Show()
        {
            StringBuilder ans = new StringBuilder();
            ans.Append("Processing Stage ");
            ans.Append(STAGE.ToString());
            ans.Append(" ( ");
            ans.Append(PERCENT[0].ToString() + "%");
            for (int i = 1; i < PERCENT.Count; i++) ans.Append("-" + PERCENT[i].ToString() + "%");
            ans.Append(" )");
            F.Text = ans.ToString();
            System.Windows.Forms.Application.DoEvents();
        }
        static void Show(int percent, int idx)
        {
            if (PERCENT[idx] == percent) return;
            PERCENT[idx] = percent;
            Show();
        }
        public static string Reformat(string code, Form1 f)
        {
            F = f;
            STAGE = 1; PERCENT[0] = 100; Show();
            CODE = code.Replace("\r\n", "\n");
            STAGE++; PERCENT[0] = 0; Show();
            SplitLine();
            STAGE++; PERCENT[0] = 0; Show();
            OperatorAddSpace();
            STAGE++; PERCENT[0] = 100; Show();
            Replace("  ", " ");
            STAGE++; PERCENT[0] = 100; Show();
            Replace(" ;", ";");
            STAGE++; PERCENT[0] = 100; Show();
            Replace(" ,", ",");
            STAGE++; PERCENT[0] = 100; Show();
            Replace(" \n", "\n");
            STAGE++; PERCENT[0] = 100; Show();
            F.Text = "Almost Complete...";
            CODE = CODE.Replace("\n", "\r\n");
            return CODE;
        }
        static void RemoveLineNumber()
        {
            CHANGE.Clear();
            int idx = 0;
            for (; idx < CODE.Length; )
            {
                int r = idx;
                for (; r < CODE.Length && IsDigit(CODE[r]); ) r++;
                if (idx < CODE.Length && CODE[r] == '.')
                {
                    CHANGE.Add(new Change(idx, r + 1, ""));
                }
                while (idx < CODE.Length && CODE[idx] != '\n') idx++;
                if (idx < CODE.Length && CODE[idx] == '\n') idx++;
            }
            ApplyChange();
        }
        public static string RemoveLineNumber(string code, Form1 f)
        {
            F = f;
            STAGE = 1; PERCENT[0] = 100; Show();
            CODE = code.Replace("\r\n", "\n");
            STAGE++; PERCENT[0] = 100; Show();
            RemoveLineNumber();
            F.Text = "Almost Complete...";
            CODE = CODE.Replace("\n", "\r\n");
            return CODE;
        }
        static void RemoveLeftNumber()
        {
            CHANGE.Clear();
            int idx = 0;
            for (; idx < CODE.Length; )
            {
                int r = idx;
                for (; r < CODE.Length && (CODE[r] == '\t' || CODE[r] == ' '); ) r++;
                if(IsDigit(CODE[r]))
                {
                    while (r<CODE.Length&&IsDigit(CODE[r])) r++;
                    CHANGE.Add(new Change(idx, r + 1, ""));
                }
                while (idx < CODE.Length && CODE[idx] != '\n') idx++;
                if (idx < CODE.Length && CODE[idx] == '\n') idx++;
            }
            ApplyChange();
        }
        public static string RemoveLeftNumber(string code,Form1 f)
        {
            F = f;
            STAGE = 1; PERCENT[0] = 100; Show();
            CODE = code.Replace("\r\n", "\n");
            STAGE++; PERCENT[0] = 100; Show();
            RemoveLeftNumber();
            F.Text = "Almost Complete...";
            CODE = CODE.Replace("\n", "\r\n");
            return CODE;
        }
        static void RemoveRightSpace()
        {
            CHANGE.Clear();
            int idx = 0;
            for (; idx < CODE.Length; )
            {
                int r = idx;
                for (; r < CODE.Length && CODE[r] !='\n'; ) r++;
                int l=r-1;
                for (;l>=0&& CODE[l] == ' '; l--) ;
                l++;
                if(l<r)CHANGE.Add(new Change(l, r, ""));
                idx = r+1;
            }
            ApplyChange();
        }
        public static string RemoveRightSpace(string code, Form1 f)
        {
            F = f;
            STAGE = 1; PERCENT[0] = 100; Show();
            CODE = code.Replace("\r\n", "\n");
            STAGE++; PERCENT[0] = 100; Show();
            RemoveRightSpace();
            F.Text = "Almost Complete...";
            CODE = CODE.Replace("\n", "\r\n");
            return CODE;
        }
        static void RemoveLeftSpace()
        {
            CHANGE.Clear();
            int idx = 0;
            for (; ;)
            {
                int l = idx, r = idx;
                for (; r < CODE.Length && CODE[r] == ' ';) r++;
                if (l < r) CHANGE.Add(new Change(l, r, ""));
                idx = CODE.IndexOf("\n", idx + 1);
                if (idx == -1 || ++idx == CODE.Length) break;
            }
            ApplyChange();
        }
        public static string RemoveLeftSpace(string code, Form1 f)
        {
            F = f;
            STAGE = 1; PERCENT[0] = 100; Show();
            CODE = code.Replace("\r\n", "\n");
            STAGE++; PERCENT[0] = 100; Show();
            RemoveLeftSpace();
            F.Text = "Almost Complete...";
            CODE = CODE.Replace("\n", "\r\n");
            return CODE;
        }
        static void ReplaceWithTabOrRemove()
        {
            CHANGE.Clear();
            int idx = 0;
            for (; ;)
            {
                int l = idx, r = idx;
                for (; r < CODE.Length && CODE[r] == ' ';) r++;
                StringBuilder tmp = new StringBuilder();
                for (int i = 0; i < (r - l) / 4; i++) tmp.Append('\t');
                if (l < r) CHANGE.Add(new Change(l, r, tmp.ToString()));
                idx = CODE.IndexOf("\n", idx + 1);
                if (idx == -1 || ++idx == CODE.Length) break;
            }
            ApplyChange();
        }
        public static string ReplaceWithTabOrRemove(string code,Form1 f)
        {
            F = f;
            STAGE = 1; PERCENT[0] = 100; Show();
            CODE = code.Replace("\r\n", "\n");
            STAGE++; PERCENT[0] = 100; Show();
            ReplaceWithTabOrRemove();
            F.Text = "Almost Complete...";
            CODE = CODE.Replace("\n", "\r\n");
            return CODE;
        }
        static My_CPP_Code()
        {
            PERCENT.Add(0);
        }
    }
}
