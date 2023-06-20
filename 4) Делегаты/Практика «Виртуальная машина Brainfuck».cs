using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        private readonly Dictionary<char, Action<IVirtualMachine>> _commands;
        private readonly byte[] _memory;
        private readonly string _instructions;

        public VirtualMachine(string program, int memorySize)
        {
            _memory = new byte[memorySize];
            _instructions = program;
            _commands = new Dictionary<char, Action<IVirtualMachine>>();
            MemoryPointer = 0;
            InstructionPointer = 0;
        }

        public byte[] Memory => _memory;

        public int MemoryPointer { get; set; }

        public string Instructions => _instructions;

        public int InstructionPointer { get; set; }

        public void RegisterCommand(char command, Action<IVirtualMachine> implementation)
        {
            _commands[command] = implementation;
        }

        public void Run()
        {
            while (InstructionPointer < Instructions.Length)
            {
                var command = Instructions[InstructionPointer];
                if (_commands.ContainsKey(command))
                    _commands[command](this);
                InstructionPointer++;
            }
        }
    }
}

