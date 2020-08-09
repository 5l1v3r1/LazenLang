﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing
{
    class Utils
    {
        public static T[] ParseSequence<T>(Parser parser, Func<Parser, T> consumer)
        {
            var sequence = new List<T>();
            Token lastCommaEaten = null;

            while (true)
            {
                try
                {
                    lastCommaEaten = null;
                    sequence.Add(parser.TryConsumer(consumer));
                    lastCommaEaten = parser.Eat(TokenInfo.TokenType.COMMA);
                } catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    if (lastCommaEaten != null)
                    {
                        throw new ParserError(
                            new UnexpectedTokenException(TokenInfo.TokenType.COMMA),
                            lastCommaEaten.Pos
                        );
                    }
                    break;
                }
            }

            return sequence.ToArray();
        }

        /*public static string PrettyArray<T, T1>(T[] list, Func<T, string> prettyPrinter)
        {
            string result = "";
            for (int i = 0; i < list.Count(); i++)
            {
                T elem = list[i];
                result += prettyPrinter(elem);
                if (i != list.Count()) result += ", ";
            }
            return "{" + result + "}";
        }*/
    }
}
