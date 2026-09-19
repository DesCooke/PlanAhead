using MethodBoundaryAspect.Fody.Attributes;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PlanAhead.Infrastructure.Logging;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Method,
    AllowMultiple = false)]
public sealed class MethodLoggingAttribute
    : OnMethodBoundaryAspect
{
    private sealed class MethodState
    {
        public Stopwatch Stopwatch { get; } =
            Stopwatch.StartNew();

        public int Depth { get; init; }

        public bool Ended { get; set; }
    }

    private static readonly AsyncLocal<int> _depth = new();

    public override void OnEntry(
        MethodExecutionArgs args)
    {
        // Ignore compiler generated methods such as:
        // <RestoreSessionAsync>d__12.MoveNext
        if (ShouldIgnore(args.Method))
            return;

        var methodName = GetMethodName(args);

        var currentDepth = _depth.Value;

        var state = new MethodState
        {
            Depth = currentDepth
        };

        args.MethodExecutionTag = state;

        Write(
            $"{Indent(currentDepth)}> {methodName}");

        _depth.Value = currentDepth + 1;
    }

    public override void OnExit(
        MethodExecutionArgs args)
    {
        // Ignore compiler generated methods
        if (ShouldIgnore(args.Method))
            return;

        if (args.MethodExecutionTag
            is not MethodState state)
        {
            return;
        }

        if (state.Ended)
            return;

        state.Ended = true;

        _depth.Value = Math.Max(
            0,
            state.Depth);

        var elapsed =
            state.Stopwatch.ElapsedMilliseconds;

        var methodName =
            GetMethodName(args);

        Write(
            $"{Indent(state.Depth)}< " +
            $"{methodName} [{elapsed}ms]");
    }

    public override void OnException(
        MethodExecutionArgs args)
    {
        if (ShouldIgnore(args.Method))
            return;

        if (args.MethodExecutionTag
            is not MethodState state)
        {
            Write(
                $"! {GetMethodName(args)} " +
                $"EXCEPTION: {args.Exception.Message}");

            args.FlowBehavior =
                FlowBehavior.RethrowException;

            return;
        }

        var methodName =
            GetMethodName(args);

        Write(
            $"! {methodName} " +
            $"EXCEPTION: {args.Exception.Message}");

        if (!state.Ended)
        {
            state.Ended = true;

            MethodLoggingService.Depth =
                Math.Max(0, state.Depth);
        }

        // VERY IMPORTANT:
        // Don't swallow the exception.
        args.FlowBehavior =
            FlowBehavior.RethrowException;
    }

    private static bool ShouldIgnore(
        MethodBase method)
    {
        // The C# compiler generates async state
        // machine methods called MoveNext().
        if (method.Name == "MoveNext")
            return true;

        // Also ignore anything explicitly marked
        // as compiler generated.
        if (method.IsDefined(
                typeof(CompilerGeneratedAttribute),
                false))
        {
            return true;
        }

        // And ignore methods belonging to a
        // compiler-generated type.
        if (method.DeclaringType?.IsDefined(
                typeof(CompilerGeneratedAttribute),
                false) == true)
        {
            return true;
        }

        return false;
    }

    private static string GetMethodName(
        MethodExecutionArgs args)
    {
        var declaringType =
            args.Method.DeclaringType?.Name
            ?? "UnknownType";

        return $"{declaringType}.{args.Method.Name}";
    }

    private static string Indent(
        int depth)
    {
        return new string(
            ' ',
            depth * 2);
    }

    private static void Write(
        string message)
    {
        Debug.WriteLine(message);

        MethodLoggingService.Write(message);
    }
}