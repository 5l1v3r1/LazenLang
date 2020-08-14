﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    class FuncDecl : Instr
    {
        public Signature Signature;
        public Block Block;

        public FuncDecl(Signature signature, Block block)
        {
            Signature = signature;
            Block = block;
        }

        public static FuncDecl Consume(Parser parser)
        {
            Signature signature = null;
            Block block = null;

            signature = parser.TryConsumer(Signature.Consume);
            block = parser.TryConsumer((Parser p) => Block.Consume(p));

            return new FuncDecl(signature, block);
        }

        public override string Pretty()
        {
            return $"FuncDecl(signature: {Signature.Pretty()}, block: {Block.Pretty()})";
        }
    }
}
