using System;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        private static readonly char[] Symbols =
       "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890"
       .ToCharArray();

        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));
            vm.RegisterCommand(',', b => b.Memory[b.MemoryPointer] = (byte)read());
            vm.RegisterCommand('+', b => b.Memory[b.MemoryPointer] = (byte)((b.Memory[b.MemoryPointer] + 1) % 256));
            vm.RegisterCommand('-', b => b.Memory[b.MemoryPointer] = (byte)((b.Memory[b.MemoryPointer] + 255) % 256));
            vm.RegisterCommand('>', b => b.MemoryPointer = (b.MemoryPointer + 1) % b.Memory.Length);
            vm.RegisterCommand('<', b => b.MemoryPointer = (b.MemoryPointer + b.Memory.Length - 1) % b.Memory.Length);
            RegisterSymbols(vm);
        }

        private static void RegisterSymbols(IVirtualMachine vm)
        {
            foreach (var symbol in Symbols)
            {
                var symbolByte = (byte)symbol;
                vm.RegisterCommand(symbol, b => b.Memory[b.MemoryPointer] = symbolByte);
            }
        }
    }
}
