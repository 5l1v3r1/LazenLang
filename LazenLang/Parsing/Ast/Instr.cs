﻿using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast
{
    struct InstrNode
    {
        public Instr Instruction;
        public CodePosition Position;

        public InstrNode(Instr instruction, CodePosition position)
        {
            Instruction = instruction;
            Position = position;
        }
    }

    abstract class Instr
    {}
}
