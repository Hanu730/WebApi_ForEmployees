using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApi_ForEmployees.Controllers
{
    public class EmployeeDetails1Controller : Controller
    {
        private EMPLOYEEEntities db = new EMPLOYEEEntities();


        public static DataTable GetDataTableFromObjects(object[] objects)
        {

            if (objects != null && objects.Length > 0)
            {
                Type t = objects[0].GetType();

                DataTable dt = new DataTable(t.Name);

                foreach (PropertyInfo pi in t.GetProperties())
                {

                    dt.Columns.Add(new DataColumn(pi.Name));

                }

                foreach (var o in objects)
                {
                    DataRow dr = dt.NewRow();

                    foreach (DataColumn dc in dt.Columns)
                    {

                        dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);

                    }

                    dt.Rows.Add(dr);

                }

                return dt;

            }

            return null;

        }
        public ActionResult Index()
        {
            HttpClient client = new HttpClient();
            string baseUrl = ConfigurationManager.AppSettings["AppKey"].ToString();
            client.BaseAddress = new Uri("http://localhost:58336/");
          Task<HttpResponseMessage> message=  client.GetAsync("api/EmployeeDetails");
            HttpResponseMessage httpResponse = message.Result;

          List<EmployeeDetail> details = httpResponse.Content.ReadAsAsync<List<EmployeeDetail>>().Result.ToList();
            //IQueryable<EmployeeDetail> employeeDetails = message.Content.ReadAsAsync(IQueryable<EmployeeDetail>);
            DataTable dt = new DataTable("EmployeeList");
            EmployeeDetail datatable = new EmployeeDetail();

            Type t = datatable.GetType();

            // EmployeeDetails1Controller.GetDataTableFromObjects(details);

            foreach (PropertyInfo pi in t.GetProperties())
            {
                dt.Columns.Add(new DataColumn(pi.Name));
            }
            foreach (var item in details)
            {
                DataRow dr = dt.NewRow();

                foreach (DataColumn dc in dt.Columns)
                {

                    dr[dc.ColumnName] = item.GetType().GetProperty(dc.ColumnName).GetValue(item, null);

                }

                dt.Rows.Add(dr);


            }
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr[dc.ColumnName] = t.GetType().GetProperty(dc.ColumnName).GetValue(t, null);

            //}
            return View(details);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient client = new HttpClient();
            string baseUrl = ConfigurationManager.AppSettings["AppKey"].ToString();
            client.BaseAddress = new Uri(baseUrl);
            Task<HttpResponseMessage> message = client.GetAsync("api/EmployeeDetails/GetEmployeeDetails?id=" + id);
            HttpResponseMessage httpResponse = message.Result;

            EmployeeDetail details = httpResponse.Content.ReadAsAsync<EmployeeDetail>().Result;

            // EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (details == null)
            {
                return HttpNotFound();
            }
            // return View(details);
            return PartialView("Ajax_Details", details);
        }

     
        public ActionResult Create()
        {
         //   return View();
            return PartialView("Ajax_Create");
        }

      [HttpPost]
        public ActionResult Create(EmployeeDetail employeeDetail)
        {
            

            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                string baseUrl = ConfigurationManager.AppSettings["AppKey"].ToString();
                client.BaseAddress = new Uri("http://localhost:58336/");
                var data = JsonConvert.SerializeObject(employeeDetail);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> message = client.PostAsync("api/EmployeeDetails/PostEmployeeDetail?employeeDetail="+ employeeDetail, content);

                HttpResponseMessage httpResponse = message.Result;
                //db.EmployeeDetails.Add(employeeDetail);

               EmployeeDetail ob = httpResponse.Content.ReadAsAsync<EmployeeDetail>().Result;

               
               
                return RedirectToAction("Index");
            }

            return View(employeeDetail);
        }

  
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient client = new HttpClient();
            string baseUrl = ConfigurationManager.AppSettings["AppKey"].ToString();
            client.BaseAddress = new Uri(baseUrl);
            Task<HttpResponseMessage> message = client.GetAsync("api/EmployeeDetails/GetEmployeeDetails?id="+id);
            HttpResponseMessage httpResponse = message.Result;

            EmployeeDetail details = httpResponse.Content.ReadAsAsync<EmployeeDetail>().Result;

            // EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (details == null)
            {
                return HttpNotFound();
            }
            return PartialView("Ajax_Edit", details);
           // return View(details);
        }

   [HttpPost]
        public ActionResult Edit( EmployeeDetail employeeDetail)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                string baseUrl = ConfigurationManager.AppSettings["AppKey"].ToString();
                client.BaseAddress = new Uri("http://localhost:58336/");
                var content = new StringContent(JsonConvert.SerializeObject(employeeDetail), Encoding.UTF8, "application/json");

                Task<HttpResponseMessage> message = client.PutAsync("api/EmployeeDetails/PutEmployeeDetail?id="+employeeDetail.EmpId,content);
                HttpResponseMessage httpResponse = message.Result;

                return RedirectToAction("Index");
            }
            return View(employeeDetail);
        }

   
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient client = new HttpClient();
            string baseUrl = ConfigurationManager.AppSettings["AppKey"].ToString();
            client.BaseAddress = new Uri(baseUrl);
            Task<HttpResponseMessage> message = client.GetAsync("api/EmployeeDetails/GetEmployeeDetails?id=" + id);
            HttpResponseMessage httpResponse = message.Result;

            EmployeeDetail details = httpResponse.Content.ReadAsAsync<EmployeeDetail>().Result;
            if (details == null)
            {
                return HttpNotFound();
            }
            return View(details);
        }

        // POST: EmployeeDetails1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            db.EmployeeDetails.Remove(employeeDetail);
            db.SaveChanges();*/
            HttpClient client = new HttpClient();
            string baseUrl = ConfigurationManager.AppSettings["AppKey"].ToString();
            client.BaseAddress = new Uri(baseUrl);
            Task<HttpResponseMessage> message = client.DeleteAsync("api/EmployeeDetails/DeleteEmployeeDetail?id=" + id);
            HttpResponseMessage httpResponse = message.Result;
           EmployeeDetail employee =  httpResponse.Content.ReadAsAsync<EmployeeDetail>().Result;
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

        public ActionResult Hi(int i, string nam)
        {
            return View("jdfsdfj");
        }


        public ActionResult Upload(FormCollection formCollection)
        {
            var usersList = new List<EmployeeDetail>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;

                    var package = new ExcelPackage(file.InputStream) { };
                    
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var user = new EmployeeDetail();
                            user.EmpId = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                            user.EmpName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            user.Department = workSheet.Cells[rowIterator, 3].Value.ToString();
                            user.Salary = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                            user.Job = workSheet.Cells[rowIterator, 5].Value.ToString();

                            usersList.Add(user);
                        }
                    
                }
            }
           
                foreach (var item in usersList)
                {
                    db.EmployeeDetails.Add(item);
                }
                db.SaveChanges();
            
            return RedirectToAction("Index");
        }

       
        public void ExportToExcel()
        {
           
            List<EmployeeDetail> liAdvisor = db.EmployeeDetails.ToList();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet wS = ep.Workbook.Worksheets.Add("EmployeeDetails");
            wS.Cells[1, 1].Value = "Emp ID";
            wS.Cells[1, 2].Value = "Emp Name";
            wS.Cells[1, 3].Value = "Department";
            wS.Cells[1, 4].Value = "Job";
            wS.Cells[1, 5].Value = "Salary";
           
            //int col = 1;
            //wS.Row(1).Style.Font.Color.SetColor(Color.Black);
            /* for (int i = 0; i < 6; i++)
             {
                 wS.Cells[1, col].Style.Font.Bold = true;

                 col++;
             }*/
            // wS.Cells["A2"].LoadFromCollection(liAdvisor);
            int row = 2;
            foreach (var item in liAdvisor)
            {
                wS.Cells[row, 1].Value = item.EmpId;
                wS.Cells[row, 2].Value = item.EmpName;
                wS.Cells[row, 3].Value = item.Department;
                wS.Cells[row, 4].Value = item.Job;
                wS.Cells[row, 5].Value = item.Salary;
                
                row++;
            }
            string excelName = $"EmployeeDetails-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            wS.Column(1).AutoFit();
            wS.Column(2).AutoFit();
            wS.Column(3).AutoFit();
            wS.Column(4).AutoFit();
            wS.Column(5).AutoFit();
            wS.Column(6).AutoFit();
            wS.Column(7).AutoFit();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + excelName);
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();

        }
    }
}
