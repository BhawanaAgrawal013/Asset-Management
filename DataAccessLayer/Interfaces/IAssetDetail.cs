namespace DataAccessLayer
{
    public interface IAssetDetail
    {
        public List<AssetDetail> Display();
        public string Add(AssetDetail asset);
        public AssetDetail Get(int id);
        public AssetDetail Update(int id, AssetDetail newlicense);
        public string Remove(int id);
        public List<AssetCategory> DisplayCategory();
    }
}
