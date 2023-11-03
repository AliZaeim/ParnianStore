using Core.Convertors;
using Core.DTOs.Admin;
using Core.Prodocers;
using Core.Security;
using Core.Services.Interfaces;
using Core.Utility;
using DataLayer.Entities.Supplementary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Web.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    [PermissionCheckerByPermissionName("sliders")]
    public class SlidersController : Controller
    {
        private readonly ICompService _compservice;
        public SlidersController(ICompService compService)
        {
            _compservice = compService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _compservice.GetSlidersAsync());
        }
        public async Task<bool> ChangeStatus(int id, int status)
        {
            Slider slider =await _compservice.GetSliderById(id);
            bool st = false;
            if (status == 1)
            {
                st = true;
            }
            slider.SliderIsActive = st;
            _compservice.UpdateSlider(slider);
            await _compservice.SaveChangesAsync();
            return st;

        }
        // GET: Admin/Sliders/Details/5
        //[PermissionChecker(125)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _compservice.GetSliderById((int)id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(SliderVM sliderViewModel, IFormFile SliderImage)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderViewModel);
            }
            if (SliderImage == null)
            {
                ModelState.AddModelError("SliderImage", "لطفا برای اسلایدر تصویر انتخاب کنید !");
                return View(sliderViewModel);
            }

            string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
            if (SliderImage != null)
            {
                FileValidation fileValidation = await SliderImage.UploadedImageValidation(100, ex);
                if(!fileValidation.IsValid)
                {
                    ModelState.AddModelError("SliderImage", fileValidation.Message);
                    return View(sliderViewModel);
                }
            }
            Slider slider = new()
            {
                SliderTitle = sliderViewModel.SliderTitle,
                SliderTitleClass = sliderViewModel.SliderTitleClass,
                SliderTitle_DataAppear=sliderViewModel.SliderTitle_DataAppear,
                SliderImage = SliderImage.SaveUploadedImage("wwwroot/images/sliders", true),                
                SliderRegisterDateTime = DateTime.Now,
                SliderIsActive = sliderViewModel.SliderIsActive,
                SliderComment = sliderViewModel.SliderComment,
                SliderCommentClass = sliderViewModel.SliderCommentClass,
                SliderComment_DataAppear=sliderViewModel.SliderComment_DataAppear,
                SliderLink = sliderViewModel.SliderLink,
                SliderLinkText = sliderViewModel.SliderLinkText,
                SliderLinkClass = sliderViewModel.SliderLinkClass,
                SliderLink_DataAppear = sliderViewModel.SliderLink_DataAppear
            };
           
            if (!string.IsNullOrEmpty(sliderViewModel.SliderStartDate))
            {
                slider.SliderStartActiveDate = sliderViewModel.SliderStartDate.ToMiladiWithTime(sliderViewModel.SliderStartTime);
            }
            if (!string.IsNullOrEmpty(sliderViewModel.SliderEndDate))
            {
                slider.SliderEndActiveDate = sliderViewModel.SliderEndDate.ToMiladiWithTime(sliderViewModel.SliderEndTime);
            }

            _compservice.CreateSlider(slider);
            await _compservice.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _compservice.GetSliderById((int)id);
            if (slider == null)
            {
                return NotFound();
            }
            SliderVM sliderViewModel = new ()
            {
                SliderTitle = slider.SliderTitle,
                SliderTitleClass = slider.SliderTitleClass,
                SliderTitle_DataAppear = slider.SliderTitle_DataAppear,
                SliderComment = slider.SliderComment,
                SliderCommentClass = slider.SliderCommentClass,
                SliderComment_DataAppear = slider.SliderComment_DataAppear,
                SliderImage = slider.SliderImage,                
                SliderIsActive = slider.SliderIsActive,
                SliderLink = slider.SliderLink,
                SliderLinkText = slider.SliderLinkText,
                SliderLinkClass = slider.SliderLinkClass,
                SliderLink_DataAppear = slider.SliderLink_DataAppear,
                SliderId = slider.SliderId,
                SliderStartDate = slider.SliderStartActiveDate.ToShamsiN(),
                SliderEndDate = slider.SliderEndActiveDate.ToShamsiN()
            };
            if (slider.SliderStartActiveDate.HasValue)
            {
                int sh = slider.SliderStartActiveDate.Value.Hour;
                int sm = slider.SliderStartActiveDate.Value.Minute;
                sliderViewModel.SliderStartTime = sh.ToString("00") + ":" + sm.ToString("00");
            }
            if (slider.SliderEndActiveDate.HasValue)
            {
                int eh = slider.SliderEndActiveDate.Value.Hour;
                int em = slider.SliderEndActiveDate.Value.Minute;
                sliderViewModel.SliderEndTime = eh.ToString("00") + ":" + em.ToString("00");
            }

            return View(sliderViewModel);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionChecker(123)]
        public async Task<IActionResult> Edit(SliderVM sliderViewModel, IFormFile SliderImage)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderViewModel);
            }
            Slider slider = await _compservice.GetSliderById(sliderViewModel.SliderId);
            

            if (SliderImage != null)
            {
                string[] ex = { ".jpg", ".jpeg", ".gif", ".png", ".svg", ".webp", ".avif" };
                FileValidation fileValidation = await SliderImage.UploadedImageValidation(100, ex);
                if(!fileValidation.IsValid)
                {
                    ModelState.AddModelError("SliderImage", fileValidation.Message);
                    return View(sliderViewModel);
                }
                slider.SliderImage = SliderImage.SaveUploadedImage("wwwroot/images/sliders", true);
            }
            slider.SliderTitle = sliderViewModel.SliderTitle;
            slider.SliderTitleClass = sliderViewModel.SliderTitleClass;
            slider.SliderTitle_DataAppear = sliderViewModel.SliderTitle_DataAppear;
            slider.SliderComment = sliderViewModel.SliderComment;
            slider.SliderCommentClass = sliderViewModel.SliderCommentClass;
            slider.SliderComment_DataAppear = sliderViewModel.SliderComment_DataAppear;
            slider.SliderIsActive = sliderViewModel.SliderIsActive;
            slider.SliderLink = sliderViewModel.SliderLink;
            slider.SliderLinkText = sliderViewModel.SliderLinkText;
            slider.SliderLinkClass = sliderViewModel.SliderLinkClass;
            slider.SliderLink_DataAppear = sliderViewModel.SliderLink_DataAppear;
           
            if (!string.IsNullOrEmpty(sliderViewModel.SliderStartDate))
            {

                slider.SliderStartActiveDate = sliderViewModel.SliderStartDate.ToMiladiWithTime(sliderViewModel.SliderStartTime);
            }
            else
            {
                slider.SliderStartActiveDate = null;
            }
            if (!string.IsNullOrEmpty(sliderViewModel.SliderEndDate))
            {

                slider.SliderEndActiveDate = sliderViewModel.SliderEndDate.ToMiladiWithTime(sliderViewModel.SliderStartTime);
            }
            else
            {
                slider.SliderEndActiveDate = null;
            }
            _compservice.UpdateSlider(slider);
            await _compservice.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/Sliders/Delete/5
        // [PermissionChecker(124)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _compservice.GetSliderById((int)id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[PermissionChecker(124)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await _compservice.RemoveSlider(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
