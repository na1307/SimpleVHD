namespace SimpleVhd.Test;

public class SettingsTest {
    [Fact]
    public void TestSettings() => Assert.Throws<FileNotFoundException>(() => Settings.Instance);
}
