using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

namespace maui0;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif


		Console.WriteLine("start"); 
		// string rl = "((lambda x x) z)";
		// string rl = "((lambda t f u t) a b c)"; 
		string rl = "(((lambda true false unknown true? (true? true false unknown)) (lambda t f u t) (lambda t f u f) (lambda t f u u)) (lambda t f q (q t f f)))"; 
		Console.WriteLine($"Read line: {rl}");
		var rst = maui0.Expression.Parse(rl); 
		Console.WriteLine($"Parsed: {rst}"); 
		var s = new maui0.Solver(); 
		var n = s.Normalize(rst); 
		Console.WriteLine($"Normalize: {n}");
		var rst2 = s.SolveNull(n); 
		Console.WriteLine($"Solved: {rst2}"); 
		var rst3 = s.Solve1(n, ImmutableDictionary<Expression.Literal, Expression>.Empty); 
		Console.WriteLine($"Solved1: {rst3}"); 
		return builder.Build();
	}
}
