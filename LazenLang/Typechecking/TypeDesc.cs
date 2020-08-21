﻿using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    abstract class TypeDesc
    { }

    abstract class AtomType : TypeDesc
    { }

    class NameType : TypeDesc
    {
        public Identifier Name;
        public NameType(Identifier name)
        {
            Name = name;
        }
    }

    class TypeApp : TypeDesc
    {
        public NameType BaseType { get; }
        public TypeDesc[] Generics { get; }
        public TypeApp(NameType baseType, TypeDesc[] generics)
        {
            BaseType = baseType;
            Generics = generics;
        }
    }

    class ArrayType : TypeDesc
    {
        public TypeDesc ElementsType { get; }
        public ArrayType(TypeDesc elementsType)
        {
            ElementsType = elementsType;
        }
    }

    class FuncType : TypeDesc
    {
        public TypeDesc[] Domain { get; }
        public TypeDesc Codomain { get; }
        public FuncType(TypeDesc[] domain, TypeDesc codomain)
        {
            Domain = domain;
            Codomain = codomain;
        }
    }

    class TypeVariable : TypeDesc
    {
        public int Num { get; }
        public TypeVariable(int num)
        {
            Num = num;
        }
    }

    class BoolType : AtomType {}
    class CharType : AtomType {}
    class DoubleType : AtomType {}
    class IntType : AtomType {}
    class StringType : AtomType {}
    class VoidType : AtomType {}
}