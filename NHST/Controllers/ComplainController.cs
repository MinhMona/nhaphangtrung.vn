using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using MB.Extensions;
using WebUI.Business;
using System.Data;
using System.Globalization;

namespace NHST.Controllers
{
    public class ComplainController
    {
        #region CRUD
        public static string Insert(int UID, int OrderID, string Amount, string IMG, string ComplainText, int Status, DateTime CreatedDate, string CreatedBy, int TypeComplain)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_Complain c = new tbl_Complain();
                c.UID = UID;
                c.OrderID = OrderID;
                c.Amount = Amount;
                c.IMG = IMG;
                c.ComplainText = ComplainText;
                c.Status = Status;
                c.CreatedDate = CreatedDate;
                c.CreatedBy = CreatedBy;
                c.TypeComplain = TypeComplain;
                dbe.tbl_Complain.Add(c);
                dbe.SaveChanges();
                string kq = c.ID.ToString();
                return kq;
            }
        }
        public static string Update(int ID, string Amount, int Status, DateTime ModifiedDate, string ModifiedBy, string ComplainText)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_Complain.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    c.Amount = Amount;
                    c.Status = Status;
                    c.ComplainText = ComplainText;
                    c.ModifiedDate = ModifiedDate;
                    c.ModifiedBy = ModifiedBy;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateAmount(int ID, string Amount, DateTime ModifiedDate, string ModifiedBy, string ComplainText)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_Complain.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    c.Amount = Amount;
                    c.ComplainText = ComplainText;
                    c.ModifiedDate = ModifiedDate;
                    c.ModifiedBy = ModifiedBy;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateAdminStatus(int ID, int Status)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_Complain.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    c.AdminStatus = Status;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static List<tbl_Complain> GetByUID(int UID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Complain> cs = new List<tbl_Complain>();
                cs = dbe.tbl_Complain.Where(c => c.UID == UID).OrderByDescending(c => c.ID).ToList();
                return cs;
            }
        }
        public static List<tbl_Complain> GetAll(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Complain> cs = new List<tbl_Complain>();
                cs = dbe.tbl_Complain.Where(c => c.CreatedBy.Contains(s)).OrderByDescending(c => c.ID).ToList();
                return cs;
            }
        }
        public static List<tbl_Complain> GetAllByOrderShopCodeAndUID(int UID, int OrderID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Complain> cs = new List<tbl_Complain>();
                cs = dbe.tbl_Complain.Where(c => c.UID == UID && c.OrderID == OrderID).OrderByDescending(c => c.ID).ToList();
                return cs;
            }
        }
        public static List<tbl_Complain> GetAllByOrderShopCode(int OrderID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Complain> cs = new List<tbl_Complain>();
                cs = dbe.tbl_Complain.Where(c => c.OrderID == OrderID).OrderByDescending(c => c.ID).ToList();
                return cs;
            }
        }
        public static tbl_Complain GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_Complain.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    return c;
                }
                else
                    return null;
            }
        }
        public static string Delete(int ID)
        {
            using (var db = new NHSTEntities())
            {
                var p = db.tbl_Complain.Where(x => x.ID == ID).SingleOrDefault();
                if (p != null)
                {
                    db.tbl_Complain.Remove(p);
                    db.SaveChanges();
                    return "ok";
                }
                return null;
            }
        }
        #endregion

        public static List<tbl_Complain> GetByUID_SQL(int UID)
        {
            var list = new List<tbl_Complain>();
            var sql = @"select * from tbl_Complain ";
            sql += " where UID = " + UID + "";
            sql += " Order By ID desc";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);

            int i = 1;
            while (reader.Read())
            {
                var entity = new tbl_Complain();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);
                if (reader["OrderID"] != DBNull.Value)
                    entity.OrderID = reader["OrderID"].ToString().ToInt(0);

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["Amount"] != DBNull.Value)
                    entity.Amount = reader["Amount"].ToString();

                if (reader["ComplainText"] != DBNull.Value)
                    entity.ComplainText = reader["ComplainText"].ToString();

                if (reader["ProductID"] != DBNull.Value)
                    entity.ProductID = reader["ProductID"].ToString().ToInt(0);

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["OrderShopCode"] != DBNull.Value)
                    entity.OrderShopCode = reader["OrderShopCode"].ToString();

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();

                if (reader["OrderCode"] != DBNull.Value)
                    entity.OrderCode = reader["OrderCode"].ToString();
                if (reader["IMG"] != DBNull.Value)
                    entity.IMG = reader["IMG"].ToString();

                i++;
                list.Add(entity);
            }
            reader.Close();
            return list;

        }
        public static int GetTotalAdmin(string s, string fd, string td, int dathangID, int cskhID)
        {
            var sql = @" select Total=Count(*) ";
            sql += " from tbl_Complain as cp ";
            sql += " left outer join tbl_MainOder as mo ON mo.ID = cp.OrderID ";
            sql += " left outer join tbl_Account as dathang ON mo.DathangID = dathang.ID ";
            sql += " left outer join  tbl_Account as cskh ON mo.CSID = cskh.ID ";
            sql += " Where cp.CreatedBy LIKE N'%" + s + "%' AND cp.Status > 2 AND cp.AdminStatus is not null ";
            if (dathangID > 0)
            {
                sql += " AND dathang.ID=" + dathangID + " ";
            }
            if (cskhID > 0)
            {
                sql += " AND cskh.ID=" + cskhID + " ";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
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
        public static int GetTotal(string s, string fd, string td, int Status, int dathangID, int cskhID)
        {
            var sql = @"select Total=Count(*) ";
            sql += " from tbl_Complain as cp ";
            sql += " left outer join tbl_MainOder as mo ON mo.ID = cp.OrderID ";
            sql += " left outer join tbl_Account as dathang ON mo.DathangID = dathang.ID ";
            sql += " left outer join  tbl_Account as cskh ON mo.CSID = cskh.ID ";
            sql += " Where cp.CreatedBy LIKE N'%" + s + "%' ";
            if (Status > -1)
            {
                sql += " AND cp.Status=" + Status + " ";
            }
            if (dathangID > 0)
            {
                sql += " AND dathang.ID=" + dathangID + " ";
            }
            if (cskhID > 0)
            {
                sql += " AND cskh.ID=" + cskhID + " ";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
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
        
        public static List<ListComplain> GetAllBySQLAdmin(string s, int pageIndex, int pageSize, string fd, string td, int dathangID, int cskhID)
        {
            var sql = @"select cp.*, dathang.Username as DathangName, cskh.Username as CskhName ";
            sql += " from tbl_Complain as cp ";
            sql += " left outer join tbl_MainOder as mo ON mo.ID = cp.OrderID ";
            sql += " left outer join tbl_Account as dathang ON mo.DathangID = dathang.ID ";
            sql += " left outer join  tbl_Account as cskh ON mo.CSID = cskh.ID ";
            sql += " Where cp.CreatedBy LIKE N'%" + s + "%' AND cp.Status > 2 AND cp.AdminStatus is not null ";
            if (dathangID > 0)
            {
                sql += " AND dathang.ID=" + dathangID + " ";
            }
            if (cskhID > 0)
            {
                sql += " AND cskh.ID=" + cskhID + " ";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += "order by cp.ID DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<ListComplain> a = new List<ListComplain>();
            while (reader.Read())
            {
                var entity = new ListComplain();

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                int count = 0;
                int MainOrderID = 0;
                if (reader["OrderID"] != DBNull.Value)
                    MainOrderID = reader["OrderID"].ToString().ToInt(0);
                entity.MainOrderID = MainOrderID;

                if (MainOrderID > 0)
                {
                    var com = ComplainController.GetAllByOrderShopCode(Convert.ToInt32(MainOrderID));
                    if (com != null)
                    {
                        if (com.Count > 0)
                        {
                            count = com.Count;
                        }
                    }
                }
                entity.Quantity = count;

                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.Username = reader["CreatedBy"].ToString();

                if (reader["DathangName"] != DBNull.Value)
                    entity.DathangName = reader["DathangName"].ToString();

                if (reader["CskhName"] != DBNull.Value)
                    entity.CskhName = reader["CskhName"].ToString();

                if (reader["ComplainText"] != DBNull.Value)
                    entity.ComplainText = reader["ComplainText"].ToString();

                int stt = 0;
                if (reader["Status"] != DBNull.Value)
                    stt = Convert.ToInt32(reader["Status"].ToString());
                entity.Status = stt;
                entity.StatusString = PJUtils.ReturnStatusCompalint(stt);

                int type = 0;
                if (reader["TypeComplain"] != DBNull.Value)
                    type = Convert.ToInt32(reader["TypeComplain"].ToString());
                entity.TypeComplain = type;
                entity.TypeString = PJUtils.ReturnTypeCompalint(type);

                int adminstt = 0;
                if (reader["AdminStatus"] != DBNull.Value)
                    adminstt = Convert.ToInt32(reader["AdminStatus"].ToString());
                entity.AdminStatus = adminstt;
                entity.AdminStatusString = PJUtils.ReturnAdminStatusCompalint(adminstt);

                double Amount = 0;
                if (reader["Amount"] != DBNull.Value)
                    Amount = Convert.ToDouble(reader["Amount"].ToString());
                entity.Amount = string.Format("{0:N0}", Convert.ToDouble(Amount));

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                a.Add(entity);
            }
            reader.Close();
            return a;
        }

        public static List<ListComplain> GetAllBySQL(string s, int pageIndex, int pageSize, string fd, string td, int Status, int dathangID, int cskhID)
        {
            var sql = @"select cp.*, dathang.Username as DathangName, cskh.Username as CskhName ";
            sql += " from tbl_Complain as cp ";
            sql += " left outer join tbl_MainOder as mo ON mo.ID = cp.OrderID ";
            sql += " left outer join tbl_Account as dathang ON mo.DathangID = dathang.ID ";
            sql += " left outer join  tbl_Account as cskh ON mo.CSID = cskh.ID ";
            sql += " Where cp.CreatedBy LIKE N'%" + s + "%' ";
            if (Status > -1)
            {
                sql += " AND cp.Status=" + Status + " ";
            }
            if (dathangID > 0)
            {
                sql += " AND dathang.ID=" + dathangID + " ";
            }
            if (cskhID > 0)
            {
                sql += " AND cskh.ID=" + cskhID + " ";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND cp.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += "order by cp.ID DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<ListComplain> a = new List<ListComplain>();
            while (reader.Read())
            {
                var entity = new ListComplain();

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                int count = 0;
                int MainOrderID = 0;
                if (reader["OrderID"] != DBNull.Value)
                    MainOrderID = reader["OrderID"].ToString().ToInt(0);
                entity.MainOrderID = MainOrderID;

                if (MainOrderID > 0)
                {
                    var com = ComplainController.GetAllByOrderShopCode(Convert.ToInt32(MainOrderID));
                    if (com != null)
                    {
                        if (com.Count > 0)
                        {
                            count = com.Count;
                        }
                    }
                }
                entity.Quantity = count;

                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.Username = reader["CreatedBy"].ToString();

                if (reader["DathangName"] != DBNull.Value)
                    entity.DathangName = reader["DathangName"].ToString();

                if (reader["CskhName"] != DBNull.Value)
                    entity.CskhName = reader["CskhName"].ToString();

                if (reader["ComplainText"] != DBNull.Value)
                    entity.ComplainText = reader["ComplainText"].ToString();

                int stt = 0;
                if (reader["Status"] != DBNull.Value)
                    stt = Convert.ToInt32(reader["Status"].ToString());
                entity.Status = stt;
                entity.StatusString = PJUtils.ReturnStatusCompalint(stt);

                int type = 0;
                if (reader["TypeComplain"] != DBNull.Value)
                    type = Convert.ToInt32(reader["TypeComplain"].ToString());
                entity.TypeComplain = type;
                entity.TypeString = PJUtils.ReturnTypeCompalint(type);

                double Amount = 0;
                if (reader["Amount"] != DBNull.Value)               
                    Amount = Convert.ToDouble(reader["Amount"].ToString());                                  
                entity.Amount = string.Format("{0:N0}", Convert.ToDouble(Amount));

                if (reader["CreatedDate"] != DBNull.Value)               
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());                       

                a.Add(entity);
            }
            reader.Close();
            return a;
        }

        public class ListComplain
        {
            public int ID { get; set; }
            public int UID { get; set; }
            public string Username { get; set; }
            public string DathangName { get; set; }
            public string CskhName { get; set; }
            public int MainOrderID { get; set; }    
            public int Status { get; set; }
            public int AdminStatus { get; set; }
            public string AdminStatusString { get; set; }
            public int TypeComplain { get; set; }
            public int Quantity { get; set; }
            public string StatusString { get; set; }
            public string TypeString { get; set; }
            public string Amount { get; set; }
            public string ComplainText { get; set; }
            public DateTime CreatedDate { get; set; }           
        }

    }
}