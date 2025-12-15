using System;
using System.IO;
using System.Media;
using static System.Math;

namespace Triggernometry.Common.Audio
{
    public static class WaveGenerator
    {

        public static readonly Func<double, double> SineWaveFunc
            = phase => Sin(phase);

        public static readonly Func<double, double> SquareWaveFunc 
            = phase => Tanh(5 * Sin(phase)); // smoothed square wave

        public static readonly Func<double, double> TriangleWaveFunc
            = phase =>
            {
                double t = phase / (2 * PI) % 1.0;
                return t < 0.5 ? 4 * t - 1 : -4 * t + 3;
            };

        public static short[] GenerateWave(Func<double, double> waveFunc, int freq, int durationMs, int sampleRate, double volume = 1.0)
        {
            int samples = (int)(sampleRate * durationMs / 1000.0);
            short[] buffer = new short[samples];

            for (int i = 0; i < samples; i++)
            {
                double t = (double)i / sampleRate; // t/s
                double x = waveFunc(2 * PI * freq * t) * volume;
                buffer[i] = (short)(x * short.MaxValue);
            }

            return buffer;
        }

        private static byte[] GenerateWavBytes(Func<double, double> waveFunc, int frequency, int durationMs, int sampleRate = 44100, double volume = 1.0)
        {
            var buffer = GenerateWave(waveFunc, frequency, durationMs, sampleRate, volume);
            using (var memStream = new MemoryStream())
            using (var binWriter = new BinaryWriter(memStream))
            {
                int dataLength = buffer.Length * sizeof(short);
                int fileLength = 36 + dataLength;

                // === WAV Header ===
                binWriter.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
                binWriter.Write(fileLength);
                binWriter.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));
                binWriter.Write(16); // fmt chunk size
                binWriter.Write((short)1); // PCM
                binWriter.Write((short)1); // mono
                binWriter.Write(sampleRate);
                binWriter.Write(sampleRate * 2); // byte rate
                binWriter.Write((short)2); // block align
                binWriter.Write((short)16); // bits per sample
                binWriter.Write(System.Text.Encoding.ASCII.GetBytes("data"));
                binWriter.Write(dataLength);

                // === PCM Data ===
                foreach (var s in buffer)
                    binWriter.Write(s);

                return memStream.ToArray();
            }
        }

        private static void PlaySyncWav(byte[] wavBytes)
        {
            using (var memStream = new MemoryStream(wavBytes))
            using (var player = new SoundPlayer(memStream))
            {
                player.PlaySync();
            }
        }

        public static void PlaySyncWav(Func<double, double> waveFunc, int frequency, int durationMs, int sampleRate = 44100, double volume = 1.0)
            => PlaySyncWav(GenerateWavBytes(waveFunc, frequency, durationMs, sampleRate, volume));
        
        public static void PlaySyncBeep(int frequency, int durationMs, double volume = 1.0)
            => PlaySyncWav(SineWaveFunc, frequency, durationMs, volume: volume);

    }

}
