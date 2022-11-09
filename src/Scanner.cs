namespace CSLox;

public class Scanner
{
    private readonly string _source;
    private readonly List<Token> _tokens;

    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public Scanner(string source)
    {
        _source = source;
        _tokens = new List<Token>();
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;

            ScanToken();
        }
        
        _tokens.Add(new Token(TokenType.Eof, string.Empty, null, _line));

        return _tokens;
    }

    private bool IsAtEnd() => _current >= _source.Length;

    private void ScanToken()
    {
        char @char = Advance();

        TokenType tokenType = @char switch
        {
            '(' => TokenType.LeftParen,
            ')' => TokenType.RightParen,
            '{' => TokenType.LeftBrace,
            '}' => TokenType.RightBrace,
            ',' => TokenType.Comma,
            '.' => TokenType.Dot,
            '-' => TokenType.Minus,
            '+' => TokenType.Plus,
            ';' => TokenType.Semicolon,
            '*' => TokenType.Star,
            '!' => MatchNext('=') ? TokenType.BangEqual : TokenType.Bang,
            '=' => MatchNext('=') ? TokenType.EqualEqual : TokenType.Equal,
            '<' => MatchNext('=') ? TokenType.LessEqual : TokenType.Less,
            '>' => MatchNext('=') ? TokenType.GreaterEqual : TokenType.Greater,
            '/' => MatchSlash(),
            _ => TokenType.Invalid
        };

        if (tokenType is TokenType.Invalid)
        {
            CSLox.Error(_line, "Unexpected character");
        }

        if (tokenType != TokenType.None)
        {
            AddToken(tokenType);
        }
    }

    private bool MatchNext(char expected)
    {
        if (IsAtEnd()) return false;
        if (_source[_current] != expected) return false;

        _current++;

        return true;
    }

    private TokenType MatchSlash()
    {
        if (!MatchNext('/')) return TokenType.Slash;

        while (Peek() != '\n' && !IsAtEnd())
            Advance();

        return TokenType.None;
    }

    private char Peek() => IsAtEnd() ? '\0' : _source[_current];

    private char Advance() => _source[_current++];

    private void AddToken(TokenType tokenType)
    {
        AddToken(tokenType, null);
    }

    private void AddToken(TokenType tokenType, object? literal)
    {
        var text = _source.Substring(_start, _current);
        
        _tokens.Add(new Token(tokenType, text, literal, _line));
    }
}