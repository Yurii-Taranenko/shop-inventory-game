using Game.Core.DTOs;

public interface ISaveManager
{
    SaveData Load();
    void Save(SaveData data);
    void Delete();
}
