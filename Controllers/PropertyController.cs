using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.extModals;
using RealEstate.Models;
using PagedList;

namespace RealEstate.Controllers
{
    public class PropertyController : Controller
    {
        //
        // GET: /Property/
        realEstateEntities db = new realEstateEntities();

        public PropertyController()
        {
            var citiesb = db.cities;
            ViewBag.Cities = new SelectList(citiesb, "cityID", "cityName");
        }
        
        
        public ActionResult about()
        {
            var villa = db.properties.Where(me => me.pType == 1).Count();
            var student = db.properties.Where(me => me.pType == 2).Count();
            var family = db.properties.Where(me => me.pType == 3).Count();

            ViewBag.Villa = villa;
            ViewBag.Student = student;
            ViewBag.Family = family;

            return View();
        }
        public ActionResult contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult contact(FormCollection form)
        {
            return View();
        }
        public ActionResult privacy()
        {
            return View();
        }
        public ActionResult terms()
        {
            return View();
        }
        public ActionResult sitemap()
        {
            return View();
        }
        public ActionResult AllProp(int page = 1)
        {
            Int16 uID = Convert.ToInt16(Session["UserID"]);
            IEnumerable<newPropModal> cand = (from r in db.properties
                                              orderby r.PropID descending
                                              select new newPropModal
                                              {
                                                  ProID = r.PropID,
                                                  propName = r.pName,
                                                  Price = r.pPrice,
                                                  PropCity = r.city.cityName,
                                                  Address = r.pAddress,
                                                  Status = r.pActivationAdmin,
                                                  PropFor = r.propFor,
                                                  CreatedDate = r.pPublicationDate,

                                              }).Skip(0);
            int pageSize = 10;
            return View(cand.ToPagedList<newPropModal>(page, pageSize));
        }
        public ActionResult AllProperties(int page = 1)
        {
            Int16 uID = Convert.ToInt16(Session["UserID"]);
            IEnumerable<newPropModal> cand = (from r in db.properties
                                              where r.pAgentID_FK!=2
                                              orderby r.PropID descending                                              
                                              select new newPropModal
                                              {
                                                  ProID = r.PropID,
                                                  propName = r.pName,
                                                  Price = r.pPrice,
                                                  PropCity = r.city.cityName,
                                                  Address = r.pAddress,
                                                  Status = r.pActivationAdmin,
                                                  PropFor = r.propFor,
                                                  CreatedDate = r.pPublicationDate,

                                              }).Skip(0);
            int pageSize = 10;
            return View(cand.ToPagedList<newPropModal>(page, pageSize));
        }
        [HttpPost]
        public JsonResult ActivationAdmin(int id, string selctedvalue)
        {
            var activatin = db.properties.Where(a => a.PropID == id).SingleOrDefault();
            string message = "Added Successfully";
            bool activa;
            if (selctedvalue == "1")
            {
                activa = true;
            }
            else
            { activa = false; }
            activatin.pActivationAdmin = activa;
            db.SaveChanges();
            //return Json(new { message });
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult AllAgents(int page=1)
        {
            if (HttpContext.Session["UserID"] == "" || HttpContext.Session["UserID"] == null)
            {
                return RedirectToAction("login");
            }
            IEnumerable<showAgents> agents = (from r in db.users
                                              where r.userID != 2
                                              orderby r.userID descending
                                              select new showAgents
                                              {
                                                  agentID=r.userID,
                                                  UserName = r.userName,
                                                  UserEmail = r.uEmail,
                                                  city = r.city.cityName,
                                                  UserPhone = r.uPhone,
                                                  activation=r.uActivAdmin,
                                                  Dated = r.uDateCreated,
                                              }).Skip(0);
            int pageSize = 10;
            return View(agents.ToPagedList<showAgents>(page, pageSize));    
        }
        [HttpPost]
        public JsonResult SaveAgent(int id, string selctedvalue)
        {
            var activatin = db.users.Where(a => a.userID == id).SingleOrDefault();
            string message = "Added Successfully";
            bool activa;
            if (selctedvalue == "1")
            {
                activa = true;
            }
            else
            { activa = false; }
            activatin.uActivAdmin = activa;
            db.SaveChanges();
            //return Json(new { message });
            //var prop = db.properties.Where(a => a.pAgentID_FK == id);
            var df = from r in db.properties
                     where r.pAgentID_FK == id
                     select r;
            if (df != null)
            {
                foreach (var r in df)
                {
                    r.pActivationAdmin = activa;
                    db.SaveChanges();
                }
            }
            
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult Edit(FormCollection form, HttpPostedFileBase[] imagProp, editModel modal)
        {
            int id =Convert.ToInt16( form["propid"]);
            //int id = modal.ProID;
            if (ModelState.IsValid)
            {
                if (HttpContext.Session["UserID"] == "" || HttpContext.Session["UserID"] == null)
                {
                    return RedirectToAction("login");
                }
                property result = (from p in db.properties
                                 where p.PropID == id
                                 select p).SingleOrDefault();

                result.pName = modal.propName;
                result.pPrice = modal.Price;
                result.pType = modal.ProType;
                result.propFor = modal.PropFor;
                result.pFloors = modal.nooffloor;
                result.pNoofRooms = modal.noofrooms;
                result.pNoofBeds = modal.noofBed;
                result.pSizeSquare = modal.SizeSquare;
                result.pCityID_FK = modal.CityID;
                result.pAddress = modal.Address;
                result.pPostCode = Convert.ToString(modal.Postcode);
                result.pAvailableFrom =Convert.ToDateTime( modal.availablefrom);
                result.pDescription = modal.description;
                //db.Entry(result).State = System.Data.Entity.EntityState.Modified;     
                db.SaveChanges();

                propImage image = db.propImages.Create();
                foreach (HttpPostedFileBase file in imagProp)
                {
                    if (file != null)
                    {
                        if (imagProp.Count() >= 1)
                        {
                            System.IO.Stream fullstream;
                            fullstream = file.InputStream;
                            byte[] fullimage = dal.ResizeUplImage(fullstream);


                            System.IO.Stream MyStream;
                            MyStream = file.InputStream;
                            byte[] newimage = dal.ResizeUploadedImage(MyStream);
                            image.propImageThumb = newimage;


                            image.propID_FK = id;
                            image.propImage1 = fullimage;
                            image.propAgentID_FK = Convert.ToInt32(HttpContext.Session["UserID"]);
                            db.propImages.Add(image);
                            db.SaveChanges();
                        }
                    }
                }
                ViewBag.Message = "Property Updated successfully";
                ModelState.Clear();
            }
            var resl = from empl in db.properties.AsEnumerable() where empl.PropID == id select empl;
            ViewBag.Proper = resl;

            Int32? cityid = 1;
            Int32? floor = 1;
            Int32? beds = 1;
            Int32? roomm = 1;
            Int32? ptype = 1;
            string pfor = "";
            foreach (var t in resl)
            {
                cityid = t.pCityID_FK;
                floor = t.pFloors;
                beds = t.pNoofBeds;
                ptype = t.pType;
                pfor = t.propFor;
            }

            List<SelectListItem> pType = new List<SelectListItem>();
            pType.Add(new SelectListItem() { Text = "Villa", Value = "1" });
            pType.Add(new SelectListItem() { Text = "Student Apartment", Value = "2" });
            pType.Add(new SelectListItem() { Text = "Family Apartment", Value = "3" });
            this.ViewBag.pType = new SelectList(pType, "Value", "Text", ptype);

            List<SelectListItem> pFor = new List<SelectListItem>();
            pFor.Add(new SelectListItem() { Text = "Rent", Value = "Rent" });
            pFor.Add(new SelectListItem() { Text = "Sale", Value = "Sale" });
            this.ViewBag.pFor = new SelectList(pFor, "Value", "Text", pfor);

            List<SelectListItem> bedd = new List<SelectListItem>();
            bedd.Add(new SelectListItem() { Text = "1", Value = "1" });
            bedd.Add(new SelectListItem() { Text = "2", Value = "2" });
            bedd.Add(new SelectListItem() { Text = "3", Value = "3" });
            bedd.Add(new SelectListItem() { Text = "4", Value = "4" });
            bedd.Add(new SelectListItem() { Text = "5", Value = "5" });
            bedd.Add(new SelectListItem() { Text = "6", Value = "6" });
            bedd.Add(new SelectListItem() { Text = "7+", Value = "7" });
            this.ViewBag.beds = new SelectList(bedd, "Value", "Text", beds);

            List<SelectListItem> rooms = new List<SelectListItem>();
            rooms.Add(new SelectListItem() { Text = "1", Value = "1" });
            rooms.Add(new SelectListItem() { Text = "2", Value = "2" });
            rooms.Add(new SelectListItem() { Text = "3", Value = "3" });
            rooms.Add(new SelectListItem() { Text = "4", Value = "4" });
            rooms.Add(new SelectListItem() { Text = "5", Value = "5" });
            rooms.Add(new SelectListItem() { Text = "6", Value = "6" });
            rooms.Add(new SelectListItem() { Text = "7+", Value = "7" });
            this.ViewBag.rooms = new SelectList(rooms, "Value", "Text", roomm);

            List<SelectListItem> floors = new List<SelectListItem>();
            floors.Add(new SelectListItem() { Text = "1", Value = "1" });
            floors.Add(new SelectListItem() { Text = "2", Value = "2" });
            floors.Add(new SelectListItem() { Text = "3", Value = "3" });
            floors.Add(new SelectListItem() { Text = "4", Value = "4" });
            floors.Add(new SelectListItem() { Text = "5", Value = "5" });
            floors.Add(new SelectListItem() { Text = "6", Value = "6" });
            floors.Add(new SelectListItem() { Text = "7+", Value = "7" });
            this.ViewBag.floors = new SelectList(floors, "Value", "Text", floor);

            var citiesb = db.cities;
            ViewBag.Cities = new SelectList(citiesb, "cityID", "cityName");

            return View(modal);               
        }
        public ActionResult Edit(int id)
        {
            string userid=Convert.ToString( Session["UserID"]);
            if (string.IsNullOrEmpty(userid))
            {
                return RedirectToAction("login");
            }
            //var ab = db.properties.Where(x => x.PropID == id).FirstOrDefault();
            //IEnumerable<newPropModal> proper = (from r in db.properties
            //                                    where r.PropID == id
            //                                    select new newPropModal
            //                                    {
            //                                        ProID = id,
            //                                        propName = r.pName,
            //                                        PropFor = r.propFor,
            //                                        ProType = r.pType,
            //                                        Price = r.pPrice,
            //                                        nooffloor = r.pFloors,
            //                                        noofBed = r.pNoofBeds,
            //                                        SizeSquare = r.pSizeSquare,
            //                                        CityID = r.pCityID_FK,
            //                                        Address = r.pAddress,
            //                                        description = r.pDescription,
            //                                    });
            var resl = from empl in db.properties.AsEnumerable() where empl.PropID==id select empl;
            ViewBag.Proper = resl;
            //DataTable dt_leaddata = new DataTable();
            //dt_leaddata = LeadClass.GetleadDataByGuid(id);
            //var list = dt_leaddata.AsEnumerable().ToList();
            //ViewBag.leaddata = list[0];
            //var query = from empl in db.properties.AsEnumerable() select empl;
            //List<property> dt = query.ToList();
            
            Int32? cityid=1;
            Int32? floor = 1;
            Int32? beds = 1;
            Int32? roomm = 1;
            Int32? ptype = 1;
            string pfor = "";           
            foreach (var t in resl)
            {
                cityid = t.pCityID_FK;
                floor = t.pFloors;
                beds = t.pNoofBeds;
                ptype = t.pType;
                pfor = t.propFor;             
            }           

            List<SelectListItem> pType = new List<SelectListItem>();
            pType.Add(new SelectListItem() { Text = "Villa", Value = "1" });
            pType.Add(new SelectListItem() { Text = "Student Apartment", Value = "2" });
            pType.Add(new SelectListItem() { Text = "Family Apartment", Value = "3" });
            this.ViewBag.pType = new SelectList(pType, "Value", "Text", ptype);

            List<SelectListItem> pFor = new List<SelectListItem>();
            pFor.Add(new SelectListItem() { Text = "Rent", Value = "Rent" });
            pFor.Add(new SelectListItem() { Text = "Sale", Value = "Sale" });
            this.ViewBag.pFor = new SelectList(pFor, "Value", "Text", pfor);

            List<SelectListItem> bedd = new List<SelectListItem>();
            bedd.Add(new SelectListItem() { Text = "1", Value = "1" });
            bedd.Add(new SelectListItem() { Text = "2", Value = "2" });
            bedd.Add(new SelectListItem() { Text = "3", Value = "3" });
            bedd.Add(new SelectListItem() { Text = "4", Value = "4" });
            bedd.Add(new SelectListItem() { Text = "5", Value = "5" });
            bedd.Add(new SelectListItem() { Text = "6", Value = "6" });
            bedd.Add(new SelectListItem() { Text = "7+", Value = "7" });
            this.ViewBag.beds = new SelectList(bedd, "Value", "Text", beds);

            List<SelectListItem> rooms = new List<SelectListItem>();
            rooms.Add(new SelectListItem() { Text = "1", Value = "1" });
            rooms.Add(new SelectListItem() { Text = "2", Value = "2" });
            rooms.Add(new SelectListItem() { Text = "3", Value = "3" });
            rooms.Add(new SelectListItem() { Text = "4", Value = "4" });
            rooms.Add(new SelectListItem() { Text = "5", Value = "5" });
            rooms.Add(new SelectListItem() { Text = "6", Value = "6" });
            rooms.Add(new SelectListItem() { Text = "7+", Value = "7" });
            this.ViewBag.rooms = new SelectList(rooms, "Value", "Text", roomm);

            List<SelectListItem> floors = new List<SelectListItem>();
            floors.Add(new SelectListItem() { Text = "1", Value = "1" });
            floors.Add(new SelectListItem() { Text = "2", Value = "2" });
            floors.Add(new SelectListItem() { Text = "3", Value = "3" });
            floors.Add(new SelectListItem() { Text = "4", Value = "4" });
            floors.Add(new SelectListItem() { Text = "5", Value = "5" });
            floors.Add(new SelectListItem() { Text = "6", Value = "6" });
            floors.Add(new SelectListItem() { Text = "7+", Value = "7" });
            this.ViewBag.floors = new SelectList(floors, "Value", "Text", floor);
            var citiesb = db.cities;
            ViewBag.Cities = new SelectList(citiesb, "cityID", "cityName",cityid);
            editModel mod = new editModel();
            //ViewBag.Proper = dt;
            return View(mod);
        }
        public ActionResult Index()
        {
            IEnumerable<newPropModal> Prop;
            List<newPropModal> Prop1 = new List<newPropModal>();
            Prop = (from rent in db.properties
                    where rent.pActivationAdmin==true&&rent.pActivationStatus==true
                    orderby (rent.PropID) descending
                    select new newPropModal
                    {
                        ProID = rent.PropID,
                        propName = rent.pName,
                        Price = rent.pPrice,
                        PropCity = rent.city.cityName,
                        Address = rent.pAddress,
                        Status = rent.pActivationStatus,
                        PropFor = rent.propFor
                    }).Take(6);
            foreach (newPropModal data in Prop)
            {
                int bid = data.ProID;
                propImage image = db.propImages.FirstOrDefault(u => u.propID_FK == bid);
                data.imageProper = image.propImage1;
                Prop1.Add(data);
            }
            ViewBag.Data = Prop1;
            return View();
        }
        public ActionResult Family(int page = 1,int id=0)
        {           
            string reqType="";
            if (id == 1)
            {
                reqType = "Rent";
            }
            else
            {
                reqType = "Sale";
            }
            IEnumerable<viewModel> Prop;
            List<viewModel> Prop1 = new List<viewModel>();
            Prop = (from rent in db.properties
                    orderby rent.PropID descending
                    join im in db.propImages on rent.PropID equals im.propID_FK into g
                    where rent.propFor == reqType && rent.pType == 3 && rent.pActivationAdmin == true && rent.pActivationStatus == true
                    select new viewModel
                    {
                        ProID = rent.PropID,
                        propName = rent.pName,
                        noofBed = rent.pNoofBeds,
                        ProType = rent.pType,
                        PropFor = rent.propFor,
                        nooffloor = rent.pFloors,
                        sizeSquare = rent.pSizeSquare,
                        Price = rent.pPrice,
                        totalImages = g.Count()
                    }).Skip(0);
            foreach (viewModel data in Prop)
            {
                int bid = data.ProID;
                propImage image = db.propImages.FirstOrDefault(u => u.propID_FK == bid);
                data.imageProper = image.propImage1;
                Prop1.Add(data);
            }
            int pageSize = 10;
            return View(Prop1.ToPagedList<viewModel>(page, pageSize));
        }
        public ActionResult Student(int page = 1, int id = 0)
        {
            string reqType = "";
            if (id == 1)
            {
                reqType = "Rent";
            }
            else
            {
                reqType = "Sale";
            }
            IEnumerable<viewModel> Prop;
            List<viewModel> Prop1 = new List<viewModel>();
            Prop = (from rent in db.properties
                    orderby rent.PropID descending
                    join im in db.propImages on rent.PropID equals im.propID_FK into g
                    where rent.propFor == reqType && rent.pType == 2 && rent.pActivationAdmin == true && rent.pActivationStatus == true
                    select new viewModel
                    {
                        ProID = rent.PropID,
                        propName = rent.pName,
                        noofBed = rent.pNoofBeds,
                        ProType = rent.pType,
                        PropFor = rent.propFor,
                        nooffloor = rent.pFloors,
                        sizeSquare = rent.pSizeSquare,
                        Price = rent.pPrice,
                        totalImages=g.Count()
                    }).Skip(0);
            foreach (viewModel data in Prop)
            {
                int bid = data.ProID;
                propImage image = db.propImages.FirstOrDefault(u => u.propID_FK == bid);
                data.imageProper = image.propImage1;
                Prop1.Add(data);
            }
            int pageSize = 10;
            return View(Prop1.ToPagedList<viewModel>(page, pageSize));
        }
        public ActionResult Villa(int page = 1, int id = 0)
        {
            string reqType = "";
            if (id == 1)
            {
                reqType = "Rent";
            }
            else
            {
                reqType = "Sale";
            }
            IEnumerable<viewModel> Prop;
            List<viewModel> Prop1 = new List<viewModel>();
            Prop = (from rent in db.properties
                    orderby rent.PropID descending
                    join im in db.propImages on rent.PropID equals im.propID_FK into g
                    where rent.propFor == reqType && rent.pType == 1 && rent.pActivationAdmin == true && rent.pActivationStatus == true
                    select new viewModel
                    {
                        ProID = rent.PropID,
                        propName = rent.pName,
                        noofBed = rent.pNoofBeds,
                        ProType = rent.pType,
                        PropFor = rent.propFor,
                        nooffloor = rent.pFloors,
                        sizeSquare = rent.pSizeSquare,                   
                        Price = rent.pPrice,
                        totalImages = g.Count()
                    }).Skip(0);
            foreach (viewModel data in Prop)
            {
                int bid = data.ProID;
                propImage image = db.propImages.FirstOrDefault(u => u.propID_FK == bid);
                data.imageProper = image.propImage1;
                Prop1.Add(data);
            }
            int pageSize = 10;
            return View(Prop1.ToPagedList<viewModel>(page, pageSize));            
        }
        public ActionResult Results(int page=1)
        {
            string type = Request.Form["sType"];
            Int32 propTypee = Convert.ToInt32(Request.Form["propType"]);
            int MinPrice =Convert.ToInt16( Request.Form["minPrice"]);
            int MaxPrice =Convert.ToInt16( Request.Form["maxPrice"]);
            int citi =Convert.ToInt16( Request.Form["city"]);
            if (MinPrice == 0 && MaxPrice == 0)
            {
                MinPrice = 1000;
                MaxPrice = 10000;
            }
            else if (MinPrice == 0 && MaxPrice > 0)
            {
                MinPrice = 1000;
            }
            else if (MaxPrice == 0 && MinPrice > 0)
            {
                MaxPrice = 10000;
            }
            //IEnumerable<newPropModal> cand;
            //if (MinPrice =="0" || MaxPrice == "0")
            //{

            //}
            //else
            //{
            //                              cand = (from r in db.properties
            //                                      orderby r.PropID descending
            //                                      where r.propFor == type && r.pType == propTypee
            //                                      select new newPropModal
            //                                      {
            //                                          ProID = r.PropID,
            //                                          propName = r.pName,
            //                                          Price = r.pPrice,
            //                                          PropCity = r.city.cityName,
            //                                          Address = r.pAddress,
            //                                          Status = r.pActivationStatus,
            //                                          PropFor = r.propFor,
            //                                          CreatedDate = r.pPublicationDate,
            //                                      }).Skip(0);                
            //}
            IEnumerable<viewModel> Prop;
             List<viewModel> Prop1 = new List<viewModel>();         
                Prop = (from rent in db.properties
                        orderby rent.PropID descending
                        where rent.propFor == type && rent.pType == propTypee && rent.pCityID_FK == citi && (rent.pPrice >= MinPrice && rent.pPrice <= MaxPrice) && rent.pActivationAdmin == true && rent.pActivationStatus == true                        
                        select new viewModel
                        {
                            ProID = rent.PropID,
                            propName = rent.pName,
                            noofBed = rent.pNoofBeds,
                            ProType = rent.pType,
                            PropFor = rent.propFor,
                            nooffloor = rent.pFloors,
                            sizeSquare = rent.pSizeSquare,
                            //Address = rent.pAddress,
                            //Postcode = rent.pPostCode,
                            //PropCity = rent.city.cityName,
                            //description = rent.pDescription,
                            //agentPhone = t.user.uPhone,
                            //availablefrom = rent.pAvailableFrom,
                            //CreatedDate = rent.pPublicationDate,
                            Price = rent.pPrice,
                        }).Skip(0);
                foreach (viewModel data in Prop)
                {
                    int bid = data.ProID;
                    propImage image = db.propImages.FirstOrDefault(u => u.propID_FK == bid);
                    data.imageProper = image.propImage1;
                    Prop1.Add(data);
                }
            int pageSize = 10;
            return View(Prop1.ToPagedList<viewModel>(page, pageSize));

            //return View();
        }
        public ActionResult Detail(int? id = 0)
        {
            IEnumerable<viewModel> result = (from t in db.properties
                                             where t.PropID == id
                                             select new viewModel
                                             {
                                                 ProID = t.PropID,
                                                 propName = t.pName,
                                                 noofBed = t.pNoofBeds,
                                                 ProType = t.pType,
                                                 PropFor = t.propFor,
                                                 nooffloor = t.pFloors,
                                                 noofRooms=t.pNoofRooms,
                                                 sizeSquare = t.pSizeSquare,
                                                 Address = t.pAddress,
                                                 Postcode = t.pPostCode,
                                                 PropCity = t.city.cityName,
                                                 description = t.pDescription,
                                                 agentPhone = t.user.uPhone,
                                                 agentCity=t.user.city.cityName,
                                                 agentEmail=t.user.uEmail,
                                                 agentName=t.user.userName,
                                                 availablefrom = t.pAvailableFrom,
                                                 CreatedDate = t.pPublicationDate,
                                                 Price = t.pPrice,
                                             });
            ViewBag.PropDetail = result;
            IEnumerable<newPropModal> cand;
            cand = from r in db.propImages
                   where r.propID_FK == id
                   select new newPropModal
                   {
                       imageId = r.imageID,
                       imageProper = r.propImage1,
                       imageThum = r.propImageThumb,
                   };
            //Session["url"] = Request.Url.AbsoluteUri;
            return View(cand.ToList());
        }
        public ActionResult PropertyDetail(int? id = 0)
        {
            if (HttpContext.Session["UserID"] == "" || HttpContext.Session["UserID"] == null)
            {
                return RedirectToAction("login");
            }
            IEnumerable<viewModel> result = (from t in db.properties
                                                where t.PropID == id
                                                select new viewModel
                                                {
                                                    ProID = t.PropID,
                                                    propName=t.pName,
                                                    noofBed = t.pNoofBeds,
                                                    ProType = t.pType,
                                                    PropFor=t.propFor,
                                                    nooffloor=t.pFloors,
                                                    sizeSquare=t.pSizeSquare,
                                                    Address = t.pAddress,
                                                    Postcode = t.pPostCode,
                                                    PropCity = t.city.cityName,
                                                    description = t.pDescription,
                                                    //agentPhone = t.user.uPhone,
                                                    availablefrom = t.pAvailableFrom,
                                                    CreatedDate = t.pPublicationDate,
                                                    Price = t.pPrice,
                                                });
            ViewBag.PropDetail = result;
            IEnumerable<newPropModal> cand;
            cand = from r in db.propImages
                   where r.propID_FK == id
                   select new newPropModal
                   {
                       imageId = r.imageID,
                       imageProper = r.propImage1,
                       imageThum = r.propImageThumb,
                   };
            //Session["url"] = Request.Url.AbsoluteUri;
            return View(cand.ToList());
            //return View();
        }
        public ActionResult All(int page=1)
        {
            if (HttpContext.Session["UserID"] == "" || HttpContext.Session["UserID"] == null)
            {
                return RedirectToAction("login");
            }
            Int16 uID =Convert.ToInt16( Session["UserID"]);
             IEnumerable<newPropModal> cand = (from r in db.properties
                    orderby r.PropID descending
                    where r.pAgentID_FK==uID 
                    select new newPropModal
                    {
                       ProID=r.PropID,
                        propName = r.pName,
                        Price = r.pPrice,
                        PropCity = r.city.cityName,
                        Address = r.pAddress,
                        Status = r.pActivationStatus,                        
                        PropFor = r.propFor,
                        CreatedDate = r.pPublicationDate,

                    }).Skip(0);
            int pageSize = 10;
            return View(cand.ToPagedList<newPropModal>(page, pageSize));
        }        
        [HttpPost]
        public JsonResult SaveActivation(int id, string selctedvalue)
        {           
            var activatin = db.properties.Where(a => a.PropID == id).SingleOrDefault();
            string message = "Added Successfully";
            bool activa;
            if (selctedvalue == "1")
            {
                activa = true;
            }
            else
            { activa = false; }
            activatin.pActivationStatus = activa;
            db.SaveChanges();
            //return Json(new { message });
            return new JsonResult { Data=message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult New()
        {
            var citiesb = db.cities;
            ViewBag.Cities = new SelectList(citiesb, "cityID", "cityName");
            return View();
        }
        [HttpPost]
        public ActionResult New(FormCollection form, HttpPostedFileBase[] imagesProp, newPropModal modal)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session["UserID"] == "" || HttpContext.Session["UserID"] == null)
                {
                    return RedirectToAction("login");
                }
                
                property rent = db.properties.Create();

                rent.pName = modal.propName;
                rent.pPrice = modal.Price;
                rent.pType =Convert.ToInt16( form["propType"]);
                rent.propFor =Convert.ToString( form["propFor"]);
                rent.pFloors =Convert.ToInt16( form["noofFloors"]);
                rent.pNoofRooms = Convert.ToInt16(form["noofRooms"]);
                rent.pNoofBeds = Convert.ToInt32(form["noofBeds"]);
                rent.pSizeSquare = modal.SizeSquare;
                rent.pCityID_FK = modal.cities.cityID;
                rent.pAddress = modal.Address;
                rent.pPostCode =Convert.ToString( modal.Postcode);
                rent.pAvailableFrom =Convert.ToDateTime( modal.availablefrom);
                rent.pDescription = modal.description;
                rent.pAgentID_FK = Convert.ToInt32(HttpContext.Session["UserID"]);
                rent.pPublicationDate = DateTime.Now;
                rent.pActivationStatus = true;
                rent.pActivationAdmin = true;
                //var coverlet = "A";             
                db.properties.Add(rent);
                db.SaveChanges();
                Int32 PropertyID = rent.PropID;
                propImage image = db.propImages.Create();
                foreach (HttpPostedFileBase file in imagesProp)
                {
                    if (file != null)
                    {
                        if (imagesProp.Count() >= 1)
                        {
                            System.IO.Stream fullstream;
                            fullstream = file.InputStream;
                            byte[] fullimage =dal.ResizeUplImage(fullstream);


                            System.IO.Stream MyStream;
                            MyStream = file.InputStream;
                            byte[] newimage = dal.ResizeUploadedImage(MyStream);
                            image.propImageThumb = newimage;


                            image.propID_FK = PropertyID;
                            image.propImage1 = fullimage;
                            image.propAgentID_FK = Convert.ToInt32(HttpContext.Session["UserID"]);
                            db.propImages.Add(image);
                            db.SaveChanges();
                        }
                    }
                }
                ViewBag.Message = "Property Added successfully";
                ModelState.Clear();
            }
            var citiesb = db.cities;
            ViewBag.Cities = new SelectList(citiesb, "cityID", "cityName");
            return View();
        }
        public ActionResult Agents(int page=1)
        {           
            IEnumerable<showAgents> agents = (from r in db.users
                                                where r.userID!=2
                                                 orderby r.userID descending
                                                 select new showAgents
                                                 {
                                                     UserName = r.userName,
                                                     UserEmail = r.uEmail,
                                                     city = r.city.cityName,
                                                     UserPhone = r.uPhone,
                                                     Dated = r.uDateCreated,
                                                 }).Skip(0);
            int pageSize = 10;
            return View(agents.ToPagedList<showAgents>(page, pageSize));           
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(loginModel modal)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<user> ab = (from r in db.users
                                                     where r.uEmail == modal.UserEmail && r.uPassword == modal.Password && r.uActivAdmin==true&&r.uActivation==true
                                                     select r).ToList();
                string userNam="";
                if (ab.Count() > 0)
                {
                    foreach (var item in ab)
                    {
                       int userid = item.userID;
                       userNam = item.userName;
                       HttpContext.Session["UserID"] = userid;
                       HttpContext.Session["userNam"] = userNam;                       
                        //string base64 = Convert.ToBase64String(item.AgentLogo);
                        //Session["image"] = base64;                     
                    }
                    if (Convert.ToInt16( Session["UserID"])==2)                    
                    return RedirectToAction("Dashboard");
                    return RedirectToAction("New");
                    //System.Web.Security.FormsAuthentication.SetAuthCookie(userNam, true);
                    //User.Identity.Name
                }
                else
                {
                    ViewBag.Error = "Invalid Credentials or Your Account is suspended by Admin";
                }
            }
            return View();
        }
        public ActionResult register()
        {
            var citiesb = db.cities;
            ViewBag.Cities = new SelectList(citiesb, "cityID", "cityName");

            return View();
        }
        [HttpPost]
        public ActionResult register(registerModal modal)
        {
            if (ModelState.IsValid)
            {
                var ab = db.users.Where(x => x.uEmail == modal.UserEmail).FirstOrDefault();
                if (ab == null)
                {
                    var request = HttpContext.Request;
                    var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                    var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
                    Guid guidid = Guid.NewGuid();
                    user regis = db.users.Create();
                    regis.userName = modal.UserName;
                    regis.uPhone = modal.UserPhone;
                    regis.uEmail = modal.UserEmail;
                    regis.uPassword = modal.Password;
                    regis.uCityID_FK = modal.cities.cityID;
                    regis.uActivAdmin = false;
                    regis.uActivation = false;
                    regis.uGuid = guidid;
                    regis.uDateCreated = DateTime.Now;
                    db.users.Add(regis);
                    db.SaveChanges();
                                       
                    if(sendEmailModal.sendEmail(modal.UserEmail,baseUrl,guidid))
                    ViewBag.success = "Confirmation Link successfully sent to your email. Check your email to verify your Account";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.emailError = "Email Already Exist";
                }
            }
            var citiesb = db.cities;
            ViewBag.Cities = new SelectList(citiesb, "cityID", "cityName");
            return View();
        }
        [AllowAnonymous]
        public ActionResult Activation(Guid id)
        {          
            user result = db.users.Where(x => x.uGuid == id && x.uActivation == false).FirstOrDefault();
            if (result!=null)
            {
                result.uActivation = true;
                result.uActivAdmin = true;
                db.SaveChanges();
                ViewBag.success = "You are successfully registered Now Login to Upload Properties etc.";
                //return RedirectToAction("CreateUser", "Account", new { id = g });
            }
            else
            {
                ViewBag.error = "Account is Already Activated or Blocked by Admin";             
            }
            return View();
        }
        public ActionResult Dashboard()
        {
            string userid=Convert.ToString( Session["UserID"]);
            if (string.IsNullOrEmpty(userid))
            {
                return RedirectToAction("login");
            }
            return View();
        }
        public ActionResult logout()
        {
            Session.Abandon();
            
            return RedirectToAction("Index");
            //return View();
        }
	}
}