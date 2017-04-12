using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Threading;

namespace Digging_Game_3_Camera_Practice
{
    public partial class CreateDevice : Form
    {
        #region constructors
        Device _device = null;
        #endregion
        public CreateDevice()
        {
            MessageBox.Show("a");
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Text = "D3D Tutorial 01: CreateDevice";
            this.Shown += CreateDevice_Shown;
        }

        void CreateDevice_Shown(object sender, EventArgs e)
        {
            if (!this.InitializeGraphics()) // Initialize Direct3D
            {
                MessageBox.Show("Could not initialize Direct3D.  This tutorial will exit.");
                return;
            }
            this.Show();
            // While the form is still valid, render and process messages
            while (this.Created)
            {
                this.Render();
                Application.DoEvents();
            }
        }
        public bool InitializeGraphics()
        {
            PresentParameters presentParams = new PresentParameters();
            try
            {
                // Now  setup our D3D stuff
                presentParams.Windowed = true;
                presentParams.SwapEffect = SwapEffect.Discard;
                
                //ThreadStart thread_start = new ThreadStart(delegate{
                _device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);
                /*});
                
                Thread thread = new Thread(thread_start);
                thread.Start();
                thread.Join();*/
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private void Render()
        {
            if (_device == null)
                return;

            //Clear the backbuffer to a blue color 
            _device.Clear(ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);
            //Begin the scene
            _device.BeginScene();

            // Rendering of scene objects can happen here

            //End the scene
            _device.EndScene();
            _device.Present();
        }
    }
}
