using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers
{
    public class moneysController : Controller
    {
        string connectionString = "server=localhost;port=3306;database=money;uid=root;password=321753joe";
        private moneyEntities db = new moneyEntities();

        // GET: moneys
        public ActionResult Index()
        {
            return View(db.money.ToList());
        }

        // GET: moneys/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            money money = db.money.Find(id);
            if (money == null)
            {
                return HttpNotFound();
            }
            return View(money);
        }

        // GET: moneys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: moneys/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "date,thing,pay,earn,count,id")] money money)
        {
            if (ModelState.IsValid)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO money(date,thing,pay,earn) " +
                      "VALUES ('" + money.date + "','" + money.thing + "','" + money.pay + "','" + money.earn +
                        "'); ";
                    MySqlScript script = new MySqlScript(connection, sql);
                    script.Execute();
                    connection.Close();
                    return RedirectToAction("Index");
                }
            }

            return View(money);
        }

        // GET: moneys/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            money money = db.money.Find(id);
            if (money == null)
            {
                return HttpNotFound();
            }
            return View(money);
        }

        // POST: moneys/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "date,thing,pay,earn,count,id")] money money)
        {
            if (ModelState.IsValid)
            {
                db.Entry(money).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(money);
        }

        // GET: moneys/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            money money = db.money.Find(id);
            if (money == null)
            {
                return HttpNotFound();
            }
            return View(money);
        }

        // POST: moneys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            money money = db.money.Find(id);
            db.money.Remove(money);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
