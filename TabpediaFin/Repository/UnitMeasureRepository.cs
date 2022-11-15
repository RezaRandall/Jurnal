namespace TabpediaFin.Repository
{
    public class UnitMeasureRepository : BaseRepository, IUnitMeasureRepository
    {
        public UnitMeasureRepository(DbManager dbContext)
            : base(dbContext)
        {}

        public async Task<UnitMeasureDto> GetAllUnitMeasure()
        {
            string query = $@"SELECT 
                                    um.*, at.Id, au.Id 
                                    FROM UnitMeasure um 
                                    INNER JOIN AppTenant at ON at.Id = um.Id
                                    INNER JOIN AppUser au ON au.Id = um.Id ";
            using (var cn = _dbManager.CreateConnection())
            {
                return await cn.QueryFirstOrDefaultAsync<UnitMeasureDto>(query);
            }
        }
    }

    public interface IUnitMeasureRepository
    {
        Task<UnitMeasureDto> GetAllUnitMeasure();

        //Task<bool> UpsertRefreshTokenAsync(int userid, string refreshToken);

        //Task<bool> IsValidRefreshToken(int userId, string refreshToken);
    }
}
