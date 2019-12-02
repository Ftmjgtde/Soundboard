using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;


namespace KredeSoundboardGame
{
    public partial class Form1 : Form
    {
        NAudio.Wave.WaveOut sourceStream;
        NAudio.Wave.DirectSoundOut output;
        public Form1()
        {
            InitializeComponent();

            List<NAudio.Wave.WaveOutCapabilities> sources = new List<NAudio.Wave.WaveOutCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveOut.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveOut.GetCapabilities(i));
            }

            DeviceList.Items.Clear();

            foreach (var source in sources)
            {
                ListViewItem item = new ListViewItem(source.ProductName);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, source.Channels.ToString()));
                DeviceList.Items.Add(item);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string buttonTag = Convert.ToString((sender as Button).Tag);
            string buttonName = (sender as Button).Name;
            NAudio.Wave.WaveFileReader wave = new NAudio.Wave.WaveFileReader(buttonName + "." + buttonTag);

            if (checkBox1.Checked)
            {
                output = new NAudio.Wave.DirectSoundOut();
                output.Init(new NAudio.Wave.WaveChannel32(wave));
                output.Play();
            }
            
            if (DeviceList.SelectedItems.Count == 0) return;
            int deviceNumber = DeviceList.SelectedItems[0].Index;
            sourceStream = new NAudio.Wave.WaveOut();
            sourceStream.DeviceNumber = deviceNumber;

            sourceStream.Init(wave);
            sourceStream.Play();
        }

        private void STOP_Click(object sender, EventArgs e)
        {
            if (sourceStream != null)
            {
                sourceStream.Dispose();
                sourceStream = null;
            }
            if (output != null)
            {
                output.Dispose();
                output = null;
            }

        }
    }
}
