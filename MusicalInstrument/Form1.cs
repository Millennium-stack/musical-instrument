using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace MusicalInstrument
{
    public partial class Form1 : Form
    {
        SignalGenerator sine = new SignalGenerator()
        {
            Type = SignalGeneratorType.Sin,
            Gain = 0.2,
        };

        WaveOutEvent player = new WaveOutEvent();

        public Form1()
        {
            InitializeComponent();

            player.Init(sine);

            trackFrequency.ValueChanged += (s, e) => sine.Frequency = trackFrequency.Value;
            trackFrequency.Value = 600;

            trackVolume.ValueChanged += (s, e) => player.Volume = trackVolume.Value / 100F;
            trackVolume.Value = 50;
        }

        private Point cursorPoritionOnMouseDown;
        private bool ButtonIsDown = false;
        private void TheMouseDown(object sender, MouseEventArgs e)
        {
            player.Play();

            cursorPoritionOnMouseDown = e.Location;
            ButtonIsDown = true;
        }

        private void TheMouseUp(object sender, MouseEventArgs e)
        {
            player.Stop();
            ButtonIsDown = false;
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            var dX = cursorPoritionOnMouseDown.X - e.X;
            var dY = cursorPoritionOnMouseDown.Y - e.Y;

            var vol = player.Volume - (dX / 100000f);
            var freq = sine.Frequency + (dY / 10f);

            if (ButtonIsDown)
            {
                player.Volume = (vol > 0) ? (vol < 1) ? vol : 1 : 0;
                sine.Frequency = (freq > 100) ? (freq < 1000) ? freq : 1000 : 100;

                trackFrequency.Value = (int)Math.Round(sine.Frequency);
                trackVolume.Value = (int)Math.Round(player.Volume * 100);
            }

            Text = $"Musical Instrument! ({dX}, {dY}), ({vol}, {freq})";
        }
    }
}
