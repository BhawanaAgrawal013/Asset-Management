namespace DataAccessLayer
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AssetManagmentContext _managmentContext;
        public AssetRepository(AssetManagmentContext managmentContext)
        {
            _managmentContext = managmentContext;
        }

        public List<Assets> Display()
        {
            var result = _managmentContext.Assets.Include(x => x.user).ToList();
            result = _managmentContext.Assets.Include(x => x.asset).ToList();
            return result;
        }


        public List<Assets> Get(int userid)
        {
            return _managmentContext.Assets.Where(i => i.UserId == userid).ToList();
        }

        private bool UserExists(int id)
        {
            if (_managmentContext.Users.Where(user => user.Id == id).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AssetExists(int id)
        {
            if(_managmentContext.AssetDetails.Where(x => x.Id == id).Any())
            {
                return true;
            }

            return false;
        }

        public string Assign(Assets asset)
        {
            if(UserExists((int)asset.UserId) && AssetExists(asset.AssetId))
            {
                if (asset.Status == "Assigned")
                {
                    AssetDetail assetdetail = _managmentContext.AssetDetails.Where(x => x.Id == asset.AssetId).First();
                    assetdetail.Quantity = assetdetail.Quantity - 1;
                }
/*                else
                {
                    int quantity = _managmentContext.AssetDetails.Where(x => x.Id == asset.AssetId).Select(x => x.Quantity).First() + 1;
                }*/
                _managmentContext.Assets.Add(asset);
                _managmentContext.SaveChanges();
                return "Sucessfully Assigned the Assets";
            }
            return "Assets cannot be Assigned";
        }

        public string UnAssign(Assets asset)
        {
            if (UserExists((int)asset.UserId) && AssetExists(asset.AssetId))
            {
                Assets oldasset = _managmentContext.Assets.Where(i => i.Id == asset.Id).FirstOrDefault();
                oldasset.AssetId = asset.AssetId;
                oldasset.UserId = asset.UserId;
                oldasset.Status = asset.Status;
                oldasset.DateOfReturn = asset.DateOfReturn;
                oldasset.Reason = asset.Reason;
                if(oldasset.Status == "Assigned")
                {
                    /*                    int quantity = _managmentContext.AssetDetails.Where(x => x.Id == asset.AssetId).Select(x => x.Quantity).First() - 1;*/
                    AssetDetail assetdetail = _managmentContext.AssetDetails.Where(x => x.Id == asset.AssetId).First();
                    assetdetail.Quantity = assetdetail.Quantity - 1;
                }
                if(oldasset.Status == "Return" || oldasset.Status == "Unassigned")
                {
                    /*                    int quantity = _managmentContext.AssetDetails.Where(x => x.Id == asset.AssetId).Select(x => x.Quantity).First() + 1;*/
                    AssetDetail assetdetail = _managmentContext.AssetDetails.Where(x => x.Id == asset.AssetId).First();
                    assetdetail.Quantity = assetdetail.Quantity + 1;
                }
                _managmentContext.SaveChanges();
                return "Unassigned successfully";
            }
            return "Invalid Request";
        }
        
        public string Remove(int id)
        {
            Assets asset = _managmentContext.Assets.Where(i => i.Id == id).FirstOrDefault();
            _managmentContext.Assets.Remove(asset);
            _managmentContext.SaveChanges();
            return "Assets record deleted successfully";
        }
    }
}
