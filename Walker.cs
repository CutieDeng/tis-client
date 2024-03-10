namespace maui0; 

using System.Collections.Immutable;
using System.Linq.Expressions;

public class Walker {

}

/// <summary>
///  def the lambda calculus context here 
/// </summary>
public class Context {

}

public class Value {

}


public class Coolean {

}

public class Solver {

    public Expression SolveNull(Expression exp) => 
        Solve(exp, ImmutableDictionary<Expression.Literal, Expression>.Empty); 

    public Expression Normalize(Expression exp) {
        if (exp is Expression.Literal l) {
            return l; 
        } 
        if (exp is Expression.ExpressionList el) {
            Expression[] exprs = el.Expressions; 
            var op = Normalize(exprs[0]); 
            var args = exprs[1..]; 
            Expression toParse; 
            if (op is Expression.Literal lam) {
                if (lam.Value == "lambda") {
                    if (args[0] is not Expression.Literal arg) {
                        throw new Exception("invalid lambda expression"); 
                    }
                    if (args.Length != 2) {
                        // collect it 
                        var other2 = args[1..]; 
                        Expression otherExp = new Expression.ExpressionList([lam, .. other2]);
                        toParse = otherExp; 
                    } else {
                        toParse = args[1]; 
                    }
                    return new Expression.ExpressionList([lam, arg, Normalize(toParse)]); 
                } 
            }
            var newExp4 = new Expression.ExpressionList([op, Normalize(args[0])]); 
            if (args.Length == 1) {
                return newExp4;
            }
            var others = args[1..]; 
            var newExp3 = new Expression.ExpressionList([newExp4, ..others]); 
            return Normalize(newExp3); 
        } 
        throw new Exception("Invalid Exp"); 
    }

    public Expression Solve0(Expression exp, (Expression.Literal, Expression)? map) {
        if (exp is Expression.Literal l) {
            if (map.HasValue && map.Value.Item1 == l) {
                return Solve0(map.Value.Item2, map); 
            }
            return l; 
        } 
        
        throw new NotImplementedException(); 
    }

    public Expression Solve(Expression exp, ImmutableDictionary<Expression.Literal, Expression> ctx, Expression? toSubstitute = null) {
        if (exp is Expression.Literal l) {
            if (ctx.TryGetValue(l, out var v)) {
                // return Solve(v, ctx.Remove(l), toSubstitute);  
                return Solve(v, ctx, toSubstitute);  
            }
            if (toSubstitute != null) {
                throw new Exception($"invalid substitution, {l} is literal, but attempt to do substitution with {toSubstitute}"); 
            }
            return l; 
        } 
        if (exp is Expression.ExpressionList el) {
            Expression[] exprs = el.Expressions; 

            var op = Solve(exprs[0], ctx, null); 

            var args = exprs[1..]; 

            // consider lambda 
            if (op is Expression.Literal lam) {
                if (lam.Value == "lambda") {
                    if (args[0] is not Expression.Literal arg) {
                        throw new Exception("invalid lambda expression"); 
                    }
                    if (args.Length != 2) {
                        // collect it 
                        var other2 = args[1..]; 
                        var newEExp = new Expression.ExpressionList([lam, .. other2]);
                        var newExp2 = new Expression.ExpressionList([lam, args[0], newEExp]); 
                        Console.WriteLine($"src: {exp}, normalize: {newExp2}");
                        return Solve(newExp2, ctx, toSubstitute); 
                    }
                    if (toSubstitute != null) {
                        var ctx2 = ctx.Add(arg, toSubstitute!);
                        return Solve(args[1], ctx2, null);
                    } else {
                        var args1 = Solve(args[1], ctx.Remove(arg), null); 
                        var newExp3 = new Expression.ExpressionList([lam, args[0], args1]); 
                        return newExp3; 
                    }
                } 
            }
            // normal substitute 
            for (int i = 0; i < args.Length; i++) {
                args[i] = Solve(args[i], ctx, null); 
            } 
            try {
                var sop = Solve(op, ctx, args[0]); 
                var others = args[1..]; 
                if (others.Length == 0) {
                    return sop; 
                }
                var newExp = new Expression.ExpressionList([sop, ..others]);
                return Solve(newExp, ctx, toSubstitute); 
            } catch (Exception ) {
                if (toSubstitute == null) {
                    return exp; 
                }
                throw; 
            }
        }
        throw new NotImplementedException(); 
    } 
}
