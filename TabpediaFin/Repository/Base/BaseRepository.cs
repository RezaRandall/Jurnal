namespace TabpediaFin.Repository.Base;

public abstract class BaseRepository
{
    protected readonly DbManager _dbManager;

    public BaseRepository(DbManager dbManager)
    {
        _dbManager = dbManager;
    }
}
