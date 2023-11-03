using DataLayer.Entities.Blogs;
using DataLayer.Entities.Supplementary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface ICompService
    {
        #region Generic
        public void SaveChanges();
        public Task SaveChangesAsync();
        public Task<Blog> GetBlogByIdAsync(string GId);
        public Task<SitePage> GetSitePageByEnNameAsync(string EnName);
        public Task<InitialInfo> GetInitialInfoAsync();
        #endregion Generic
        #region Slider
        public void CreateSlider(Slider slider);
        public Task<List<Slider>> GetSlidersAsync();
        public void UpdateSlider(Slider slider);
        public Task RemoveSlider(int id);
        public Task<Slider> GetSliderById(int id);
        #endregion
        #region Banner
        public void CreateBannerNextSlider(BannerNextSlider bannerNextSlider);
        public Task<List<BannerNextSlider>> GetBannerNextSlidersAsync();
        public Task<BannerNextSlider> GetBannerNextSliderById(int Id);
        public void EditBanner(BannerNextSlider bannerNextSlider);
        #endregion Banner
        #region Key 
        public Task<List<UniqueKey>> GetUniqueKeysAsync();
        public List<string> AllKeys();
        public void CreateUniqueKey(UniqueKey uniqueKey);
        #endregion Key
        #region State
        public Task<List<State>> GetStatesAsync();
        public Task<State> GetStateByIdAsync(int Id);
        #endregion State
        #region County 
        public Task<County> GetCountyByIdAsync(int countyId);
        public Task<List<County>> GetCountiesAsync();
        public Task<List<County>> GetCountiesOfStateAsync(int stateId);
        #endregion County
        #region FractionSlider
        public void CreateFractionSlider(FractionSlider fractionSlider);
        public void UpdateFractionSlider(FractionSlider fractionSlider);
        public Task<List<FractionSlider>> GetFractionSlidersAsync();
        public Task<FractionSlider> GetFractionByIdAsync(int Id);
        public void RemoveFractionSlider(FractionSlider fractionSlider);
        public void CreateFSImage(FSImage fSImage);
        public void UpdateFSImage(FSImage fSImage);
        public Task<List<FSImage>> GetFSImagesAsync();
        public Task<FSImage> GetFSImageByIdAsync(int Id);
        public void RemoveFSImage(FSImage fSImage);

        public void CreateFSText(FSText fSText);
        public void UpdateFSText(FSText fSText);
        public Task<List<FSText>> GetFSTextsAsync();
        public Task<FSText> GetFSTextByIdAsync(int Id);
        public void RemoveFSText(FSText fSText);
        #endregion FractionSlider
        #region Terms
        public void CreateTerm(Terms Terms);
        public void UpdateTerm(Terms Terms);
        public Task<List<Terms>> GetTermsAsync();
        public Task<Terms> GetTermsByIdAsync(int Id);
        public Task RemoveTermsByIdAsync(int Id);
        public bool ExistTerms(int Id);
        #endregion Terms
        #region Aboutfaq
        public void CreateAboutUsfaq(AboutUsfaq aboutUsfaq);
        public void UpateAboutUsfas(AboutUsfaq aboutUsfaq);
        public Task<AboutUsfaq> GetAboutUsfaqByIdAsync(int Id);
        public bool ExistAboutUsfaq(int id);
        public Task<List<AboutUsfaq>> GetAboutUsfaqsAsync();
        public void RemoveAboutUsfaq(AboutUsfaq aboutUsfaq);
        #endregion Aboutfaq
    }
}
