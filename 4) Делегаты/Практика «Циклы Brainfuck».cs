using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        private static Dictionary<int, int> bracketsPositions;

        public static void RegisterTo(IVirtualMachine vm)
        {
            bracketsPositions = new Dictionary<int, int>();
            var stackBrackets = new Stack<int>();

            for (int i = 0; i < vm.Instructions.Length; i++)
            {
                if (vm.Instructions[i] == '[')
                {
                    stackBrackets.Push(i);
                }
                else if (vm.Instructions[i] == ']')
                {
                    int startBracket = stackBrackets.Pop();
                    int endBracket = i;
                    bracketsPositions[startBracket] = endBracket;
                    bracketsPositions[endBracket] = startBracket;
                }
            }

            vm.RegisterCommand('[', execute =>
            {
                if (execute.Memory[execute.MemoryPointer] == 0)
                {
                    execute.InstructionPointer = bracketsPositions[execute.InstructionPointer];
                }
            });

            vm.RegisterCommand(']', execute =>
            {
                if (execute.Memory[execute.MemoryPointer] != 0)
                {
                    execute.InstructionPointer = bracketsPositions[execute.InstructionPointer];
                }
            });
        }
    }
}
