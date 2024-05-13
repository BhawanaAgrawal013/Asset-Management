namespace DataAccessLayer
{
    public class AssetDetailRepository  : IAssetDetail
    {
        private readonly AssetManagmentContext _managmentContext;
        public AssetDetailRepository(AssetManagmentContext _managmentContext)
        {
            this._managmentContext = _managmentContext;
        }

        public List<AssetDetail> Display()
        {
            return _managmentContext.AssetDetails.Include(x => x.assetCategory).ToList();
        }

        public List<AssetCategory> DisplayCategory()
        {
            return _managmentContext.AssetCategories.ToList();
        }
        public AssetDetail Get(int id)
        {
            try
            {
                return _managmentContext.AssetDetails.Where(i => i.Id == id).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public string Add(AssetDetail asset)
        {
            if (asset == null)
            {
                return "Asset cannot be added, the object seems to be of null type";
            }
            else
            {
                _managmentContext.AssetDetails.Add(asset);
                _managmentContext.SaveChanges();
                return "Sucessfully Added asset";
            }
        }

        public AssetDetail Update(int id, AssetDetail newasset)
        {
            AssetDetail oldasset = _managmentContext.AssetDetails.Where(i => i.Id == id).FirstOrDefault();
            oldasset.Name = newasset.Name;
            oldasset.Source = newasset.Source;
            oldasset.Type = newasset.Type;
            oldasset.Date = newasset.Date;
            oldasset.Quantity = newasset.Quantity;
            _managmentContext.SaveChanges();
            return oldasset;
        }

        public string Remove(int id)
        {
            AssetDetail asset = Get(id);
            if(asset == null)
            {
                return "Asset not deleted";
            }
            _managmentContext.AssetDetails.Remove(asset);
            _managmentContext.SaveChanges();
            return "Asset deleted successfully";
        }
    }
}
