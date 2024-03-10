using System.Text;

namespace maui0; 

public interface Expression
{
    public record struct Literal (string Value) : Expression
    {
    }  

    public record struct ExpressionList (Expression[] Expressions) : Expression
    {
        public override string ToString() {
            var sb = new StringBuilder(); 
            sb.Append("("); 
            foreach (var exp in Expressions) {
                sb.Append(exp.ToString()); 
                sb.Append(" "); 
            }
            sb.Append(")"); 
            return sb.ToString(); 
        }
    } 

    public static Expression[] Parse0(Span<Rune> runes, ref int index, out bool endByRightBracket) {
        var rst = new List<Expression>(); 
        var buf = new StringBuilder(); 
        while (true) {
            bool toStr = false; 
            // check buf end? 
            bool bufEmpty = buf.Length == 0; 
            // check end. 
            if (index >= runes.Length) {
                if (!bufEmpty) {
                    rst.Add(new Literal(buf.ToString())); 
                }
                endByRightBracket = false; 
                return rst.ToArray(); 
            } 
            // whitespace check
            Rune cur = runes[index]; 
            if (cur.IsAscii) {
                char c = (char ) cur.Value; 
                // meaningless split 
                if (c == ' ' || c == '\n' || c == '\r' || c == '\t') {
                    index += 1; 
                    toStr = true; 
                }
                // left bracket | right bracket 
                if (c == '(' || c == ')') {
                    toStr = true; 
                } 
                if (toStr && !bufEmpty) {
                    rst.Add(new Literal(buf.ToString())); 
                    buf.Clear(); 
                }
                if (c == '(') {
                    index += 1; 
                    var exps = Parse0(runes, ref index, out var ebr); 
                    if (ebr) {
                        rst.Add(new ExpressionList(exps)); 
                    } else {
                        throw new Exception("Expecting ')' but got EOF"); 
                    }
                }
                if (c == ')') {
                    index += 1; 
                    endByRightBracket = true; 
                    return rst.ToArray(); 
                }
                if (toStr) {
                    continue; 
                }
            } 
            buf.Append(cur); 
            index += 1;  
        }
    }

    public static Expression Parse(string input) {
        var runes = input.EnumerateRunes(); 
        var runeList = new List<Rune>(); 
        foreach (var rune in runes) {
            runeList.Add(rune); 
        } 
        var runeArray = runeList.ToArray(); 
        int idx = 0; 
        // ParseImpl(runeArray, ref idx, out var exp, out var trim); 
        var exp = Parse0(runeArray, ref idx, out var trim); 
        if (trim) {
            throw new Exception("Expecting EOF but got ')'"); 
        }
        if (exp.Length != 1) {
            var expPack = new ExpressionList(exp); 
            throw new Exception($"Expecting only one expression but got {exp.Length} expressions: {expPack}"); 
        }
        return exp[0]; 
        // return new ExpressionList(exp); 
    }

    private static void ParseImpl(Span<Rune> toParse, ref int idx, out Expression[] exp, out bool trim) {
        var rst = new List<Expression>(); 
        // empty triming 
        var buffer = new StringBuilder(); 
        trim = false;
        while (true) {
            // not empty triming! 
            bool flag = false; 
            bool ret = false; 
            while (idx < toParse.Length && toParse[idx].IsAscii && 
                (char.IsWhiteSpace((char)toParse[idx].Value) || (char)toParse[idx].Value == '\n' || (char)toParse[idx].Value == '\r' || (char)toParse[idx].Value == '\t')) { 
                idx += 1; 
                flag = true; 
            }
            if (idx < toParse.Length && toParse[idx].IsAscii && (char)toParse[idx].Value == ')') {
                idx += 1; 
                flag = true; 
                ret = true; 
            } 
            if (idx >= toParse.Length) {
                if (buffer.Length != 0) {
                    flag = true; 
                } 
            }
            if (flag && buffer.Length != 0) {
                rst.Add(new Literal(buffer.ToString())); 
                buffer.Clear();  
            }
            if (ret) {
                exp = [.. rst]; 
                return ; 
            }
            if (idx >= toParse.Length) {
                exp = [.. rst]; 
                trim = true; 
                return ; 
            }
            if (toParse[idx].IsAscii && (char )toParse[idx].Value == '(') {
                idx += 1; 
                ParseImpl(toParse, ref idx, out var subExp, out trim); 
                if (trim) {
                    exp = [.. rst]; 
                    return ; 
                }
                rst.Add(new ExpressionList(subExp));
                if (idx >= toParse.Length) {
                    exp = [.. rst]; 
                    trim = true; 
                    return ; 
                } else if (toParse[idx].IsAscii && (char ) toParse[idx].Value == ')') {
                    idx += 1; 
                } else {
                    throw new Exception($"Expecting ')' but got {toParse[idx]}");
                }
            } else {
                bool f = true; 
                while (f && idx < toParse.Length) {
                    var v = toParse[idx].Value; 
                    var isA = toParse[idx].IsAscii; 
                    if (isA && (char ) v == '(' || (char ) v == ')' || char.IsWhiteSpace((char) v) || (char) v == '\n' || (char) v == '\r' || (char) v == '\t') {
                        f = false; 
                    } else {
                        buffer.Append(toParse[idx]);
                        idx += 1; 
                    } 
                }
            }
        }
        throw new Exception("Unreachable code"); 
    }
}
