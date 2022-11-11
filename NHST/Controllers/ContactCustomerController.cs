using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using System.Data;
using WebUI.Business;
using MB.Extensions;

namespace NHST.Controllers
{
    public class ContactCustomerController
    {
        #region CRUD
        public static tbl_ContactCustomer InsertNew(string fullname, string phone, string note, string email)
        {
            using (var db = new NHSTEntities())
            {
                tbl_ContactCustomer sv = new tbl_ContactCustomer();
                sv.FullName = fullname;
                sv.Phone = phone;
                sv.Email = email;
                sv.Note = note;
                sv.CreatedDate = DateTime.Now;
                sv.Status = 1;
                db.tbl_ContactCustomer.Add(sv);
                db.SaveChanges();
                return sv;
            }
        }

        public static string Update(int ID, int Status, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_ContactCustomer.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    c.Status = Status;
                    c.ModifiedDate = ModifiedDate;
                    c.ModifiedBy = ModifiedBy;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static int GetTotal(string s, string phone, int Status)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_ContactCustomer ";
            sql += "Where  FullName like N'%" + s + "%' and Phone like N'%" + phone + "%'";
            if (Status > 0)
            {
                sql += " And Status=" + Status + " ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);

            }
            reader.Close();
            return a;
        }

        public static List<tbl_ContactCustomer> GetAllBySQL(string s, string phone, int pageIndex, int pageSize, int Status)
        {
            var sql = @"select * ";
            sql += "from tbl_ContactCustomer ";
            sql += "Where FullName Like N'%" + s + "%' and Phone Like N'%" + phone + "%'";
            if (Status > 0)
            {
                sql += " And Status=" + Status + " ";
            }
            sql += "order by id DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_ContactCustomer> a = new List<tbl_ContactCustomer>();
            while (reader.Read())
            {
                var entity = new tbl_ContactCustomer();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["FullName"] != DBNull.Value)
                    entity.FullName = reader["FullName"].ToString();

                if (reader["Phone"] != DBNull.Value)
                    entity.Phone = reader["Phone"].ToString();

                if (reader["Note"] != DBNull.Value)
                    entity.Note = reader["Note"].ToString();

                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                if (reader["Status"] != DBNull.Value)
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());

                a.Add(entity);
            }
            reader.Close();
            return a;
        }

        public static List<tbl_ContactCustomer> GetAll()
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_ContactCustomer.ToList();
                if (sv.Count > 0)
                    return sv;
                return null;
            }
        }

        public static tbl_ContactCustomer GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_ContactCustomer.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    return c;
                }
                else
                    return null;
            }
        }

        public static string Insert(string Fullname, string Email, string Phone, string ContactContent, bool IsRead, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_Contact p = new tbl_Contact();
                p.Fullname = Fullname;
                p.Email = Email;
                p.Phone = Phone;
                p.ContactContent = ContactContent;
                p.IsRead = IsRead;
                p.CreatedDate = CreatedDate;
                p.CreatedBy = CreatedBy;
                dbe.tbl_Contact.Add(p);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = p.ID.ToString();
                return k;
            }
        }

        public static string Update(int ID, bool IsRead, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var p = dbe.tbl_Contact.Where(pa => pa.ID == ID).SingleOrDefault();
                if (p != null)
                {
                    p.IsRead = IsRead;
                    p.ModifiedBy = ModifiedBy;
                    p.ModifiedDate = ModifiedDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string Delete(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var p = dbe.tbl_Menu.Where(pa => pa.ID == ID).SingleOrDefault();
                if (p != null)
                {
                    dbe.tbl_Menu.Remove(p);
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static List<tbl_Contact> GetAll(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Contact> pages = new List<tbl_Contact>();
                pages = dbe.tbl_Contact.Where(p => p.Fullname.Contains(s)).OrderByDescending(a => a.CreatedDate).ToList();
                return pages;
            }
        }
        #endregion
    }
}