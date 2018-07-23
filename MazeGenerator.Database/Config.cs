namespace MazeGenerator.Database
{
    public static class Config
    {
        public static string ConnectionString =>
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Maze;Integrated Security=True;Connect Timeout=30;";
    }
}