using Core.Services.Interfaces;
using DataLayer.Context;
using DataLayer.Entities.Blogs;
using DataLayer.Entities.Supplementary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CompService:ICompService
    {
        private readonly MyContext _Context;
        public CompService(MyContext Context)
        {
            _Context = Context;
        }
        #region Generic
        public async Task<InitialInfo> GetInitialInfoAsync()
        {
            return await _Context.InitialInfos.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        }
        public async Task<SitePage> GetSitePageByEnNameAsync(string EnName)
        {
            return await _Context.SitePages.SingleOrDefaultAsync(x => x.EnName.Equals(EnName));
        }
        public void SaveChanges()
        {
            _Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
        public async Task<Blog> GetBlogByIdAsync(string GId)
        {
            return await _Context.Blogs.SingleOrDefaultAsync(x => x.BlogId.ToString() == GId);
        }
        #endregion Generic
        #region Slider
        public void CreateSlider(Slider slider)
        {
             _Context.Sliders.Add(slider);           
        }

        public async Task<List<Slider>> GetSlidersAsync()
        {
            return await _Context.Sliders.ToListAsync();
        }

        public void UpdateSlider(Slider slider)
        {
            _Context.Sliders.Update(slider);
        }

        public async Task RemoveSlider(int id)
        {
            Slider slider = await _Context.Sliders.FindAsync(id);
            _Context.Sliders.Remove(slider);
        }

        public async Task<Slider> GetSliderById(int id)
        {
            return await _Context.Sliders.FindAsync(id);
        }

        #endregion Slider
        #region Keys
        public async Task<List<UniqueKey>> GetUniqueKeysAsync()
        {
            return await _Context.UniqueKeys.ToListAsync();
        }

        public List<string> AllKeys()
        {
            return _Context.UniqueKeys.Select(s => s.Key).ToList();
        }
        #endregion Keys
        public void CreateUniqueKey(UniqueKey uniqueKey)
        {
            _Context.UniqueKeys.Add(uniqueKey);
        }

        #region State
        public async Task<List<State>> GetStatesAsync()
        {
            return await _Context.States.Include(r => r.Counties).ToListAsync();
        }

        public async Task<State> GetStateByIdAsync(int Id)
        {
            return await _Context.States.Include(r => r.Counties).SingleOrDefaultAsync(x => x.StateId == Id);
        }
        #endregion State

        #region County
        public async Task<County> GetCountyByIdAsync(int countyId)
        {
            return await _Context.Counties.Include(r => r.State).SingleOrDefaultAsync(x => x.CountyId == countyId);
        }

        public async Task<List<County>> GetCountiesAsync()
        {
            return await _Context.Counties.Include(r => r.State).ToListAsync();
        }

        public async Task<List<County>> GetCountiesOfStateAsync(int stateId)
        {
            return await _Context.Counties.Include(r => r.State).Where(w => w.StateId == stateId).ToListAsync();
        }




        #endregion County
        #region Banner
        public async Task<BannerNextSlider> GetBannerNextSliderById(int Id)
        {
            return await _Context.BannerNextSliders.FindAsync(Id);
        }

        public void EditBanner(BannerNextSlider bannerNextSlider)
        {
            _Context.BannerNextSliders.Update(bannerNextSlider);
        }

        public void CreateBannerNextSlider(BannerNextSlider bannerNextSlider)
        {
            _Context.BannerNextSliders.Add(bannerNextSlider);
        }

        public async Task<List<BannerNextSlider>> GetBannerNextSlidersAsync()
        {
            return await _Context.BannerNextSliders.ToListAsync();
        }


        #endregion Banner
        #region FractionSlider
        public void CreateFractionSlider(FractionSlider fractionSlider)
        {
            _Context.FractionSliders.Add(fractionSlider);
        }

        public void UpdateFractionSlider(FractionSlider fractionSlider)
        {
            _Context.FractionSliders.Update(fractionSlider);
        }

        public async Task<List<FractionSlider>> GetFractionSlidersAsync()
        {
            return await _Context.FractionSliders.Include(x => x.FSImages).Include(x => x.FSTexts).ToListAsync();
        }

        public async Task<FractionSlider> GetFractionByIdAsync(int Id)
        {
            return await _Context.FractionSliders.Include(x => x.FSImages).Include(x => x.FSTexts)
                .SingleOrDefaultAsync(x => x.Id == Id);
        }
        public void RemoveFractionSlider(FractionSlider fractionSlider)
        {
            _Context.FractionSliders.Remove(fractionSlider);
        }
        public void CreateFSImage(FSImage fSImage)
        {
            _Context.FSImages.Add(fSImage);
        }

        public void UpdateFSImage(FSImage fSImage)
        {
            _Context.FSImages.Update(fSImage);
        }

        public async Task<List<FSImage>> GetFSImagesAsync()
        {
            return await _Context.FSImages.Include(x => x.FractionSlider).Include(x => x.FractionSlider.FSTexts).ToListAsync();
        }

        public async Task<FSImage> GetFSImageByIdAsync(int Id)
        {
            return await _Context.FSImages.Include(x => x.FractionSlider).Include(x => x.FractionSlider.FSTexts).SingleOrDefaultAsync(x => x.Id == Id);
        }

        public void RemoveFSImage(FSImage fSImage)
        {
            _Context.FSImages.Remove(fSImage);
        }

        public void CreateFSText(FSText fSText)
        {
            _Context.FSTexts.Add(fSText);
        }

        public void UpdateFSText(FSText fSText)
        {
            _Context.FSTexts.Update(fSText);
        }

        public async Task<List<FSText>> GetFSTextsAsync()
        {
            return await _Context.FSTexts.Include(x => x.FractionSlider).ToListAsync();
        }

        public async Task<FSText> GetFSTextByIdAsync(int Id)
        {
            return await _Context.FSTexts.Include(x => x.FractionSlider).SingleOrDefaultAsync(x => x.Id == Id);
        }

        public void RemoveFSText(FSText fSText)
        {
            _Context.FSTexts.Remove(fSText);
        }
        #endregion FractionSlider
        #region Terms
        public void CreateTerm(Terms Terms)
        {
            _Context.Terms.Add(Terms);
        }

        public void UpdateTerm(Terms Terms)
        {
            _Context.Terms.Update(Terms);
        }

        public async Task<List<Terms>> GetTermsAsync()
        {
            return await _Context.Terms.ToListAsync();
        }

        public async Task<Terms> GetTermsByIdAsync(int Id)
        {
            return await _Context.Terms.SingleOrDefaultAsync(x => x.Id == Id);
        }

        public async Task RemoveTermsByIdAsync(int Id)
        {
            Terms terms = await _Context.Terms.FindAsync(Id);
        }

        public bool ExistTerms(int Id)
        {
            return _Context.Terms.Any(x => x.Id == Id);
        }



        #endregion Terms
        #region Aboutfaq
        public void CreateAboutUsfaq(AboutUsfaq aboutUsfaq)
        {
            _Context.AboutUsfaqs.Add(aboutUsfaq);
        }

        public void UpateAboutUsfas(AboutUsfaq aboutUsfaq)
        {
            _Context.AboutUsfaqs.Update(aboutUsfaq);
        }

        public async Task<AboutUsfaq> GetAboutUsfaqByIdAsync(int Id)
        {
            return await _Context.AboutUsfaqs.FindAsync(Id);
        }

        public bool ExistAboutUsfaq(int id)
        {
            return _Context.AboutUsfaqs.Any(x => x.Id == id);
        }

        public async Task<List<AboutUsfaq>> GetAboutUsfaqsAsync()
        {
            return await _Context.AboutUsfaqs.ToListAsync();
        }

        public void RemoveAboutUsfaq(AboutUsfaq aboutUsfaq)
        {
            _Context.AboutUsfaqs.Remove(aboutUsfaq);
        }
        #endregion Aboutfaq
    }
}
