using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2021
{
    class ConsoleSpinner : IDisposable
    {
        private const string _sequence = @"/-\|";
        private int _counter = 0;
        private bool _active;
        private int _consoleLine;
        private string _text;
        private readonly Thread _thread;

        public ConsoleSpinner()
        {
            _thread = new Thread(Spin);
        }

        public void Start(string text = null)
        {
            _active = true;
            _consoleLine = Console.CursorTop;
            _text = text;
            if (!_thread.IsAlive) _thread.Start();
        }

        public void Stop()
        {
            _active = false;
            Console.SetCursorPosition(0, _consoleLine);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, _consoleLine);
        }

        private void Spin()
        {
            while (_active)
            {
                _counter++;
                Console.SetCursorPosition(0, _consoleLine);
                Console.Write($"{_text} {_sequence[_counter % _sequence.Length]}");
                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
