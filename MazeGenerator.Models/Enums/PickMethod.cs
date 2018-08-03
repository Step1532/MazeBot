namespace MazeGenerator.Models.Enums
{
    //TODO: Можно удалить enum и связанную с ним логику, оставить один тип
    public enum PickMethod : byte
    {
        Newest,
        Oldest,
        Random,
        Cyclic
    }
}