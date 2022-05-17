#if DEBUG

namespace TestStuff;

public class getCombinationsShould
{
    [Fact]
    public void ReturnAllCombos()
    {
        var data = new object[]
        {
            new[] { 1, 2 },
            "fix",
            new[] { "A", "B"}
        };
        
        TestInvoke.Delegate(test).WithCombinationsOf(data);
        var r = result.ToArray();
        Assert.Equal("1,fix,A",r[0]);
        Assert.Equal("1,fix,B",r[1]);
        Assert.Equal("2,fix,A",r[2]);
        Assert.Equal("2,fix,B",r[3]);
        Assert.Equal(4, r.Length);
    }

    private List<string> result = new ();
    void test(int a, string b, string c)
    {
        Assert.Equal("fix",b);
        result.Add($"{a},{b},{c}");
    }
}

#endif