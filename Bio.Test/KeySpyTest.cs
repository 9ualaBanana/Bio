using FluentAssertions;
using Xunit;

namespace Bio.Test;

public class KeySpyTest
{
    [Fact]
    public void IsActive_ReturnsFalseBeforeActivated()
    {
        using var keySpy = new KeySpy();
        
        keySpy.IsActive.Should().BeFalse();
    }
    
    [Fact]
    public void IsActive_ReturnsTrueAfterActivated()
    {
        using var keySpy = new KeySpy();

        keySpy.Activate();
        
        keySpy.IsActive.Should().BeTrue();
    }

    [Fact]
    public void IsActive_ReturnsFalseAfterDisposed()
    {
        using var keySpy = new KeySpy();

        keySpy.Activate();
        keySpy.Deactivate();
        
        keySpy.IsActive.Should().BeFalse();
    }
}
