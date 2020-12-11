namespace BlackPearl.Prism.Module
{
    public interface IModuleInfo
    {
        string Id { get; }
        string Header { get; }
        object DisplayImage { get; }
        string NavigationPath { get; }
    }
}
