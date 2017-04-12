using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Digging_Game_Demonstrator
{
    class Sound
    {
        static bool PlaySound = true;
        static Device DEVICE;
        static Dictionary<string, SecondaryBuffer> BUFFER;
        static Dictionary<string, Stream> STREAM;
        public static void Begin(string name,BufferPlayFlags playflags=BufferPlayFlags.Looping)
        {
            if (!PlaySound) return;
            if (!BUFFER.ContainsKey(name))
            {
                string s = @"Sound\" + name + ".wav";
                FileStream fs=new FileStream(s, FileMode.Open);
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                fs.Dispose();
                STREAM[name] = new MemoryStream(bs);
                BUFFER[name] = new SecondaryBuffer(STREAM[name], DEVICE);
            }
            BUFFER[name].SetCurrentPosition(0);
            BUFFER[name].Play(0, playflags);
        }
        public static void SetVolumn(string name,int value)
        {
            if (!PlaySound) return;
            BUFFER[name].Volume = value;
        }
        public static void Stop(string name)
        {
            BUFFER[name].Stop();
        }
        public static void Play(string name)
        {
            if (!PlaySound) return;
            STREAM[name].Position = 0;
            SecondaryBuffer buffer = new SecondaryBuffer(STREAM[name], DEVICE);
            buffer.Stop();
            buffer.SetCurrentPosition(0);
            buffer.Play(0, BufferPlayFlags.Default);
        }
        public static void StopAll()
        {
            foreach(var a in BUFFER)
            {
                a.Value.Stop();
            }
        }
        public static void InitialComponents()
        {
            try
            {
                DEVICE = new Device();
                DEVICE.SetCooperativeLevel(PublicVariables.THIS, CooperativeLevel.Priority);
                BUFFER = new Dictionary<string, SecondaryBuffer>();
                STREAM = new Dictionary<string, Stream>();
                DirectoryInfo dir = new DirectoryInfo("Sound");
                foreach (FileInfo f in dir.GetFiles())
                {
                    if (f.Extension != ".wav") continue;
                    string name = f.Name.Remove(f.Name.Length - 4);
                    Begin(name);
                    Stop(name);
                }
            }
            catch(OutOfMemoryException)
            {
                MessageBox.Show("You can still play the game(if other components loaded successfully), but sound effects will be unavailable", "It seems there's not enough memory on your computer");
                DEVICE.Dispose();
                BUFFER.Clear();
                STREAM.Clear();
                PlaySound = false;
            }
        }
    }
}
