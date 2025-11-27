using Game.Core.DTOs;
using Game.Infrastructure.Persistence;
using System;

public class SaveManager : ISaveManager
{
    private readonly IRepo<SaveData> _repo;

    public SaveManager(IRepo<SaveData> repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public SaveData Load()
    {
        return _repo.Load() ?? new SaveData();
    }

    public void Save(SaveData data)
    {
        _repo.Save(data);
    }

    public void Delete()
    {
        if (_repo is JsonSaveSystem js) js.Delete();
    }
}
