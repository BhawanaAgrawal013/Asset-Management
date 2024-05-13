namespace DataAccessLayer
{
    public interface IAssetRepository
    {
        public List<Assets> Display();

        public List<Assets> Get(int userid);
        public string Assign(Assets asset);

        public string UnAssign(Assets givenAsset);

        public string Remove(int id);
    }
}
