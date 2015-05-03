namespace NetConsole.WebApi.Interfaces
{
    public interface ICommandMetadata<T> : IMetadata where T : IActionMeta  
    {
         
        int ActionsCount { get; set; }

        T[] ActionsMeta { get; set; }

        string Name { get; set; }

        string Overview { get; set; }

    }
}