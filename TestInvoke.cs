using System.Collections;

namespace TestStuff;

public static class TestInvoke
{
    public static DelegateInvocationContext Delegate(Delegate d)
        => new DelegateInvocationContext(d);

    public class DelegateInvocationContext
    {
        private readonly Delegate @delegate;

        internal DelegateInvocationContext(Delegate @delegate)
        {
            this.@delegate = @delegate;
        }

        public void WithCombinationsOf(params object[] data)
        {
            foreach (var combo in getAllCombinations(data))
                @delegate.DynamicInvoke(combo);
        }

        //nesty stuff below...
        private IEnumerable<object[]> getAllCombinations(params object[] parameterOptions)
        {
            var root = new node(new List<node>());
            var current = new node[] { root };

            node[] AddProjected(node[] currentNodes, object o)
            {
                List<node> result = new();

                foreach (var n in currentNodes)
                {
                    foreach (var v in iterate(o))
                    {
                        var node = new node(v, n);
                        result.Add(node);
                    }
                }

                return result.ToArray();
            }

            foreach (var param in parameterOptions)
            {
                current = AddProjected(current, param);
            }

            IEnumerable<object> merge(node c)
            {
                var n = c;
                while (n.Value != null && n.Parent != null)
                {
                    yield return n.Value;
                    n = n.Parent;
                }
            }

            return current.Select(n => merge(n).Reverse().ToArray()).ToArray();
        }
        private class node
        {
            public readonly object Value;
            public readonly node Parent;

            public node(object value = null, node parent = null)
            {
                this.Value = value;
                this.Parent = parent;
            }
        }
        static IEnumerable<object> iterate(object parameter)
        {
            if (parameter is string)
            {
                yield return parameter;
                yield break;
            }

            if (parameter is IEnumerable e)
            {
                foreach (var v in e)
                    yield return v;

                yield break;
            }

            yield return parameter;
        }
    }
}