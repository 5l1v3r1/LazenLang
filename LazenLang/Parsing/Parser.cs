﻿using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LazenLang.Parsing
{
    interface IParserErrorContent { }

    // "Parser class" exceptions
    struct FailedEatToken : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public FailedEatToken(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    struct FailedConsumer : IParserErrorContent
    {}

    struct NoTokenLeft : IParserErrorContent
    {}

    // "Real" exceptions
    struct UnexpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public UnexpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    struct ExpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public ExpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    struct ExpectedElementException : IParserErrorContent
    {
        public string Message { get; }
        public ExpectedElementException(string message)
        {
            Message = message;
        }
    }

    struct InvalidElementException : IParserErrorContent
    {
        public string Message { get; }
        public InvalidElementException(string message)
        {
            Message = message;
        }
    }

    struct MysticalLineException : IParserErrorContent
    {}

    struct InvalidDoubleLit : IParserErrorContent
    {
        public string Value { get; }
        public InvalidDoubleLit(string value)
        {
            Value = value;
        }
    }

    struct InvalidCharLit : IParserErrorContent
    {
        public string Value { get; }
        public InvalidCharLit(string value)
        {
            Value = value;
        }
    }

    // ---
    class ParserError : Exception
    {
        public IParserErrorContent Content;
        public CodePosition Position;

        public ParserError(IParserErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
        }

        public bool IsExceptionFictive()
        {
            return Content is FailedEatToken ||
                   Content is FailedConsumer;
        }
    }
    
    class Parser
    {
        public List<Token> Tokens { get; set; }
        public Token LastTokenEaten { get; set; }
        public Token ActualToken { get; set; }
        public CodePosition Cursor { get; set; }

        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            ActualToken = tokens[0];
            Cursor = new CodePosition(0, 0);
        }

        public void SetActualToken()
        {
            if (Tokens.Count == 0)
                return;
            ActualToken = Tokens[0];
            Cursor = Tokens[0].Pos;
        }

        public static Func<Parser, Token> CurryEat(TokenInfo.TokenType tokenType) =>
            (Parser p) => p.Eat(tokenType);

        public Token Eat(TokenInfo.TokenType tokenType)
        {
            if (Tokens.Count == 0)
                throw new ParserError(new NoTokenLeft(), Cursor);
            else if (ActualToken.Type != tokenType)
                throw new ParserError(new FailedEatToken(tokenType), Cursor);

            Token token = ActualToken;
            Tokens.RemoveAt(0);
            SetActualToken();

            LastTokenEaten = token;
            return token;
        }

        public Token TryManyEats(TokenInfo.TokenType[] tokenTypes)
        {
            ParserError lastError = null;

            foreach (TokenInfo.TokenType tokType in tokenTypes)
            {
                try
                {
                    return Eat(tokType);
                } catch (ParserError ex)
                {
                    lastError = ex;
                }
            }

            throw lastError;
        }

        public T TryConsumer<T>(Func<Parser, T> consumer)
        {
            var oldTokens = new Token[Tokens.Count];
            Tokens.CopyTo(oldTokens);

            try
            {
                return consumer(this);
            } catch (ParserError ex)
            {
                Tokens = oldTokens.ToList();
                throw ex;
            }
        }

        public T TryManyConsumers<T>(Func<Parser, T>[] consumers)
        {
            ParserError lastError = null;


            foreach (var consumer in consumers)
            {
                try
                {
                    return TryConsumer(consumer);
                } catch (ParserError ex)
                {
                    if (ex.IsExceptionFictive())
                    {
                        lastError = ex;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            throw lastError;
        }
    }
}