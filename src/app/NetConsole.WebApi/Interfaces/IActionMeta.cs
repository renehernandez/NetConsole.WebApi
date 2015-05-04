namespace NetConsole.WebApi.Interfaces
{
    public interface IActionMeta
    {

        string Name { get; set; }

        bool Default { get; set; }

        string ReturnType { get; set; } 

        string[] ParamsType { get; set; }

    }
}