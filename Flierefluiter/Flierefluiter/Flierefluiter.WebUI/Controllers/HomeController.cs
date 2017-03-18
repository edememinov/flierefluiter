using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flierefluiter.Domain.Abstract;
using Flierefluiter.Domain.Entities;
using Flierefluiter.Domain.Concrete;
using Flierefluiter.WebUI.Models;
using System.Data.Entity;

namespace Flierefluiter.WebUI.Controllers
{
    public class HomeController : Controller
    {
        
        private EFDbContext db = new EFDbContext();
        private IFlierefluiterRepository repository;
       

        public HomeController(IFlierefluiterRepository repository)
        {
            this.repository = repository;

        }


        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            ReservatieModel reservering = new ReservatieModel
            {
                Plaatsen = repository.Plaatss.ToList(),
                Velden = repository.Velds.ToList()

            };
            return View(reservering);
        }

        [HttpGet]
        public ActionResult ReserveringSucces()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ReservatieModel reservatieModel)
        {

            reservatieModel.Velden = repository.Velds.Where(p => p.VeldID.Equals(reservatieModel.Reservering.VeldID)).ToList();
            reservatieModel.Plaatsen = repository.Plaatss.Where(p => p.VeldID.Equals(reservatieModel.Reservering.VeldID)).ToList();
            Plaats GekozenPlaats = reservatieModel.Plaatsen.First();
            Veld GekozenVeld = reservatieModel.Velden.FirstOrDefault();
            reservatieModel.Reserveringen = repository.Reserverings.Where(p => p.BeginDatum >= reservatieModel.Reservering.BeginDatum && p.BeginDatum <= reservatieModel.Reservering.EindDatum || p.EindDatum <= reservatieModel.Reservering.BeginDatum && p.EindDatum >= reservatieModel.Reservering.EindDatum).ToList();





            Reservering Reservering = new Reservering {
                BeginDatum = reservatieModel.Reservering.BeginDatum,
                EindDatum = reservatieModel.Reservering.EindDatum,
                Email = reservatieModel.Reservering.Email,
                Naam = reservatieModel.Reservering.Naam,
                Telnr = reservatieModel.Reservering.Telnr,
                PlaatsId = GekozenPlaats.PlaatsID,
                VeldID = GekozenVeld.VeldID,
         
            };

            if(reservatieModel.Reserveringen != null)
            {
                foreach(Reservering reservatie in reservatieModel.Reserveringen)
                {
                    if (Reservering.PlaatsId.Equals(reservatie.PlaatsId))
                    {
                        var ControlePlaats = Reservering.PlaatsId + 1;

                        if (ControlePlaats > reservatieModel.Plaatsen.OrderByDescending(p => p.PlaatsID).First().PlaatsID)
                        {
                            //throw new Exception("Geem plek");
                            
                            ViewBag.Message = "Alle plekken zijn al bezet voor dit veld tijdens de door u gekozen eind- en begindatum";
                            return View();
                            
                        }

                        else
                        {
                            Reservering.PlaatsId++;

                            if (ModelState.IsValid)
                            {


                                if (reservatieModel.Reservering.EindDatum > reservatieModel.Reservering.BeginDatum)
                                {



                                    db.Reserverings.Add(Reservering);
                                    db.SaveChanges();
                                    return RedirectToAction("ReserveringSucces");
                                }
                            }

                            return View(reservatieModel);
                        }

                    }
                    }
                }

            return View(reservatieModel);

        }


            
            
    }
}