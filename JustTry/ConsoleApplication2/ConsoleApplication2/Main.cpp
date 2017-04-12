#include<cstdio>
#include<vector>
#include<string>
#include<map>
#include<algorithm>
#include<queue>
#include<set>
using namespace std;
//template<class T>inline bool getmin(T&a,const T&b){return b<a?(a=b,true):false;}
//template<class T>inline bool getmax(T&a,const T&b){return a<b?(a=b,true):false;}
char masterBuffer[4][380000003];
char *bufGetChar;
int ptrGetChar;
size_t nGetChar;
inline bool ReadS(char *s)
{
	char c;
	while (c = bufGetChar[ptrGetChar++], c<'a' || 'z'<c)
	{
		if (c == EOF)return false;
	}
	do
	{
		*s++ = c;
	} while (c = bufGetChar[ptrGetChar++], 'a' <= c&&c <= 'z');
	*s = '\0';
	return true;
}
inline bool ReadD(unsigned &v)
{
	char c;
	while (c = bufGetChar[ptrGetChar++], c<'0' || '9'<c)
	{
		if (c == EOF)return false;
	}
	v = 0;
	do
	{
		(v *= 10) += (c - '0');
	} while (c = bufGetChar[ptrGetChar++], '0' <= c&&c <= '9');
	return true;
}
namespace Trie
{
	const int W = 26, FINAL_SZ = 770003, WORD_COUNT = 290003;
	int CH[FINAL_SZ][W], ID[FINAL_SZ];
	vector<string>WORDS;
	int SZ;
	inline void Expand()
	{
		for (int c = 0; c<W; c++)CH[SZ][c] = 0;
		ID[SZ] = -1;
		++SZ;
	}
	inline void Clear()
	{
		SZ = 0;
		WORDS.reserve(WORD_COUNT);
		Expand();
	}
	inline int GetNxt(const int u, const int c)
	{
		if (CH[u][c])return CH[u][c];
		else
		{
			Expand();
			return CH[u][c] = SZ - 1;
		}
	}
	inline void Insert(const char *s)
	{
		int u = 0;
		for (int i = 0; s[i]; i++)u = GetNxt(u, s[i] - 'a');
		if (ID[u] == -2)return;
		ID[u] = -2;
	}
	void Build(const int u, int &id, string &s)
	{
		if (ID[u] == -2)
		{
			ID[u] = id++;
			WORDS.push_back(s);
		}
		for (int c = 0, *ch = CH[u]; c<W; c++, ch++)
		{
			if (*ch)
			{
				s.push_back((char)('a' + c));
				Build(*ch, id, s);
				s.pop_back();
			}
		}
	}
	inline void Build()
	{
		int id = 0;
		string s;
		Build(0, id, s);
	}
	inline int Id(const char *s)
	{
		int u = 0;
		for (int i = 0; s[i]; i++)u = CH[u][s[i] - 'a'];
		return ID[u];
	}
	inline bool Find(const char *s)
	{
		int u = 0;
		for (int i = 0; s[i]; i++)
		{
			if (!CH[u][s[i] - 'a'])return false;
			u = CH[u][s[i] - 'a'];
		}
		return ID[u] >= 0;
	}
	inline string Word(const int id)
	{
		if (id == -3)return "_";
		return WORDS[id];
	}
}
struct Phrase
{
	int n, *words;
	unsigned frequency;
	Phrase(int _n) :n(_n), words(new int[n]) {}
	inline string ToString()const
	{
		string ans;
		for (int i = 0; i<n; i++)
		{
			ans += Trie::Word(words[i]);
			ans.push_back(' ');
		}
		ans.pop_back();
		ans.push_back('\t');
		ans += std::to_string(frequency);
		return ans;
	}
};
inline bool operator<(const Phrase &a, const Phrase &b)
{
	if (a.frequency != b.frequency)return a.frequency>b.frequency;
	for (int i = 0; i<a.n&&i<b.n; i++)
	{
		if (a.words[i] != b.words[i])return a.words[i]<b.words[i];
	}
	return a.n<b.n;
}
vector<Phrase>PHRASES[6];
inline void Initialize(string filePath)
{
	Trie::Clear();
	PHRASES[2].reserve(8500000);
	PHRASES[3].reserve(16000000);
	PHRASES[4].reserve(12000000);
	PHRASES[5].reserve(5800000);
	{
		char *tmp = new char[1000000];
		for (int n = 2; n <= 5; n++)
		{
			//            printf("n=%d\n",n);
			auto getFileName = [&](int a)->string {return filePath + std::to_string(a) + "gm.small.txt"; };
			FILE *f = fopen(getFileName(n).c_str(), "r");
			bufGetChar = masterBuffer[n - 2];
			nGetChar = fread(bufGetChar, 1, 380000000, f);
			bufGetChar[nGetChar++] = EOF;
			ptrGetChar = 0;
			fclose(f);
			while (ReadS(tmp))
			{
				Trie::Insert(tmp);
				for (int i = 1; i<n; i++)
				{
					ReadS(tmp);
					Trie::Insert(tmp);
				}
				unsigned v;
				ReadD(v);
			}
		}
		//        puts("Building");
		Trie::Build();
		//        puts("Built");
		//        unsigned mx=0,sum=0;
		for (int n = 2; n <= 5; n++)
		{
			//            printf("n=%d\n",n);
			bufGetChar = masterBuffer[n - 2];
			ptrGetChar = 0;
			while (ReadS(tmp))
			{
				Phrase p = Phrase(n);
				p.words[0] = Trie::Id(tmp);
				for (int i = 1; i<n; i++)
				{
					ReadS(tmp);
					p.words[i] = Trie::Id(tmp);
				}
				//                sum+=n;
				ReadD(p.frequency);
				PHRASES[n].push_back(p);
				//                getmax(mx,p.frequency);
				//            vs.push_back(p.frequency);
			}
		}
		delete[]tmp;
		//        puts("Initialized");
		//        printf("mx=%u,sum=%u\n",mx,sum);
	}
	//for (int n = 2; n <= 5; n++)printf("%d\n", (int)PHRASES[n].size());
}
inline bool IsMatch(const Phrase &p, const vector<int>&input)
{
	if (p.n != (int)input.size())return false;
	for (int i = 0; i<p.n; i++)
	{
		if (input[i] != -3 && p.words[i] != input[i])return false;
	}
	return true;
}
inline bool IsMatch(const Phrase &p, const vector<vector<int> >&input)
{
	for (const auto &s : input)
	{
		if (IsMatch(p, s))return true;
	}
	return false;
}
void DfsExpand3(const vector<string>&source, const int dep, vector<string>&now, set<vector<string> >&target)
{
	if (now.size()>5)return;
	if (dep == (int)source.size())
	{
		target.insert(now);
		return;
	}
	const string &s = source[dep];
	if (s != "*")
	{
		now.push_back(s);
		DfsExpand3(source, dep + 1, now, target);
		now.pop_back();
	}
	else
	{
		DfsExpand3(source, dep + 1, now, target);
		for (int i = 0; i<5; i++)
		{
			now.push_back("_");
			DfsExpand3(source, dep + 1, now, target);
		}
		for (int i = 0; i<5; i++)now.pop_back();
	}
}
inline void Expand3(const set<vector<string> >&source, set<vector<string> >&target)
{
	target.clear();
	for (const auto &s : source)
	{
		vector<string>now;
		DfsExpand3(s, 0, now, target);
	}
}
void DfsExpand2(const vector<string>&source, const int dep, vector<string>&now, set<vector<string> >&target)
{
	if (now.size()>5)return;
	if (dep == (int)source.size())
	{
		target.insert(now);
		return;
	}
	vector<string>candidator;
	{
		const char *s = source[dep].c_str();
		string ts;
		for (int i = 0;; i++)
		{
			if (s[i] == '/' || s[i] == '\0')
			{
				candidator.push_back(ts);
				ts.clear();
				if (s[i] == '\0')break;
			}
			else
			{
				ts += s[i];
			}
		}
	}
	for (const string &s : candidator)
	{
		now.push_back(s);
		DfsExpand2(source, dep + 1, now, target);
		now.pop_back();
	}
}
inline void Expand2(const set<vector<string> >&source, set<vector<string> >&target)
{
	target.clear();
	for (const auto &s : source)
	{
		vector<string>now;
		DfsExpand2(s, 0, now, target);
	}
}
void DfsExpand1(const vector<string>&source, const int dep, vector<string>&now, set<vector<string> >&target)
{
	if (now.size()>5)return;
	if (dep == (int)source.size())
	{
		target.insert(now);
		return;
	}
	if (source[dep][0] != '?')
	{
		now.push_back(source[dep]);
		DfsExpand1(source, dep + 1, now, target);
		now.pop_back();
	}
	else
	{
		now.push_back(source[dep].substr(1));
		DfsExpand1(source, dep + 1, now, target);
		now.pop_back();
		DfsExpand1(source, dep + 1, now, target);
	}
}
inline void Expand1(const set<vector<string> >&source, set<vector<string> >&target)
{
	target.clear();
	for (const auto &s : source)
	{
		vector<string>now;
		DfsExpand1(s, 0, now, target);
	}
}
inline vector<vector<int>>ExpandInput(vector<string>input)
{
	set<vector<string> >tmp1, tmp2;
	tmp1.insert(input);
	Expand1(tmp1, tmp2);//"?"
	Expand2(tmp2, tmp1);
	Expand3(tmp1, tmp2);
	vector<vector<int> >ans;
	for (const auto &a : tmp2)
	{
		vector<int>s;
		bool valid = true;
		for (const string &b : a)
		{
			if (b == "_")s.push_back(-3);
			//else if (!Trie::Find(b.c_str()))
			//{
			//	//                printf("invalid string: \"%s\"\n",b.c_str());
			//	valid = false;
			//	break;
			//}
			else s.push_back(Trie::Id(b.c_str()));
		}
		if (valid)ans.push_back(s);
	}
	return ans;
}
inline void SortQueries(const vector<vector<int>>&qqq, vector<int>&queries, const int depth)
{
	const int wordCount = 281670;
	static vector<int>bucket[wordCount];//word count
	if ((int)queries.size() * 5<wordCount)
	{
		sort(queries.begin(), queries.end(), [&](const int a, const int b)->bool
		{
			return qqq[a][depth]<qqq[b][depth];
		});
	}
	else
	{
		vector<int>ans;
		for (const int q : queries)
		{
			const int v = qqq[q][depth];
			if (v == -3)ans.push_back(q);
			else
			{
				bucket[v].push_back(q);
			}
		}
		for (int i = 0; i<wordCount; i++)
		{
			for (const int q : bucket[i])ans.push_back(q);
			bucket[i].clear();
		}
		queries.swap(ans);
	}
}
inline void SortPhrases(const vector<Phrase>&ppp, vector<int>&phrases, const int depth)
{
	const int wordCount = 281670;
	static vector<int>bucket[wordCount];//word count
	if ((int)phrases.size() * 5<wordCount)
	{
		sort(phrases.begin(), phrases.end(), [&](const int a, const int b)->bool
		{
			return ppp[a].words[depth]<ppp[b].words[depth];
		});
	}
	else
	{
		vector<int>ans;
		for (const int p : phrases)
		{
			const int v = ppp[p].words[depth];
			if (v == -3)ans.push_back(p);
			else bucket[v].push_back(p);
		}
		for (int i = 0; i<wordCount; i++)
		{
			for (const int p : bucket[i])ans.push_back(p);
			bucket[i].clear();
		}
		phrases.swap(ans);
	}
}
inline void UpdateAnswer(vector<const Phrase*>&answer, const Phrase *p)
{
	for (const auto a : answer)if (a == p)return;
	answer.push_back(p);
	for (int i = (int)answer.size() - 1; i >= 1 && (*answer[i])<(*answer[i - 1]); i--)swap(answer[i], answer[i - 1]);
	if (answer.size()>5)answer.pop_back();
}
vector<vector<const Phrase*>>ANSWER;
vector<vector<int>>QUERIES[6];
void Solve(const int n, const vector<int>&queries, vector<int>&_phrases, const int depth)
{
	if (queries.empty() || _phrases.empty())return;
	//    printf("depth=%d, queries.size=%d, phrases.size=%d\n",depth,(int)queries.size(),(int)phrases.size());
	const auto &phrases = _phrases;
	const auto &qqq = QUERIES[n];
	const auto &ppp = PHRASES[n];
	if (depth == n)
	{
		//        for(const int p:phrases)printf(" %s",ppp[p].ToString().c_str());puts("");
		for (const int q : queries)
		{
			for (const int p : phrases)
			{
				UpdateAnswer(ANSWER[qqq[q][n]], &ppp[p]);
			}
		}
		return;
	}
	{
		vector<int>nxt_queries, nxt_phrases;
		//    for(int i=1;i<(int)queries.size();i++)assert(qqq[queries[i-1]][depth]<qqq[queries[i]][depth]);
		//    for(int i=1;i<(int)phrases.size();i++)assert(ppp[phrases[i-1]].words[depth]<ppp[phrases[i]].words[depth]);
		for (int qi = 0, pi = 0;;)
		{
			for (; pi<(int)phrases.size() && ppp[phrases[pi]].words[depth]<qqq[queries[qi]][depth]; pi++);
			const int v = qqq[queries[qi]][depth];
			for (; qi<(int)queries.size() && qqq[queries[qi]][depth] == v; qi++)nxt_queries.push_back(queries[qi]);
			for (; pi<(int)phrases.size() && ppp[phrases[pi]].words[depth] == v; pi++)nxt_phrases.push_back(phrases[pi]);
			SortQueries(qqq, nxt_queries, depth+1);
			if (depth + 1<n)SortPhrases(ppp, nxt_phrases, depth+1);
			Solve(n, nxt_queries, nxt_phrases, depth + 1);
			nxt_queries.clear();
			nxt_phrases.clear();
			if (qi == (int)queries.size() || pi == (int)phrases.size())break;
		}
	}
	if (qqq[queries[0]][depth] == -3)
	{
		//        puts("qqq=-3");
		vector<int>nxt_queries;
		for (int i = 0; i<(int)queries.size() && qqq[queries[i]][depth] == -3; i++)nxt_queries.push_back(queries[i]);
		SortQueries(qqq, nxt_queries, depth+1);
		if (depth + 1<n)SortPhrases(ppp, _phrases, depth+1);
		Solve(n, nxt_queries, _phrases, depth + 1);
	}
}
inline void Solve(/*string fileName*/)
{
	//    freopen(fileName.c_str(),"r",stdin);
	char *tmp = new char[1000000];
	vector<string>inputs;
	for (; fgets(tmp, 1000000, stdin);)
	{
		for (int i = 0;; i++)if (tmp[i] == '\n' || tmp[i] == '\0')
		{
			tmp[i] = '\0';
			break;
		}
		inputs.push_back(tmp);
		//        printf("query: %s\n",tmp);
		vector<string>input;
		{
			string ts;
			for (int i = 0;; i++)
			{
				if (tmp[i] != ' '&&tmp[i] != '\0')
				{
					ts += tmp[i];
				}
				else
				{
					if (!ts.empty())
					{
						input.push_back(ts);
						//                        printf("ts=%s\n",ts.c_str());
						ts.clear();
					}
					if (tmp[i] == '\0')break;
				}
			}
		}
		const int id = (int)ANSWER.size();
		ANSWER.push_back(vector<const Phrase*>());
		for (auto s : ExpandInput(input))
		{
			s.push_back(id);
			QUERIES[s.size() - 1].push_back(s);
		}
	}
	//    puts("inputs finished");
	vector<int>queries, phrases;
	for (int n = 2; n <= 5; n++)
	{
		//        sort(QUERIES[n].begin(),QUERIES[n].end());
		phrases.resize(PHRASES[n].size());
		for (int i = 0; i < (int)QUERIES[n].size(); i++)queries.push_back(i);
		for (int i = 0; i < (int)PHRASES[n].size(); i++)phrases[i] = i;
		SortQueries(QUERIES[n], queries, 0);
		//sort(queries.begin(), queries.end(), [&](const int a, const int b)->bool
		//{
		//	return QUERIES[n][a][0]<QUERIES[n][b][0];
		//});
		//        printf("Solveing...n=%d, QUERIES.size=%d, PHRASES.size=%d\n",n,(int)QUERIES[n].size(),(int)PHRASES[n].size());
		Solve(n, queries, phrases, 0);
		queries.clear();
	}
	//    puts("Solved");
	delete[]tmp;
	for (int i = 0; i<(int)inputs.size(); i++)
	{
		printf("query: %s\n", inputs[i].c_str());
		const auto &ans = ANSWER[i];
		printf("output: %d\n", (int)ans.size());
		for (const Phrase *p : ans)
		{
			printf("%s\n", p->ToString().c_str());
		}
	}
}
int main(int argc, char **argv)
{
	//    if (argc != 2)
	//    {
	//        return -1;
	//    }
	//    Initialize(argv[1]);
	freopen("C:\\Users\\Burney\\OneDrive\\Sync\\C# Code\\JustTry\\ConsoleApplication2\\ConsoleApplication2\\input0100.txt", "r", stdin);
	freopen("C:\\Users\\Burney\\OneDrive\\Sync\\C# Code\\JustTry\\ConsoleApplication2\\ConsoleApplication2\\new0100.txt", "w", stdout);
	Initialize("C:\\Users\\Burney\\OneDrive\\Sync\\C# Code\\JustTry\\ConsoleApplication2\\ConsoleApplication2\\");
	Solve(/*"input0010.txt"*/);
	//    Solve("input0100.txt");
	return 0;
}